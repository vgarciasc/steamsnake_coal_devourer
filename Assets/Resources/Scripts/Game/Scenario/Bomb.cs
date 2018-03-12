using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour {

	public SpriteRenderer sr;
	public ParticleSystem explosion;
	public GameObject circleOfDoom;
	bool exploding = false;

	public IEnumerator PrepareAndExplode() {
		float time = 5f;
		float division = 0.5f;

		for (int i = 0; i < 20; i++) {
			if (sr == null) yield break;
			
			sr.color = Color.white;
			sr.DOColor(Color.red, time * 0.5f).OnComplete(() => {
				sr.DOColor(Color.white, time * 0.5f);
			});
			yield return new WaitForSeconds(time);
			time *= division;
		}

		StartCoroutine(Explode());
	}

	IEnumerator Explode() {
		explosion.Play();
		sr.enabled = false;

		yield return new WaitUntil(() => explosion.isPlaying);
		circleOfDoom.SetActive(true);
		yield return new WaitForSeconds(explosion.main.startLifetime.constant + explosion.main.duration);
		
		ItemSpawner.instance.SpawnItemRandomPos();
		Destroy(this.gameObject);
	}
}
