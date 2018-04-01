using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableByRay : Photon.PunBehaviour {
	void OnTriggerEnter2D(Collider2D collider) {
		var obj = collider.gameObject;
		if ((obj.tag == "SnakeRay" && PhotonNetwork.isMasterClient)) {
			PhotonNetwork.Destroy(photonView);
		}
	}
}
