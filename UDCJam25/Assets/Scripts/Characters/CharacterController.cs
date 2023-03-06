using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CharacterController : MonoBehaviour
{
	public Vector2Int startingPosition;
	public float characterSpeed = 5f;
	public bool characterSelected = false;

	private Vector2Int newPos;
	float diffX;
	float diffY;

	// Start is called before the first frame update
	void Start()
    {
		if(startingPosition != Vector2Int.zero) {
			transform.position = new Vector3(startingPosition.x, startingPosition.y);
		} else {
			startingPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
		}
		
		newPos = startingPosition;

		GameManager.Instance.board.GetTile(startingPosition.x, startingPosition.y).isOccupied = true;
		GameManager.Instance.board.GetTile(startingPosition.x, startingPosition.y).occupant = gameObject;
	}

    public int SetCharacterPosition(Vector2Int pos) {
		GameManager.Instance.board.GetTile((int) transform.position.x, (int) transform.position.y).isOccupied = false;
		//transform.position = new Vector3(pos.x, pos.y);
		newPos = pos;
		diffX = newPos.x - transform.position.x;
		diffY = newPos.y - transform.position.y;
		GameManager.Instance.board.GetTile(pos.x, pos.y).isOccupied = true;
		GameManager.Instance.board.GetTile(pos.x, pos.y).occupant = gameObject;
		return 4;
	}

	private void Update() {
		if(transform.position != new Vector3(newPos.x, newPos.y)) {
			if(transform.position.x != newPos.x) {
				if(Mathf.Abs(transform.position.x - newPos.x) < 0.1) {
					transform.position = new Vector3(newPos.x, transform.position.y);
				} else {
					float increment = diffX * characterSpeed * Time.deltaTime;
					transform.position += new Vector3(increment, 0);
				}
			} 
			
			else if(transform.position.y != newPos.y) {
				if(Mathf.Abs(transform.position.y - newPos.y) < 0.1) {
					transform.position = new Vector3(transform.position.x, newPos.y);
				} else {
					float increment = diffY * characterSpeed * Time.deltaTime;
					transform.position += new Vector3(0, increment);
				}
			}
		}
	}
}
