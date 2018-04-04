using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SteamsnakeManager : Photon.PunBehaviour {

	[Header("References")]
	public List<ParticleSystem> massExplosion;

	public SteamsnakeMovement movement;
	public bool isShootingRay;

	SpecialCamera specialCamera;

	void Start() {
		movement = this.GetComponent<SteamsnakeMovement>();
		specialCamera = Camera.main.GetComponentInParent<SpecialCamera>();
	}

	void Update () {
		if (photonView.isMine) {
			HandlePowers();
			HandleSnakeShake();
		}		
	}

	void HandlePowers() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			photonView.RPC("ShootRay", PhotonTargets.All);
		}
	}

	[PunRPC]
	void ShootRay() {
		StartCoroutine(ShootRayCoroutine());
	}

	IEnumerator ShootRayCoroutine() {
		var head = movement.GetHead();
		var ray = head.ray;

		RaycastHit2D hit = Physics2D.Raycast(
			head.transform.position,
			head.transform.rotation * Vector3.down,
			Mathf.Infinity,
			(1 << LayerMask.NameToLayer("Wall")) | (1 << LayerMask.NameToLayer("Object")));
		if (hit.collider == null) yield break;

		Vector3 diff = hit.point - (Vector2) head.transform.position;
		var size = diff.x != 0 ? diff.x : diff.y;

		movement.canMove = false;
		yield return new WaitForSeconds(0.5f);
		ToggleRay(true, size);
		yield return new WaitForSeconds(3.0f);
		ToggleRay(false, size);
	}

	void ToggleRay(bool value, float size) {
		var head = movement.GetHead();
		var ray = head.ray;
		
		isShootingRay = value;
		movement.canMove = !value;
		movement.canMoveHead = !value;
		ray.SetActive(value);

		if (value) {
			ray.GetComponentInChildren<SpriteRenderer>().size = new Vector2(size, 0.5f);
			ray.transform.localPosition = new Vector2(0, Mathf.Abs(size) * -1);
			ray.GetComponentInChildren<BoxCollider2D>().size = new Vector2(Mathf.Abs(size), 0.5f);
		}
	}

	void HandleSnakeShake() {
		if (isShootingRay) {
			float power = 0.8f;
			specialCamera.screenShake_(power);
		}
	}

	public void Die() {
		StartCoroutine(GameOverManager.instance.EndGame(true));
		photonView.RPC("DieAnimation", PhotonTargets.All);
	}

	[PunRPC]
	void DieAnimation() {
		foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>()) {
			sr.DOColor(Color.red, 2f).OnComplete(() => {
				sr.DOColor(Color.black, 0.5f);
				movement.MassExplode();
			});
		}
	}
}
