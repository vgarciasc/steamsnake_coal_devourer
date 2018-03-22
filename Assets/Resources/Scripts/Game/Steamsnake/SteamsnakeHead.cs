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

	void OnTriggerEnter2D(Collider2D collider) {
		var obj = collider.gameObject;
		if (obj.tag == "Explosion" && PhotonNetwork.isMasterClient) {
			StartCoroutine(GameOverManager.instance.EndGame(true));
		}
	}
}
