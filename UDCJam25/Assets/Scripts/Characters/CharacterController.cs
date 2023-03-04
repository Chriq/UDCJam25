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
	}

    public int SetCharacterPosition(Vector2Int pos) {
		GameManager.Instance.board.GetTile((int) transform.position.x, (int) transform.position.y).isOccupied = false;
		transform.position = new Vector3(pos.x, pos.y);
		GameManager.Instance.board.GetTile(pos.x, pos.y).isOccupied = true;
		return 10;
	}

	private void OnMouseDown() {
		Debug.Log("clicked");
		if(Input.GetMouseButtonDown(0)) {
			StartCoroutine(SelectCharacter());
		}
	}

	IEnumerator SelectCharacter() {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.selectedCharacter = gameObject;
		characterSelected = true;
	}
}
