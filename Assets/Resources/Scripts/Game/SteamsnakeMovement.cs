using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SteamsnakeMovement : Photon.PunBehaviour, IPunObservable {

	[Header("References")]
	[SerializeField]
	GameObject blobPrefab;
	[SerializeField]
	GameObject headBlobPrefab;
	[SerializeField]
	Transform blobContainer;

	List<Vector2> blobPositions = new List<Vector2>();
	Vector2[] blobPositionsArray;
	List<GameObject> blobs = new List<GameObject>();

	Direction currentDirection = Direction.LEFT;

	[Header("Mechanics")]
	[SerializeField]
	float viewRadius = 4f;

	SpriteRenderer linkSprite;
	float speed = 0.6f;

	void Start () {
		this.transform.position = GamePlayerManager.instance.snakeSpawn.position;
		InitializeBlobs();

		if (!PhotonNetwork.inRoom) return;
		if ((bool) PhotonNetwork.player.CustomProperties["is_link"]) return;

		StartCoroutine(Move());
	} 
	
	void Update () {
		if (!PhotonNetwork.inRoom) return;

		UpdateBody();

		if ((bool) PhotonNetwork.player.CustomProperties["is_link"]) return;

		HandleDirection();
		HandleVisibility();
	}

	void InitializeBlobs() {
		HushPuppy.destroyChildren(blobContainer.gameObject);

		blobs = new List<GameObject>();
		blobPositions = new List<Vector2>();
		int size = 5;

		for (int i = 0; i < size; i++) {
			var go = Instantiate(i == size - 1 ? headBlobPrefab : blobPrefab,
				this.transform.position + (Vector3.up * 0.5f * i),
				Quaternion.identity);
			go.transform.SetParent(blobContainer);
			blobs.Add(go);

			blobPositions.Add(go.transform.position);
		}

		UpdateArrayFromList();
	}

	void UpdateListFromArray() {
		blobPositions = new List<Vector2>();
		for (int i = 0; i < blobPositionsArray.Length; i++) {
			blobPositions.Add(blobPositionsArray[i]);
		}
	}

	void UpdateArrayFromList() {
		blobPositionsArray = new Vector2[blobPositions.Count];
		for (int i = 0; i < blobPositions.Count; i++) {
			blobPositionsArray[i] = blobPositions[i];
		}
	}

	IEnumerator Move() {
		while (true) {
			yield return new WaitForSeconds(speed);

			photonView.RPC("Advance", PhotonTargets.All, currentDirection);
		}
	}

	[PunRPC]
	void Advance(Direction direction) {
		var head = blobPositions.ElementAt(blobPositions.Count - 1);
		
		Vector2 offset = Vector2.zero;
		switch (direction) {
			case Direction.TOP:
				offset = new Vector3(0, 1, 0);
				break;
			case Direction.RIGHT:
				offset = new Vector3(1, 0, 0);
				break;
			case Direction.LEFT:
				offset = new Vector3(-1, 0, 0);
				break;
			case Direction.BOTTOM:
				offset = new Vector3(0, -1, 0);
				break;
		}

		RaycastHit2D hit = Physics2D.Raycast(
			blobs.Last().transform.position,
			offset, 
			0.5f, 
			LayerMask.NameToLayer("Walls") | LayerMask.NameToLayer("Snake"));
		if (hit.collider != null) {
			return;
		}

		Vector3 newHead = head + offset * 0.5f;
		blobPositions.RemoveAt(0);
		blobPositions.Add(newHead);

		UpdateBody();
	}

	void UpdateBody() {
		for (int i = 0; i < blobPositions.Count; i++) {
			if (blobPositions.ElementAt(i) != (Vector2) blobs[i].transform.position) {
				blobs[i].transform.DOMove(
					blobPositions.ElementAt(i),
					speed);
			}
		}
	}

	void HandleDirection() {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		var dir = Direction.NONE;

		if (horizontal < 0f) dir = Direction.LEFT;
		else if (horizontal > 0f) dir = Direction.RIGHT;
		else if (vertical > 0f) dir = Direction.TOP;
		else if (vertical < 0f) dir = Direction.BOTTOM;

		if (dir != Direction.NONE && !OppositeDirections(currentDirection, dir)) {
			currentDirection = dir;
		}
	}

	public static bool OppositeDirections(Direction dir_1, Direction dir_2) {
		return (dir_1 == Direction.LEFT && dir_2 == Direction.RIGHT ||
			dir_2 == Direction.LEFT && dir_1 == Direction.RIGHT ||
			dir_1 == Direction.TOP && dir_2 == Direction.BOTTOM ||
			dir_2 == Direction.TOP && dir_1 == Direction.BOTTOM);
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
		    stream.SendNext(currentDirection);
		}
		else {
			currentDirection = (Direction) stream.ReceiveNext();
		}
    }

	void HandleVisibility() {
		var invisibles = FindObjectsOfType<InvisibleToBoss>();
		foreach (var inv in invisibles) {
			inv.GetComponent<SpriteRenderer>().enabled = (
				(blobs.Last().transform.position - inv.transform.position).magnitude < viewRadius
			);
		}
	}
}
