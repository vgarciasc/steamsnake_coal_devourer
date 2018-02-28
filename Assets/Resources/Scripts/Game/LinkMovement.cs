using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMovement : Photon.PunBehaviour {

	bool isLink;
	bool isBoss;

	Rigidbody2D rb;

	void Start () {
		isLink = (bool) photonView.owner.CustomProperties["is_link"];
		isBoss = (bool) photonView.owner.CustomProperties["is_boss"];
		if (isLink) GetComponent<SpriteRenderer>().color = Color.green;
		else if (isBoss) GetComponent<SpriteRenderer>().color = Color.red;

		rb = this.GetComponentInChildren<Rigidbody2D>();

		if (photonView.isMine) {
			this.transform.position = new Vector3(
				Random.Range(-1.5f, 1.5f),
				Random.Range(-1.5f, 1.5f),
				0f);
		}
	}
	
	void Update () {
		if (!photonView.isMine) return;

		HandleMovement();
		HandleLeaveRoom();
	}

	void HandleMovement() {
		float horizontal = Input.GetAxis("Horizontal");		
		float vertical = Input.GetAxis("Vertical");

		rb.velocity = new Vector3(
			horizontal,
			vertical
		) * 2f;
	}

	void HandleLeaveRoom() {
		if (Input.GetKeyDown(KeyCode.L)) {
			GamePlayerManager.ExitGame();
		}
	}
}
