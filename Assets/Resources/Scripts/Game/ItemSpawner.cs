using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Photon.PunBehaviour {

	[Header("References")]
	[SerializeField]
	GameObject bombPrefab;

	[Header("Mechanics")]
	public float minX = 0f;
	public float maxX = 0f;
	public float minY = 0f;
	public float maxY = 0f;

	public static ItemSpawner instance;

	void Start() {
		instance = this;

		for (int i = 0; i < 2; i++) {
			SpawnItemRandomPos();
		}
	}

	public void SpawnItemRandomPos() {
		if (!PhotonNetwork.isMasterClient) return;

		Vector3 randomPos = new Vector2(
			Random.Range(minX, maxX),
			Random.Range(minY, maxY));
		RaycastHit2D hit = Physics2D.Raycast(
			randomPos,
			Vector2.up,
			1f,
			1 << (LayerMask.NameToLayer("Wall")));
		
		if (hit.collider != null) {
			SpawnItemRandomPos();
			return;
		}

		photonView.RPC("SpawnItem", PhotonTargets.All, randomPos);
	}

	[PunRPC]
	void SpawnItem(Vector3 position) {
		var obj = Instantiate(bombPrefab, position, Quaternion.identity);
		obj.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
	}
}
