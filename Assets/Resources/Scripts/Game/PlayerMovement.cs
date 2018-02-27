using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.PunBehaviour {

	void Start () {
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

		this.transform.position += new Vector3(
			horizontal,
			vertical
		) * Time.deltaTime * 5f;
	}

	void HandleLeaveRoom() {
		if (Input.GetKeyDown(KeyCode.L)) {
			GamePlayerManager.ExitGame();
		}
	}
}
