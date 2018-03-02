using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOutOfEditor : MonoBehaviour {
	void Start () {
		if (Application.isPlaying) {
			this.gameObject.SetActive(false);
		}
	}
}
