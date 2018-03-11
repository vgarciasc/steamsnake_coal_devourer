using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamsnakeHead : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collision) {
		var obj = collision.gameObject;
		if (obj.tag == "Link" && PhotonNetwork.isMasterClient) {
			StartCoroutine(GameOverManager.instance.EndGame(false));
		}
		if (obj.tag == "Bomb" && PhotonNetwork.isMasterClient) {
			StartCoroutine(GameOverManager.instance.EndGame(true));
		}
	}
}
