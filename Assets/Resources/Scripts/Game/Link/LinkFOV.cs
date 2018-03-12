using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkFOV : MonoBehaviour {
	public List<GameObject> seen = new List<GameObject>();

	void OnTriggerEnter2D(Collider2D collider) {
		var obj = collider.gameObject;
		if (!seen.Contains(obj) && obj != null) {
			seen.Add(obj);
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		var obj = collider.gameObject;
		if (seen.Contains(obj) && obj != null) {
			seen.Remove(obj);
		}
	}

	public bool ContainsWall() {
		foreach (GameObject go in seen) {
			if (LayerMask.LayerToName(go.layer) == "Wall") {
				return true;
			}
		}

		return false;
	}
}
