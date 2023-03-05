using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CharacterController : MonoBehaviour
{
	public Vector2Int startingPosition;
	public bool characterSelected = false;

	// Start is called before the first frame update
	void Start()
    {
		transform.position = new Vector3(startingPosition.x, startingPosition.y);
		GameManager.Instance.board.GetTile(startingPosition.x, startingPosition.y).isOccupied = true;
		GameManager.Instance.board.GetTile(startingPosition.x, startingPosition.y).occupant = gameObject;
	}

    public int SetCharacterPosition(Vector2Int pos) {
		GameManager.Instance.board.GetTile((int) transform.position.x, (int) transform.position.y).isOccupied = false;
		transform.position = new Vector3(pos.x, pos.y);
		GameManager.Instance.board.GetTile(pos.x, pos.y).isOccupied = true;
		GameManager.Instance.board.GetTile(pos.x, pos.y).occupant = gameObject;
		return 4;
	}

	private void OnMouseDown() {
		Debug.Log("clicked");
		if(Input.GetMouseButtonDown(0)) {
			StartCoroutine(SelectCharacter());
		}
	}

	IEnumerator SelectCharacter() {
		yield return new WaitForEndOfFrame();
		Debug.Log(gameObject.name);
		GameManager.Instance.selectedCharacter = gameObject;
		characterSelected = true;
	}
}
