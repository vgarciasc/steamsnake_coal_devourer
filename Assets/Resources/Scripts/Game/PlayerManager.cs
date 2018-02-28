using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.PunBehaviour {

	public bool isBoss;
	public bool isLink;

	[SerializeField]
	GameObject playerPrefab;
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
				GameObject go = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
			} else if (isBoss) {
				GameObject go = PhotonNetwork.Instantiate(steamsnakePrefab.name, Vector3.zero, Quaternion.identity, 0);
			}
		}
	}
}
