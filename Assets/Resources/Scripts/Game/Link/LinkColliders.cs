using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkColliders : MonoBehaviour {
	LinkManager manager;
	
	void Start() {
		manager = GetComponent<LinkManager>();
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		var obj = collider.gameObject;
		if ((obj.tag == "SnakeRay" || obj.tag == "Explosion" || obj.tag == "LinkInstakill") && PhotonNetwork.isMasterClient) {
			StartCoroutine(Die());
		}
	}

	IEnumerator Die() {
		manager.photonView.RPC("Die", PhotonTargets.All);
		yield return new WaitForSeconds(1f);
		StartCoroutine(GameOverManager.instance.EndGame(false));
	}
}
