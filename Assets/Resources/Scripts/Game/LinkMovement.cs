﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMovement : Photon.PunBehaviour, IPunObservable {

	bool isLink;
	bool isBoss;

	Rigidbody2D rb;
	Animator animator;
	SpriteRenderer sr;

	bool flipSprite;

	void Start () {
		isLink = (bool) photonView.owner.CustomProperties["is_link"];
		isBoss = (bool) photonView.owner.CustomProperties["is_boss"];

		rb = this.GetComponentInChildren<Rigidbody2D>();
		animator = this.GetComponentInChildren<Animator>();
		sr = this.GetComponentInChildren<SpriteRenderer>();

		this.transform.position = GamePlayerManager.instance.linkSpawn.position;
	}
	
	void Update () {
		HandleAnimation();

		if (!photonView.isMine) {
			return;
		};

		HandleMovement();
	}

	void HandleMovement() {
		float horizontal = Input.GetAxis("Horizontal");		
		float vertical = Input.GetAxis("Vertical");

		rb.velocity = new Vector3(
			horizontal,
			vertical
		) * 2f;

		if (rb.velocity.x != 0) flipSprite = (rb.velocity.x < 0f);
	}

	void HandleAnimation() {
		animator.SetBool("walking", rb.velocity.magnitude > 0.5f);
		sr.flipX = flipSprite;
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
		    stream.SendNext(flipSprite);
		}
		else {
			flipSprite = (bool) stream.ReceiveNext();
		}
    }
}
