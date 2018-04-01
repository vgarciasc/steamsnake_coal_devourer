using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepGenerator : Photon.PunBehaviour {
	
	Vector3 lastFootstepPlace = Vector3.zero;

	[Header("References")]
	[SerializeField]
	GameObject footstepsOrigin;
	[SerializeField]
	GameObject footstepsPrefab;

	public bool shouldRotate = false;

	void Update () {
		if (photonView.isMine) {
			HandleFootsteps();
		}
	}

	void HandleFootsteps() {
		if (Vector3.Distance(lastFootstepPlace, this.transform.position) > 0.5f) {
			photonView.RPC("PlaceFootstep",
				PhotonTargets.All,
				footstepsOrigin.transform.position,
				Vector3.Angle(lastFootstepPlace, this.transform.position) * Mathf.Rad2Deg + 90);
			lastFootstepPlace = this.transform.position;
		}
	}
	
	[PunRPC]
	void PlaceFootstep(Vector3 position, float rotation) {
		var obj = Instantiate(footstepsPrefab, position, Quaternion.identity);
		if (shouldRotate) obj.transform.Rotate(new Vector3(0, 0, rotation));
	}
}
