using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinkUI : Photon.PunBehaviour {

	LinkUICompass compass;
	SteamsnakeHead steamsnakeHead;

	void Start() {
		bool isLink = (bool) photonView.owner.CustomProperties["is_link"];
		bool isBoss = (bool) photonView.owner.CustomProperties["is_boss"];

		compass = GameObject.FindGameObjectWithTag ("LinkCompass").GetComponentInChildren<LinkUICompass>();

		if (isBoss) {
			compass.gameObject.SetActive (false);
			this.enabled = false;
		}
	}

	void Update() {
		if (steamsnakeHead == null) {
			var obj = GameObject.FindGameObjectWithTag ("Snake");
			if (obj != null) steamsnakeHead = obj.GetComponentInChildren<SteamsnakeHead> ();
		} else {
			HandleCompass();
		}
	}

	void HandleCompass() {
		Vector3 diff = (steamsnakeHead.transform.position - this.transform.position).normalized;
		float angle = Vector2.Angle (Vector2.up, diff);
		print (angle);
		angle *= (diff.x < 0 ? 1 : -1);
		print (angle);

		compass.AngleTo( angle );
	}
}
