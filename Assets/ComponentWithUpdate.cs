using UnityEngine;
using System.Collections;

public class ComponentWithUpdate : MonoBehaviour {
	int updates = 0;

	
	void Update () {
		updates++;
	}
}
