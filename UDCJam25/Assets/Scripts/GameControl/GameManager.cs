using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Board board;
    public GameObject selectedCharacter;
    public GameState gameState;

    private static GameManager _instance;
    public static GameManager Instance { 
        get { 
            if(_instance == null) {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

	private void Awake() {
        board = null;
        GameObject boardObj = GameObject.Find("Board");
        if(boardObj != null ) {
            board = boardObj.GetComponent<Board>();
		}
        if(board == null) {
            board = new GameObject("Board").AddComponent<Board>();
        }

		selectedCharacter = GameObject.FindGameObjectWithTag("Player");

        gameState = GameState.RPG;
	}

    public void SwitchGameState(GameState gameState) {
        this.gameState = gameState;
    }

    // returns 'ok' if cell is free, otherwise return object tag
    public string ValidateCharacterMovement(Vector2Int destination) {
        Tile tile = board.GetTile(destination.x, destination.y);
        if(tile) {
            if(tile.isOccupied) {
                return tile.occupant.tag.Equals("Untagged") ? "occupied" : tile.occupant.tag;
            } else {
                return "ok";
            }
        } else {
            return "occupied";
        }
    }

    public GameObject GetTileOccupant(Vector2Int tile) {
        Tile boardTile = board.GetTile(tile.x, tile.y);
        if(boardTile && boardTile.isOccupied && boardTile.occupant) {
            return boardTile.occupant;
		}

        return null;
    }

    public void ChangeCharacterElevation(int int_effects_value, Vector3 int_location) {
        Vector2Int gridLocation = new Vector2Int((int) int_location.x, (int) int_location.y);
        CharacterController characterController;
        switch(int_effects_value) {
            case 7: // left
                if(selectedCharacter && selectedCharacter.TryGetComponent<CharacterController>(out characterController)) {
					characterController.characterSelected = false;
                    if(board.GetTile((int)characterController.transform.position.x, (int)characterController.transform.position.y).tileData.height == 0) {
                        characterController.SetCharacterPosition(gridLocation + new Vector2Int(1, 0));
                    } else {
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(1, 0));
					}
					
                }
                break;
            case 8: // right
				if(selectedCharacter && selectedCharacter.TryGetComponent<CharacterController>(out characterController)) {
                    characterController.characterSelected = false;
					if(board.GetTile((int)characterController.transform.position.x, (int)characterController.transform.position.y).tileData.height == 0) {
						characterController.SetCharacterPosition(gridLocation + new Vector2Int(-1, 0));
					} else {
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(-1, 0));
					}
					
				}
				break;
            case 9: // bottom
				if(selectedCharacter && selectedCharacter.TryGetComponent<CharacterController>(out characterController)) {
					characterController.characterSelected = false;
					if(board.GetTile((int)characterController.transform.position.x, (int)characterController.transform.position.y).tileData.height == 0) {
						characterController.SetCharacterPosition(gridLocation + new Vector2Int(0, 1));
					} else {
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(0, 1));
					}
				}
				break;
            default: break;
        }
    }
}

public enum GameState {
    RPG,
    PLAYER_TURN,
    ENEMY_TURN
}