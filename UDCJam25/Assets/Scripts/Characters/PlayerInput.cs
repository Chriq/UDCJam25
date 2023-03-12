using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
		if(Input.GetMouseButtonDown(0) && inputActive && controller.characterSelected && !EventSystem.current.IsPointerOverGameObject()) {
			Vector3 mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2Int toCell = new Vector2Int(Mathf.RoundToInt(mouseDown.x), Mathf.RoundToInt(mouseDown.y));

			string cellValidation = GameManager.Instance.ValidateCharacterMovement(toCell);
			if(cellValidation.Equals("ok")) {
				if(GameManager.Instance.gameState == GameState.PLAYER_TURN) {
					controller.maxPathLength = GameManager.Instance.battleController.currentPlayerPoints;
				}

				if(GameManager.Instance.board.GetTile(toCell.x, toCell.y).tileData.height == GameManager.Instance.board.GetTile(controller.currentPosition.x, controller.currentPosition.y).tileData.height) {
					lastMovement = controller.SetCharacterPosition(toCell);
					controller.characterSelected = false;
				}

				controller.maxPathLength = 0;
			} else if(cellValidation.Equals("Player") || cellValidation.Equals("Enemy")) {
				GameObject target = GameManager.Instance.GetTileOccupant(toCell);
				CombatEntity targetCombat = null;
				if(target) {
					targetCombat = target.GetComponent<CombatEntity>();
				}

				if(targetCombat) {
					lastMovement = GameManager.Instance.UseSelectedItem(combat, targetCombat);
					Debug.Log(targetCombat.health);
					controller.characterSelected = false;
				}
			}
			
			GameManager.Instance.UpdateBattlePoints(lastMovement);
			lastMovement = 0;
		}
	}

	public void ProcessMovementInput() {
		StartCoroutine(SelectCharacter());
	}

	IEnumerator SelectCharacter() {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.SetSelectedCharacter(gameObject);
		controller.characterSelected = true;
	}
}
