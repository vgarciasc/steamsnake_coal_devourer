using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Photon.PunBehaviour {

	[Header("References")]
	[SerializeField]
	GameObject bombPrefab;
	[SerializeField]
	Transform bombSpawnList;

	public static ItemSpawner instance;

	void Start() {
		instance = this;

		for (int i = 0; i < 2; i++) {
			SpawnItemRandomPos();
		}
	}

	public void SpawnItemRandomPos() {
		if (!PhotonNetwork.isMasterClient) return;

		int dice = Random.Range(0, bombSpawnList.childCount);
		Vector3 pos = bombSpawnList.GetChild(dice).position;

		RaycastHit2D hit = Physics2D.Raycast(
			pos,
			Vector3.up,
			0.5f,
			(1 << LayerMask.NameToLayer("Wall")) | (1 << LayerMask.NameToLayer("Object")));

		if (hit.collider != null) {
			SpawnItemRandomPos();
			return;
		}

		SpawnItem(pos);
	}

	void SpawnItem(Vector3 position) {
		var obj = PhotonNetwork.Instantiate("Bomb Prefab", position, Quaternion.identity, 0);
		obj.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
	}
}
