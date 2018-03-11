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
	GameObject heldObject;

	LinkMovement linkMovement;

	void Start() {
		linkMovement = this.GetComponent<LinkMovement>();
	}

	void Update () {
		if (photonView.isMine) {
			HandleHolding();
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
			else if (heldObject != null) {
				photonView.RPC("PutDownObject", PhotonTargets.All);
			}
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			if (heldObject != null) {
				photonView.RPC("ThrowObject", PhotonTargets.All);
			}
		}
	}

	public LinkFOV GetCurrentLinkFOV() {
		return linkFOVs[linkMovement.currentDirection == Direction.LEFT ? 1 : 3];
	}

	[PunRPC]
	void HoldObject(int id) {
		heldObject = PhotonView.Find(id).gameObject;
		heldObject.transform.SetParent(heldObjectContainer.transform);
		// heldObject.transform.DOMove(heldObjectContainer.transform.position, 0.2f);
		heldObject.transform.position = heldObjectContainer.transform.position;
		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
	}

	[PunRPC]
	void PutDownObject() {
		if (GetCurrentLinkFOV().ContainsWall()) {
			return;
		}

		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		heldObject.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);

		Vector3 downPos = GetCurrentLinkFOV().transform.position;
		heldObject.transform.position = downPos;
		// heldObject.transform.DOMove(downPos, 0.2f);
		heldObject = null;
	}

	[PunRPC]
	void ThrowObject() {
		if (GetCurrentLinkFOV().ContainsWall()) {
			return;
		}

		float throwModifier = 10f;

		heldObject.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		heldObject.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);

		float flip = linkMovement.currentDirection == Direction.LEFT ? 1f : -1f;
		heldObject.GetComponent<Rigidbody2D>().velocity = new Vector3(throwModifier * flip, -2f);

		heldObject = null;
	}
}
