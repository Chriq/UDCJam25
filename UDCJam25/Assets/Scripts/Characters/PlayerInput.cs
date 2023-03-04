using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    CharacterController controller;
	public bool inputActive = true;
	public int lastMovement = 0;

	private void Awake() {
		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
    {
		if(Input.GetMouseButtonDown(0) && inputActive) {
			if(!controller.characterSelected) {

			} else {
				Vector3 mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2Int toCell = new Vector2Int(Mathf.RoundToInt(mouseDown.x), Mathf.RoundToInt(mouseDown.y));

				lastMovement = controller.SetCharacterPosition(toCell);
				controller.characterSelected = false;
			}
		}
	}
}
