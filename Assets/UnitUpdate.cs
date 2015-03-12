using UnityEngine;
using System.Collections;

public class UnitUpdate : MonoBehaviour {
	float rotationRate;

	float rotation;

	Transform trf;

	void Awake() {
		rotationRate = (Random.value - 0.5f) * 10f;
		trf = transform;
	}
	
	void Update () {
		rotation += rotationRate + Time.deltaTime;
		trf.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
		trf.position = trf.position + trf.up * Time.deltaTime;
	}
}
