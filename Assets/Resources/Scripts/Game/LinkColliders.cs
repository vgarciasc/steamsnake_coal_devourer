using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkColliders : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		var obj = collider.gameObject;
		if (obj.tag == "SnakeRay" && PhotonNetwork.isMasterClient) {
			StartCoroutine(GameOverManager.instance.EndGame(false));
		}
	}
}
