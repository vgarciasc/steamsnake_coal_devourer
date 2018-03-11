using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamsnakeHead : MonoBehaviour {

	public GameObject ray;

	public void RotateToDirection(Direction direction) {
		float turns = 0;
		switch (direction) {
			case Direction.RIGHT:
				turns = 1;
				break;
			case Direction.TOP:
				turns = 2;
				break;
			case Direction.LEFT:
				turns = 3;
				break;
			case Direction.BOTTOM:
				turns = 0;
				break;
		}

		this.transform.rotation = Quaternion.Euler(0, 0, turns * 90);
	}

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
