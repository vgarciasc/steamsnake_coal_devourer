using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerLobbyManager : Photon.PunBehaviour {
	[Header("References")]
	[SerializeField]
	GameObject swapButton;
	[SerializeField]
	GameObject startGameButton;
	[SerializeField]
	GameObject leaveRoomButton;
	[SerializeField]
	TextMeshProUGUI player_1_name;
	[SerializeField]
	TextMeshProUGUI player_2_name;

	List<string> playerNames = new List<string>();

	void Start() {
		if (!PhotonNetwork.inRoom) {
			swapButton.SetActive(false);
			startGameButton.SetActive(false);
			leaveRoomButton.SetActive(false);
			return;
		}

		PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"reversed", false}});
		if (PhotonNetwork.room.PlayerCount == 1) {
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"first_player_ID", PhotonNetwork.player.ID}});
		}

		PhotonNetwork.automaticallySyncScene = true;
		startGameButton.SetActive(PhotonNetwork.isMasterClient);

		UpdateNames();
	}

	void UpdateNames() {
		playerNames = new List<string>();
		print("playerList[0]: " + PhotonNetwork.playerList[0].NickName);
		playerNames.Add(PhotonNetwork.playerList[0].NickName);

		if (PhotonNetwork.room.PlayerCount > 1) {
			playerNames.Add(PhotonNetwork.playerList[1].NickName);
		}

		ShowNames();
	}

	public void ShowNames() {
		string player_1 = "---";
		string player_2 = "---";

		if (playerNames.Count == 1) {
			player_1 = playerNames[0];
		} else {
			bool reversed = GetReversed();
			player_1 = reversed ? playerNames[1] : playerNames[0];
			player_2 = reversed ? playerNames[0] : playerNames[1];
		}

		player_1_name.text = player_1;
		player_2_name.text = player_2;
	}

	public void SwapPlayerRoles() {
		bool reversed = GetReversed();
		PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"reversed", !reversed}});
		ShowNames();
	}

	public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
		ShowNames();
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		playerNames.Add(newPlayer.NickName);
		ShowNames();
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		UpdateNames();
	}

	bool GetReversed() {
		var aux = PhotonNetwork.room.CustomProperties["reversed"];
		
		if (aux == null) {
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {{"reversed", false}});
			return false;
		} else {
			return (bool) aux;
		}
	}

	public void LeaveRoom() {
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom() {
		SceneManager.LoadScene("RoomLobby");
	}

	public void StartGame() {
		PhotonNetwork.LoadLevel("Game");
	}
}
