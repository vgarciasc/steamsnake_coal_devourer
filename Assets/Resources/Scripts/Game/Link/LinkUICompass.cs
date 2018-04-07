using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LinkUICompass : MonoBehaviour {
	public Image indicator;
	public List<Image> circles;

	void Start() {
		StartCoroutine (_Start ());
	}

	IEnumerator _Start() {
		yield return new WaitUntil(() => PhotonNetwork.player.CustomProperties["is_link"] != null);
		if ((bool) PhotonNetwork.player.CustomProperties ["is_boss"]) {
			this.gameObject.SetActive (false);
			yield break;
		}

		StartCoroutine (StartCircles ());
	}

	IEnumerator StartCircles() {
		for (int i = 0; i < circles.Count; i++) {
			StartMovingCircle (circles[i]);
			yield return new WaitForSeconds (1f);
		}
	}

	void StartMovingCircle(Image circle) {
		circle.color = new Color (1f, 1f, 1f, 0.5f);
		circle.transform.localScale = Vector3.zero;
		circle.transform.DOScale (Vector3.one, 2f);
		circle.DOFade (0f, 2f).OnComplete(() => {
			StartMovingCircle(circle);
		});
	}

	public void AngleTo(float angle) {
		indicator.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
	}

	public void ToggleIndicator(bool value) {
		indicator.gameObject.SetActive(value);
	}
}
