using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateIfNotMine : Photon.PunBehaviour {

	void Start () {
		Deactivate();
	}

	public void Deactivate() {
		PhotonView _photonView = null;
		Transform aux = this.transform;

		for (int i = 0; i < 10 && _photonView == null; i++) {
			_photonView = aux.GetComponent<PhotonView>();
			aux = aux.transform.parent;
		}

		if (PhotonNetwork.inRoom && !_photonView.isMine) {
			this.gameObject.SetActive(false);
		}
	}
}
