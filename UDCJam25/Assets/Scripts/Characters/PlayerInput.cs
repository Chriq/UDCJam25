using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    CharacterController controller;
	CombatEntity combat;
	public bool inputActive = true;
	public int lastMovement = 0;

	private void Awake() {
		controller = GetComponent<CharacterController>();
		combat = GetComponent<CombatEntity>();
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
				} else if(cellValidation.Equals("Enemy")) {
					GameObject target = GameManager.Instance.GetTileOccupant(toCell);
					CombatEntity targetCombat = null;
					if(target) {
						targetCombat = target.GetComponent<CombatEntity>();
					}

					if(targetCombat) {
						lastMovement = combat.UseItem(0, targetCombat);
						Debug.Log(targetCombat.health);
						controller.characterSelected = false;
					}
				}
			}
		}
	}

	public Vector2Int ProcessMovementInput() {
		Vector3 mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector2Int(Mathf.RoundToInt(mouseDown.x), Mathf.RoundToInt(mouseDown.y));
	}

	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			StartCoroutine(SelectCharacter());
		}
	}

	IEnumerator SelectCharacter() {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.selectedCharacter = gameObject;
		controller.characterSelected = true;
	}
}
