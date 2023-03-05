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
	void Update() {
		if(Input.GetMouseButtonDown(0) && inputActive) {
			if(controller.characterSelected) {
				Vector3 mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2Int toCell = new Vector2Int(Mathf.RoundToInt(mouseDown.x), Mathf.RoundToInt(mouseDown.y));

				string cellValidation = GameManager.Instance.ValidateCharacterMovement(toCell);
				if(cellValidation.Equals("ok")) {
					lastMovement = controller.SetCharacterPosition(toCell);
					controller.characterSelected = false;
				} else if(cellValidation.Equals("Player")) {
					controller.characterSelected = false;
				}
			}
		}
	}
}
