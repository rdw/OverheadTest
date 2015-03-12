using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ManageBase : MonoBehaviour {
	public Transform updatePrefab;
	public Transform passivePrefab;

	enum MoveType { Update, Passive };
	string[] moveTexts;
	MoveType selectedMoveType = MoveType.Update;

	int numUnits = 10;

	int numDisabled = 1;

	int numUpdateComponents = 0;
	int numNonupdateComponents = 0;

	List<Transform> units = new List<Transform>();

	int frames = 0;
	float lastSeconds = 0;

	int fps = 0;

	List<PassiveUpdate> passives = new List<PassiveUpdate>();

	string numUnitsText = "10";
	string numDisabledText = "1";
	string numUpdateComponentsText = "0";
	string numNonupdateComponentsText = "0";

	void Awake() {
		moveTexts = Enum.GetNames(typeof(MoveType));
		Regenerate();
	}

	void OnGUI() {
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, 20));
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("FPS: {0,3}", fps));

		GUILayout.Label("Units");
		numUnitsText = GUILayout.TextField(numUnitsText);
		try {
			var newUnits = Convert.ToInt32(numUnitsText);
			if(newUnits != numUnits) {
				numUnits = newUnits;
				if(numUnits < 0) numUnits = 0;
				Regenerate();
			}
		} catch(FormatException e) {
		}

		GUILayout.Label("Disabled");
		numDisabledText = GUILayout.TextField(numDisabledText);
		try {
			var newDisabled = Convert.ToInt32(numDisabledText);
			if(newDisabled != numDisabled) {
				numDisabled = newDisabled;
				if(numDisabled < 0) numDisabled = 0;
				Regenerate();
			}
		} catch(FormatException e) {
		}

		GUILayout.Label("UpdateComps");
		numUpdateComponentsText = GUILayout.TextField(numUpdateComponentsText);
		try {
			var newUpdateComponents = Convert.ToInt32(numUpdateComponentsText);
			if(newUpdateComponents != numUpdateComponents) {
				numUpdateComponents = newUpdateComponents;
				Regenerate();
			}
		} catch(FormatException e) {
		}

		GUILayout.Label("PlainComps");
		numNonupdateComponentsText = GUILayout.TextField(numNonupdateComponentsText);
		try {
			var newNonupdateComponents = Convert.ToInt32(numNonupdateComponentsText);
			if(newNonupdateComponents != numNonupdateComponents) {
				numNonupdateComponents = newNonupdateComponents;
				Regenerate();
			}
		} catch(FormatException e) {
		}

		var newMoveType = (MoveType)GUILayout.SelectionGrid((int)selectedMoveType, moveTexts, 3);
		if(newMoveType != selectedMoveType) {
			for(int i = units.Count - 1; i >= 0; i--) {
				switch(selectedMoveType) {
				case MoveType.Update: Remove_Update(i); break;
				case MoveType.Passive: Remove_Passive(i); break;
				}
			}
			selectedMoveType = newMoveType;
			Regenerate();
		}

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	void Regenerate() {
		for(int i = units.Count - 1; i >= 0; i--) {
			switch(selectedMoveType) {
			case MoveType.Update: Remove_Update(i); break;
			case MoveType.Passive: Remove_Passive(i); break;
			}
		}

		for(int i = 0; i < numUnits; i++) {
			bool enabled = i >= numDisabled;
			switch(selectedMoveType) {
			case MoveType.Update: 
				Create_Update();
				SetEnabled_Update(i, enabled); 
				break;
			case MoveType.Passive:
				Create_Passive();
				SetEnabled_Passive(i, enabled); 
				break;
			}
		}
	}

	void Create_Update() {
		var trf = (Transform)Instantiate(updatePrefab);
		units.Add(trf);
		var go = trf.gameObject;
		for(int i = 0; i < numUpdateComponents; i++) {
			go.AddComponent<ComponentWithUpdate>();
		}

		for(int i = 0; i < numNonupdateComponents; i++) {
			go.AddComponent<ComponentWithoutUpdate>();
		}
	}

	void Remove_Update(int idx) {
		var last = units[idx];
		Destroy(last.gameObject);
		units.RemoveAt(idx);
	}

	void SetEnabled_Update(int idx, bool enabled) {
		units[idx].gameObject.SetActive(enabled);
	}

	void Create_Passive() {
		var trf = (Transform)Instantiate(passivePrefab);
		var pu = new PassiveUpdate(trf);
		units.Add (trf);
		passives.Add (pu);
	}

	void Remove_Passive(int idx) {
		var last = units[idx];
		Destroy(last.gameObject);
		units.RemoveAt(idx);
		passives.RemoveAt(idx);
	}

	void SetEnabled_Passive(int idx, bool enabled) {
		units[idx].gameObject.SetActive(enabled);
		passives[idx].enabled = enabled;
	}

	void Update () {
		frames++;

		if(Time.realtimeSinceStartup - lastSeconds > 1) {
			lastSeconds = lastSeconds + 1f;
			fps = frames;
			frames = 0;
		}

		if(selectedMoveType == MoveType.Passive) {
			for(int i = 0; i < passives.Count; i++) {
				passives[i].Update();
			}
		}
	}
}
