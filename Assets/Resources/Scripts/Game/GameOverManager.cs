using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverManager : Photon.PunBehaviour {

	[SerializeField]
	GameObject banner;

	void Start () {
		// if (PhotonNetwork.isMasterClient) {
		// 	StartCoroutine(EndGame());
		// }
	}

	IEnumerator EndGame() {
		yield return new WaitForSeconds(3.0f);
		photonView.RPC("GameOverBanner", PhotonTargets.All);
		yield return new WaitForSeconds(3.0f);
		GamePlayerManager.ExitGame();
	}	

	[PunRPC]
	void GameOverBanner() {
		banner.SetActive(true);
	}
}
