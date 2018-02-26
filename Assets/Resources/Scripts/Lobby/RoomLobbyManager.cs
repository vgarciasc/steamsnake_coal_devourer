using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomLobbyManager : Photon.PunBehaviour {

	public GameObject connectButton;

	void Start () {
		connectButton.SetActive(false);
		PhotonNetwork.ConnectUsingSettings("v0.0");
	}

	public override void OnConnectedToMaster() {
		connectButton.SetActive(true);
	}

	public void ConnectToGame() {
		connectButton.SetActive(false);

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("sala_1", roomOptions, TypedLobby.Default);
	}

	public void SetPlayerName(string playerName) {
		PhotonNetwork.playerName = playerName + " ";
	}

	public override void OnJoinedRoom() {
		SceneManager.LoadScene("PlayerLobby");
	}
}
