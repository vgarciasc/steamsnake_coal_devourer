using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Direction { NONE, TOP, RIGHT, BOTTOM, LEFT };
public class LinkManager : Photon.PunBehaviour {

	[Header("References")]
	[SerializeField]
	List<LinkFOV> linkFOVs = new List<LinkFOV>();
	[SerializeField]
	GameObject heldObjectContainer;
	[SerializeField]
	GameObject heldObjectPrefab;
	[SerializeField]
	GameObject bloodSplatter;

	public GameObject heldObject;
	public float viewRadius;
	public bool isDead = false;

	LinkMovement linkMovement;
	SpriteRenderer sr;
	Rigidbody2D rb;
	SpecialCamera specialCamera;

	SteamsnakeManager steamsnakeManager;

	void Start() {
		linkMovement = this.GetComponent<LinkMovement>();
		sr = this.GetComponentInChildren<SpriteRenderer>();
		rb = this.GetComponentInChildren<Rigidbody2D>();

		specialCamera = Camera.main.GetComponentInParent<SpecialCamera>();
	}

	void Update () {
		if (photonView.isMine) {
			HandleHolding();
			HandleVisibility();
			HandleSnakeShake();
		}
	}

	List<GameObject> SeenObjects() {
		List<GameObject> output = new List<GameObject>();
		foreach (var fov in linkFOVs) {
			output.AddRange(fov.seen);
		}

		return output;
	}

	List<GameObject> HoldableSeenObjects() {
		var output = new List<GameObject>();
		foreach (var s in SeenObjects()) {
			if (s.GetComponentInChildren<Holdable>()) {
				output.Add(s);
			}
		}

		return output;
	}

	void HandleHolding() {
		if (Input.GetKeyDown(KeyCode.E)) {
			var seen_obj = HoldableSeenObjects();
			if (heldObject == null && seen_obj.Count > 0) {
				photonView.RPC("HoldObject", PhotonTargets.All, seen_obj[0].GetPhotonView().viewID);
			}
			else if (heldObject != null && !GetCurrentLinkFOV().ContainsWall()) {
				photonView.RPC("PutDownObject", PhotonTargets.All);
			}
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			if (heldObject != null && !GetCurrentLinkFOV().ContainsWall()) {
				photonView.RPC("ThrowObject", PhotonTargets.All, linkMovement.currentDirection);
			}
		}
	}

	public LinkFOV GetCurrentLinkFOV() {
		switch (linkMovement.currentDirection) {
			case Direction.LEFT: return linkFOVs[1];
			case Direction.RIGHT: return linkFOVs[3];
			case Direction.TOP: return linkFOVs[0];
			case Direction.BOTTOM: return linkFOVs[2];
		}
		return linkFOVs[0];
	}

	[PunRPC]
	void HoldObject(int id) {
		heldObject = PhotonView.Find(id).gameObject;
		heldObject.transform.SetParent(heldObjectContainer.transform);
		// heldObject.transform.DOMove(heldObjectContainer.transform.position, 0.2f);
		heldObject.transform.position = heldObjectContainer.transform.position;
		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

		var bomb = heldObject.GetComponentInChildren<Bomb>();
		if (bomb != null) {
			bomb.StartExploding();
		}
	}

	[PunRPC]
	void PutDownObject() {
		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		heldObject.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);

		Vector3 downPos = GetCurrentLinkFOV().transform.position;
		heldObject.transform.position = downPos;
		// heldObject.transform.DOMove(downPos, 0.2f);
		heldObject = null;
	}

	[PunRPC]
	void ThrowObject(Direction dir) {
		float throwModifier = 10f;

		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		heldObject.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);

		var vec = GetVectorFromDirection(dir) * throwModifier;
		if (dir == Direction.LEFT || dir == Direction.RIGHT) 
			vec -= 2 * Vector3.one;

		heldObject.GetComponent<Rigidbody2D>().velocity = vec;

		heldObject = null;
	}

	Vector3 GetVectorFromDirection(Direction direction) {
		switch (direction) {
			case Direction.TOP:
				return Vector3.up;
			case Direction.BOTTOM:
				return Vector3.down;
			case Direction.RIGHT:
				return Vector3.right;
			case Direction.LEFT:
				return Vector3.left;
		}

		return Vector3.zero;
	}

	void HandleVisibility() {
		var invisibles = FindObjectsOfType<InvisibleToLink>();
		foreach (var inv in invisibles) {
			inv.GetComponent<SpriteRenderer>().enabled = (
				(this.transform.position - inv.transform.position).magnitude < viewRadius
			);
		}
	}

	[PunRPC]
	public void Die() {
		var obj = Instantiate(bloodSplatter, this.transform.position, Quaternion.identity);
		obj.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
		sr.enabled = false;
		rb.velocity = Vector3.zero;
		linkMovement.canMove = false;
		isDead = true;
	}

	void HandleSnakeShake() {
		if (steamsnakeManager == null) {
			var aux = GameObject.FindGameObjectWithTag("Snake");
			if (aux == null) return;
			steamsnakeManager = aux.GetComponentInChildren<SteamsnakeManager>();
		}

		if (steamsnakeManager != null) {
			if (steamsnakeManager.movement.isMoving) {
				float distance = Vector3.Distance(steamsnakeManager.movement.GetHead().transform.position, this.transform.position);
				float threshold = 5f;
				float power = (-1 / threshold) * distance + 1f;
				power = Mathf.Clamp(power, 0f, 1f);
				if (distance > threshold) return;
				specialCamera.screenShake_(power);
			}

			if (steamsnakeManager.isShootingRay) {
				float power = 0.7f;
				specialCamera.screenShake_(power);
			}
		}
	}
}
