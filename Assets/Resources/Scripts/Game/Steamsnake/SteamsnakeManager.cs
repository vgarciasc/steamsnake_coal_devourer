using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamsnakeManager : Photon.PunBehaviour {

	public SteamsnakeMovement movement;

	void Start() {
		movement = this.GetComponent<SteamsnakeMovement>();
	}

	void Update () {
		if (photonView.isMine) {
			HandlePowers();
		}		
	}

	void HandlePowers() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			StartCoroutine(UseRay());
		}
	}

	IEnumerator UseRay() {
		var head = movement.GetHead();
		var ray = head.ray;

		RaycastHit2D hit = Physics2D.Raycast(
			head.transform.position,
			head.transform.rotation * Vector3.down,
			Mathf.Infinity,
			1 << LayerMask.NameToLayer("Wall"));
		if (hit.collider == null) yield break;

		Vector3 diff = hit.point - (Vector2) head.transform.position;
		var size = diff.x != 0 ? diff.x : diff.y;

		photonView.RPC("ToggleRay", PhotonTargets.All, true, size);
		yield return new WaitForSeconds(3.0f);
		photonView.RPC("ToggleRay", PhotonTargets.All, false, size);
	}

	[PunRPC]
	void ToggleRay(bool value, float size) {
		var head = movement.GetHead();
		var ray = head.ray;
		
		movement.canMove = !value;
		movement.canMoveHead = !value;
		ray.SetActive(value);

		if (value) {
			ray.GetComponentInChildren<SpriteRenderer>().size = new Vector2(size, 0.5f);
			ray.transform.localPosition = new Vector2(0, Mathf.Abs(size) * -1);
			ray.GetComponentInChildren<BoxCollider2D>().size = new Vector2(Mathf.Abs(size), 0.5f);
		}
	}
}
