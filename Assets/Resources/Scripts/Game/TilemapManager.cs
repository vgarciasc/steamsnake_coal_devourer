using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TilemapManager : MonoBehaviour {

	public Tilemap lightingTilemap;
	public Tile darkTile;
	public Tile lightTile;
	public Tile lightTile_2;
	
	public float x_offset;
	public float y_offset;

	Vector3Int lastPlayerPos;

	void Update() {
		var player = GameObject.FindGameObjectWithTag("Link");
		if (player != null) {
			ChangePlayerPos(player.transform.position);
		}
	}

	void ChangePlayerPos(Vector3 playerPos) {
		Vector3Int newPlayerPos = WorldPositionToTilePosition(playerPos);

		ToggleLight(lastPlayerPos, false);
		ToggleLight(newPlayerPos, true);

		lastPlayerPos = newPlayerPos;
	}

	void ToggleLight(Vector3Int pos, bool value) {
		Vector3Int[] tiles = new Vector3Int[13];
		tiles[0] = new Vector3Int(pos.x - 1, pos.y - 1, 0);
		tiles[1] = new Vector3Int(pos.x    , pos.y - 1, 0);
		tiles[2] = new Vector3Int(pos.x + 1, pos.y - 1, 0);
		tiles[3] = new Vector3Int(pos.x - 1, pos.y    , 0);
		tiles[4] = new Vector3Int(pos.x    , pos.y    , 0);
		tiles[5] = new Vector3Int(pos.x + 1, pos.y    , 0);
		tiles[6] = new Vector3Int(pos.x - 1, pos.y + 1, 0);
		tiles[7] = new Vector3Int(pos.x    , pos.y + 1, 0);
		tiles[8] = new Vector3Int(pos.x + 1, pos.y + 1, 0);

		tiles[09] = new Vector3Int(pos.x - 2, pos.y    , 0);
		tiles[10] = new Vector3Int(pos.x + 2, pos.y    , 0);
		tiles[11] = new Vector3Int(pos.x    , pos.y - 2, 0);
		tiles[12] = new Vector3Int(pos.x    , pos.y + 2, 0);

		Tile[] textures = new Tile[13];
		for (int i = 0; i < 9; i++) textures[i] = value ? lightTile : darkTile;
		for (int i = 9; i < 13; i++) textures[i] = value ? lightTile_2 : darkTile;

		lightingTilemap.SetTiles(tiles, textures);
	}

	Vector3Int WorldPositionToTilePosition(Vector3 worldPosition) {
		return new Vector3Int(
			(int) ((worldPosition.x - (x_offset)) * 2),
			(int) ((worldPosition.y - (y_offset)) * 2),
			0
		);
	}
}
