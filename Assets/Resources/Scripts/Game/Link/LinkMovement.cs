using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMovement : Photon.PunBehaviour, IPunObservable {

	public Direction currentDirection = Direction.NONE;

	bool isLink;
	bool isBoss;

	Rigidbody2D rb;
	Animator animator;
	SpriteRenderer sr;
	float speed = 2f;

	bool flipSprite;
	LinkManager manager;
	
	Vector3 lastFootstepPlace = Vector3.zero;

	[Header("References")]
	[SerializeField]
	GameObject footstepsOrigin;
	[SerializeField]
	GameObject footstepsPrefab;

	[Header("Mechanics")]
	[Range(0f, 1f)]
	public float holdingEncumberance = 0.25f;

	public bool canMove = true;

	void Start () {
		isLink = (bool) photonView.owner.CustomProperties["is_link"];
		isBoss = (bool) photonView.owner.CustomProperties["is_boss"];

		rb = this.GetComponentInChildren<Rigidbody2D>();
		animator = this.GetComponentInChildren<Animator>();
		sr = this.GetComponentInChildren<SpriteRenderer>();
		manager = this.GetComponent<LinkManager>();

		this.transform.position = GamePlayerManager.instance.GetLinkSpawn();
	}

	void Update () {
		HandleAnimation();

		if (!photonView.isMine) {
			return;
		};

		if (canMove) {
			HandleMovement();
		}
		
		HandleFootsteps();
	}

	void HandleMovement() {
		float horizontal = Input.GetAxis("Horizontal");		
		float vertical = Input.GetAxis("Vertical");

		rb.velocity = new Vector3(
			horizontal,
			vertical
		) * speed * (manager.heldObject == null ? 1f : holdingEncumberance);

		if (rb.velocity.x != 0) flipSprite = (rb.velocity.x < 0f);
		currentDirection = (flipSprite ? Direction.RIGHT : Direction.LEFT);
	}

	void HandleAnimation() {
		animator.SetBool("walking", rb.velocity.magnitude > 0.5f);
		sr.flipX = flipSprite;
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
		    stream.SendNext(flipSprite);
			stream.SendNext(currentDirection);
		}
		else {
			flipSprite = (bool) stream.ReceiveNext();
			currentDirection = (Direction) stream.ReceiveNext();
		}
    }

	void HandleFootsteps() {
		if (Vector3.Distance(lastFootstepPlace, this.transform.position) > 0.5f) {
			photonView.RPC("PlaceFootstep",
				PhotonTargets.All,
				footstepsOrigin.transform.position,
				Vector3.Angle(lastFootstepPlace, this.transform.position) * Mathf.Rad2Deg + 90);
			lastFootstepPlace = this.transform.position;
		}
	}
	
	[PunRPC]
	void PlaceFootstep(Vector3 position, float rotation) {
		var obj = Instantiate(footstepsPrefab, position, Quaternion.identity);
		obj.transform.Rotate(new Vector3(0, 0, rotation));
	}
}
