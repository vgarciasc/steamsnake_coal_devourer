using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GamePlayerManager : Photon.PunBehaviour {
	public static GamePlayerManager instance;

	[SerializeField]
	GameObject playerManagerPrefab;

	public Transform linkSpawn;	
	public Transform snakeSpawn;

	void Start () {
		if (!PhotonNetwork.inRoom) return;
		instance = this;

		PhotonNetwork.Instantiate(playerManagerPrefab.name, Vector3.zero, Quaternion.identity, 0);
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		ExitGame();
	}

	public static void ExitGame() {
		if (!PhotonNetwork.inRoom) return;

		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("RoomLobby");
	}
}
