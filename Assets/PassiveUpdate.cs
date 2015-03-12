using UnityEngine;
using System.Collections;

public class PassiveUpdate {
	public Transform trf;
	public bool enabled = true;

	float rotationRate;
	
	float rotation;
	
	public PassiveUpdate(Transform transform) {
		trf = transform;
		rotationRate = (Random.value - 0.5f) * 10f;
	}
	
	public void Update() {
		if(!enabled) return;
		rotation += rotationRate + Time.deltaTime;
		trf.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
		trf.position = trf.position + trf.up * Time.deltaTime;
	}
}
