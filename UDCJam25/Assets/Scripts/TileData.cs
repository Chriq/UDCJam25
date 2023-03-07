using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObjects/TileData")]
public class TileData : ScriptableObject
{
	public TileBase[] tiles;
	public int height = 0;
}
