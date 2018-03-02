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
		Vector3Int[] tiles = new Vector3Int[62];
		Tile[] textures = new Tile[62];
		
		Tile tex = value ? lightTile : darkTile;
		Tile tex_2 = value ? lightTile_2 : darkTile;

		tiles[0] = new Vector3Int(pos.x - 1, pos.y - 4, 0);
		textures[0] = tex_2;
		tiles[1] = new Vector3Int(pos.x , pos.y - 4, 0);
		textures[1] = tex_2;
		tiles[2] = new Vector3Int(pos.x + 1, pos.y - 4, 0);
		textures[2] = tex_2;
		tiles[3] = new Vector3Int(pos.x - 1, pos.y + 4, 0);
		textures[3] = tex_2;
		tiles[4] = new Vector3Int(pos.x , pos.y + 4, 0);
		textures[4] = tex_2;
		tiles[5] = new Vector3Int(pos.x + 1, pos.y + 4, 0);
		textures[5] = tex_2;
		tiles[6] = new Vector3Int(pos.x - 4, pos.y - 1, 0);
		textures[6] = tex_2;
		tiles[7] = new Vector3Int(pos.x - 4, pos.y   , 0);
		textures[7] = tex_2;
		tiles[8] = new Vector3Int(pos.x - 4, pos.y + 1, 0);
		textures[8] = tex_2;
		tiles[9] = new Vector3Int(pos.x + 4, pos.y - 1, 0);
		textures[9] = tex_2;
		tiles[10] = new Vector3Int(pos.x + 4, pos.y   , 0);
		textures[10] = tex_2;
		tiles[11] = new Vector3Int(pos.x + 4, pos.y + 1, 0);
		textures[11] = tex_2;
		tiles[12] = new Vector3Int(pos.x - 3, pos.y - 2, 0);
		textures[12] = tex_2;
		tiles[13] = new Vector3Int(pos.x - 3, pos.y - 3, 0);
		textures[13] = tex_2;
		tiles[14] = new Vector3Int(pos.x - 2, pos.y - 3, 0);
		textures[14] = tex_2;
		tiles[15] = new Vector3Int(pos.x + 3, pos.y - 2, 0);
		textures[15] = tex_2;
		tiles[16] = new Vector3Int(pos.x + 3, pos.y - 3, 0);
		textures[16] = tex_2;
		tiles[17] = new Vector3Int(pos.x + 2, pos.y - 3, 0);
		textures[17] = tex_2;
		tiles[18] = new Vector3Int(pos.x - 3, pos.y + 2, 0);
		textures[18] = tex_2;
		tiles[19] = new Vector3Int(pos.x - 3, pos.y + 3, 0);
		textures[19] = tex_2;
		tiles[20] = new Vector3Int(pos.x - 2, pos.y + 3, 0);
		textures[20] = tex_2;
		tiles[21] = new Vector3Int(pos.x + 3, pos.y + 2, 0);
		textures[21] = tex_2;
		tiles[22] = new Vector3Int(pos.x + 3, pos.y + 3, 0);
		textures[22] = tex_2;
		tiles[23] = new Vector3Int(pos.x + 2, pos.y + 3, 0);
		textures[23] = tex_2;
		tiles[24] = new Vector3Int(pos.x - 3, pos.y + 1, 0);
		textures[24] = tex_2;
		tiles[25] = new Vector3Int(pos.x - 3, pos.y   , 0);
		textures[25] = tex_2;
		tiles[26] = new Vector3Int(pos.x - 3, pos.y - 1, 0);
		textures[26] = tex_2;
		tiles[27] = new Vector3Int(pos.x + 3, pos.y + 1, 0);
		textures[27] = tex_2;
		tiles[28] = new Vector3Int(pos.x + 3, pos.y   , 0);
		textures[28] = tex_2;
		tiles[29] = new Vector3Int(pos.x + 3, pos.y - 1, 0);
		textures[29] = tex_2;
		tiles[30] = new Vector3Int(pos.x + 1, pos.y - 3, 0);
		textures[30] = tex_2;
		tiles[31] = new Vector3Int(pos.x    , pos.y - 3, 0);
		textures[31] = tex_2;
		tiles[32] = new Vector3Int(pos.x - 1, pos.y - 3, 0);
		textures[32] = tex_2;
		tiles[33] = new Vector3Int(pos.x + 1, pos.y + 3, 0);
		textures[33] = tex_2;
		tiles[34] = new Vector3Int(pos.x    , pos.y + 3, 0);
		textures[34] = tex_2;
		tiles[35] = new Vector3Int(pos.x - 1, pos.y + 3, 0);
		textures[35] = tex_2;
		tiles[36] = new Vector3Int(pos.x - 2, pos.y - 2, 0);
		textures[36] = tex_2;
		tiles[37] = new Vector3Int(pos.x - 2, pos.y + 2, 0);
		textures[37] = tex_2;
		tiles[38] = new Vector3Int(pos.x + 2, pos.y - 2, 0);
		textures[38] = tex_2;
		tiles[39] = new Vector3Int(pos.x + 2, pos.y + 2, 0);
		textures[39] = tex_2;

		tiles[40] = new Vector3Int(pos.x + 1, pos.y - 2, 0);
		textures[40] = tex;
		tiles[41] = new Vector3Int(pos.x    , pos.y - 2, 0);
		textures[41] = tex;
		tiles[42] = new Vector3Int(pos.x - 1, pos.y - 2, 0);
		textures[42] = tex;
		tiles[43] = new Vector3Int(pos.x + 1, pos.y + 2, 0);
		textures[43] = tex;
		tiles[44] = new Vector3Int(pos.x    , pos.y + 2, 0);
		textures[44] = tex;
		tiles[45] = new Vector3Int(pos.x - 1, pos.y + 2, 0);
		textures[45] = tex;
		tiles[46] = new Vector3Int(pos.x - 2, pos.y + 1, 0);
		textures[46] = tex;
		tiles[47] = new Vector3Int(pos.x - 2, pos.y    , 0);
		textures[47] = tex;
		tiles[48] = new Vector3Int(pos.x - 2, pos.y - 1, 0);
		textures[48] = tex;
		tiles[49] = new Vector3Int(pos.x + 2, pos.y + 1, 0);
		textures[49] = tex;
		tiles[50] = new Vector3Int(pos.x + 2, pos.y    , 0);
		textures[50] = tex;
		tiles[51] = new Vector3Int(pos.x + 2, pos.y - 1, 0);
		textures[51] = tex;
		tiles[52] = new Vector3Int(pos.x - 1, pos.y - 1, 0);
		textures[52] = tex;
		tiles[53] = new Vector3Int(pos.x + 1, pos.y + 1, 0);
		textures[53] = tex;
		tiles[54] = new Vector3Int(pos.x - 1, pos.y + 1, 0);
		textures[54] = tex;
		tiles[55] = new Vector3Int(pos.x + 1, pos.y - 1, 0);
		textures[55] = tex;
		tiles[56] = new Vector3Int(pos.x - 1, pos.y   , 0);
		textures[56] = tex;
		tiles[57] = new Vector3Int(pos.x + 1, pos.y   , 0);
		textures[57] = tex;
		tiles[59] = new Vector3Int(pos.x, pos.y - 1, 0);
		textures[59] = tex;
		tiles[60] = new Vector3Int(pos.x, pos.y + 1, 0);
		textures[60] = tex;
		tiles[61] = new Vector3Int(pos.x, pos.y, 0);
		textures[61] = tex;

		// tiles[0] = new Vector3Int(pos.x - 1, pos.y - 1, 0);
		// tiles[1] = new Vector3Int(pos.x    , pos.y - 1, 0);
		// tiles[2] = new Vector3Int(pos.x + 1, pos.y - 1, 0);
		// tiles[3] = new Vector3Int(pos.x - 1, pos.y    , 0);
		// tiles[4] = new Vector3Int(pos.x    , pos.y    , 0);
		// tiles[5] = new Vector3Int(pos.x + 1, pos.y    , 0);
		// tiles[6] = new Vector3Int(pos.x - 1, pos.y + 1, 0);
		// tiles[7] = new Vector3Int(pos.x    , pos.y + 1, 0);
		// tiles[8] = new Vector3Int(pos.x + 1, pos.y + 1, 0);

		// tiles[09] = new Vector3Int(pos.x - 2, pos.y    , 0);
		// tiles[10] = new Vector3Int(pos.x + 2, pos.y    , 0);
		// tiles[11] = new Vector3Int(pos.x    , pos.y - 2, 0);
		// tiles[12] = new Vector3Int(pos.x    , pos.y + 2, 0);

		// for (int i = 0; i < 9; i++) textures[i] = value ? lightTile : darkTile;
		// for (int i = 9; i < 13; i++) textures[i] = value ? lightTile_2 : darkTile;

		lightingTilemap.SetTiles(tiles, textures);
	}

	Vector3Int WorldPositionToTilePosition(Vector3 worldPosition) {
		return new Vector3Int(
			(int) ((worldPosition.x - (x_offset)) * 8),
			(int) ((worldPosition.y - (y_offset)) * 8),
			0
		);
	}
}
