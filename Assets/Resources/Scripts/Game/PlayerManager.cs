using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.PunBehaviour {

	public bool isBoss;
	public bool isLink;

	[SerializeField]
	GameObject linkPrefab;
	[SerializeField]
	GameObject steamsnakePrefab;

	void Start () {
		int first_player_ID = (int) PhotonNetwork.room.CustomProperties["first_player_ID"];
		bool reversed = (bool) PhotonNetwork.room.CustomProperties["reversed"];
		isBoss = (!reversed ^ photonView.ownerId == first_player_ID);
		isLink = !isBoss;

		photonView.owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"is_boss", isBoss}});
		photonView.owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"is_link", isLink}});

		if (photonView.isMine) {
			if (isLink) {
				GameObject go = PhotonNetwork.Instantiate(linkPrefab.name, Vector3.zero, Quaternion.identity, 0);
				Camera.main.orthographicSize = 1.5f;
				Camera.main.gameObject.transform.SetParent(go.transform);
				Camera.main.transform.localPosition = new Vector3(0, 0, -10);
			} else if (isBoss) {
				GameObject go = PhotonNetwork.Instantiate(steamsnakePrefab.name, Vector3.zero, Quaternion.identity, 0);
				// Camera.main.orthographicSize = 5.05f;
			}
		}
	}

	void Update() {
		HandleLeaveRoom();
	}

	void HandleLeaveRoom() {
		if (Input.GetKeyDown(KeyCode.L)) {
			GamePlayerManager.ExitGame();
		}
	}
}
