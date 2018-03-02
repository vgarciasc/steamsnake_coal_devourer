using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMovement : Photon.PunBehaviour {

	bool isLink;
	bool isBoss;

	Rigidbody2D rb;
	Animator animator;
	SpriteRenderer sr;

	void Start () {
		isLink = (bool) photonView.owner.CustomProperties["is_link"];
		isBoss = (bool) photonView.owner.CustomProperties["is_boss"];

		rb = this.GetComponentInChildren<Rigidbody2D>();
		animator = this.GetComponentInChildren<Animator>();
		sr = this.GetComponentInChildren<SpriteRenderer>();

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
	}

	void HandleMovement() {
		float horizontal = Input.GetAxis("Horizontal");		
		float vertical = Input.GetAxis("Vertical");

		animator.SetBool("walking", rb.velocity.magnitude > 0.5f);
		if (rb.velocity.x != 0) sr.flipX = (rb.velocity.x < 0f);

		rb.velocity = new Vector3(
			horizontal,
			vertical
		) * 2f;
	}
}
