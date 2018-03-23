using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinkFootstep : MonoBehaviour {

	public float lifetime = 5f;

	void Start () {
		Evaporate();
	}

	void Evaporate() {
		GetComponentInChildren<SpriteRenderer>().DOFade(0f, lifetime).OnComplete(() => {
			Destroy(this.gameObject);
		});
	}
}
