using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamePlayerManager : Photon.PunBehaviour {
	public static GamePlayerManager instance;

	[SerializeField]
	GameObject playerPrefab;

	void Start () {
		instance = this;

		PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		ExitGame();
	}

	public static void ExitGame() {
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("RoomLobby");
	}
}
