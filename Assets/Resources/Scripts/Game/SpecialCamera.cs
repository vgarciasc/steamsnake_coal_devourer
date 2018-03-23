using UnityEngine;
using System.Collections;

public class SpecialCamera : MonoBehaviour {
    float originalSize;
    Vector3 originalPos;
    Camera cam;

	public Transform linkPlayer;
	Vector2 positionToFollow;

    void Start() {
        cam = Camera.main;
        originalPos = cam.transform.localPosition;
        originalSize = cam.orthographicSize;
    }

	void FixedUpdate() {
		if (linkPlayer != null) {
			positionToFollow = linkPlayer.position;
			this.transform.position = positionToFollow;
		}
	}

    #region Screen Shake
    public void screenShake_(float power) { StartCoroutine(screenShake(power)); }
    IEnumerator screenShake(float power) {
        var max = 0.050f;
        var min = 0.005f;

        power = (max - min) * power + min;

        for (int i = 0; i < 10; i++) {
            yield return new WaitForEndOfFrame();
            // float x = getScreenShakeDistance(power);
            // float y = getScreenShakeDistance(power);

            float x = Random.Range(-power, power);
            float y = Random.Range(-power, power);

            cam.transform.localPosition = new Vector3(originalPos.x + x,
                                                       originalPos.y + y,
                                                       originalPos.z);
        }

        cam.transform.localPosition = originalPos;
    }

    float getScreenShakeDistance(float power) {
        float power_aux = power;
        int count = 0;
        while (true) {
            count++;
            float aux = Mathf.Pow(-1, Random.Range(0, 2)) * Random.Range(power_aux / 4, power_aux / 2);
            if (Mathf.Abs(aux) > 0.1f) {
                return aux;
            }
            if (count > 5) {
                count = 0;
                power_aux += 0.25f;
            }
        }
    }
    #endregion
}