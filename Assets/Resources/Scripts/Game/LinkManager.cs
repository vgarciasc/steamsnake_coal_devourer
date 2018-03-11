using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinkManager : Photon.PunBehaviour {

	public List<LinkFOV> linkFOVs = new List<LinkFOV>();
	
	[SerializeField]
	GameObject heldObjectContainer;
	[SerializeField]
	GameObject heldObjectPrefab;
	[SerializeField]
	GameObject heldObject;

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

	void HandleHolding() {
		if (Input.GetKeyDown(KeyCode.E)) {
			var seen_obj = SeenObjects();
			if (heldObject == null && seen_obj.Count > 0) {
				photonView.RPC("StartHolding", PhotonTargets.All, seen_obj[0].GetPhotonView().viewID);
			}
			else if (heldObject != null) {
				photonView.RPC("StopHolding", PhotonTargets.All);
			}
		}
	}

	[PunRPC]
	void StartHolding(int id) {
		heldObject = PhotonView.Find(id).gameObject;
		heldObject.transform.SetParent(heldObjectContainer.transform);
		heldObject.transform.DOMove(heldObjectContainer.transform.position, 0.2f);
	}

	[PunRPC]
	void StopHolding() {
		heldObject.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
		heldObject.transform.DOMove(this.transform.position + Vector3.right * 0.25f, 0.2f);
		heldObject = null;
	}

}
