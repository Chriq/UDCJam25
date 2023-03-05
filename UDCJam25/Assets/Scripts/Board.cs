using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	// x = rows; y = cols;
	public Vector2Int size;

	public Tile[,] tiles;
	//public SpriteRenderer spriteRenderer;

	private Dictionary<string, Sprite> sprites;

	private void Awake() {
		tiles = new Tile[size.x, size.y];

		GenerateBoard();
	}

	private void GenerateBoard() {
		for(int row = 0; row < size.y; row++) {
			for(int col = 0; col < size.x; col++) {
				Tile tile = new GameObject().AddComponent<Tile>();
				tile.transform.parent = transform;
				tile.isOccupied = false;
				tile.transform.position = new Vector2(col, row);

				tiles[col, row] = tile;
			}
		}
	}

	public Tile GetTile(int x, int y) {
		if(x < size.x && x < size.y && x >= 0 && y >= 0) {
			return tiles[x, y];
		}

		return null;
	}
}

public class Tile : MonoBehaviour {
	public bool isOccupied = false;
	public GameObject occupant;
}