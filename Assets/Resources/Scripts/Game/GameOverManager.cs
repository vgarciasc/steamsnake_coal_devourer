using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameOverManager : Photon.PunBehaviour {

	[SerializeField]
	GameObject banner;

	public static GameOverManager instance;

	void Start () {
		instance = this;
		// if (PhotonNetwork.isMasterClient) {
		// 	StartCoroutine(EndGame());
		// }
	}

	public IEnumerator EndGame(bool link_win = true) {
		photonView.RPC("GameOverBanner", PhotonTargets.All, link_win);
		yield return new WaitForSeconds(3.0f);
		GamePlayerManager.ExitGame();
	}	

	[PunRPC]
	void GameOverBanner(bool link_win) {
		banner.SetActive(true);
		banner.GetComponentInChildren<TextMeshProUGUI>().text = link_win ? "link vence" : "boss vence";
	}
}
