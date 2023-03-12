using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public enum GameState
{
    RPG,
    PLAYER_TURN,
    ENEMY_TURN
}

public class GameManager : MonoBehaviour {
    public Board board;
    public GameObject selectedCharacter;
    public GameState gameState;
    public Battle battleController;
    [SerializeField] public Item selectedItem;
    private AstarPath pathfinder;

    private static GameManager _instance;

    TMP_Text selected_name;
    TMP_Text selected_desc;
    GameObject[] selected_items;
    List<List<TMP_Text>> selected_items_text;
    GameObject dialog_canvas;

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

        battleController = GameObject.Find("Battle").GetComponent<Battle>();

        GameObject.FindGameObjectWithTag("UI_Selected_Name").GetComponent<Button>();

        selected_name = GameObject.FindGameObjectWithTag("UI_Selected_Name").GetComponent<TMP_Text>();
        selected_desc = GameObject.FindGameObjectWithTag("UI_Selected_Desc").GetComponent<TMP_Text>();
        selected_items = GameObject.FindGameObjectsWithTag("UI_Items");
        selected_items_text = selected_items.Select(
            obj => obj.GetComponentsInChildren<TMP_Text>().ToList<TMP_Text>()
            ).ToList<List<TMP_Text>>();
        dialog_canvas = GameObject.Find("UI_Canvas_Dialog");
        dialog_canvas.SetActive(false);
        SetSelectedCharacter(GameObject.FindGameObjectWithTag("Player"));
        pathfinder = GameObject.Find("Pathfinding").GetComponent<AstarPath>();

		gameState = GameState.RPG;
	}

    public void SwitchGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void SetSelectedCharacter(GameObject sel)
    {
        selectedCharacter = sel;
        CombatEntity ce = selectedCharacter.GetComponent<CombatEntity>();

        // Fill selected object text
        selected_name.text = ce.character_name;
        selected_desc.text = ce.character_desc;

        for (int i = 0; i < ce.items.Count; i++)
        {
            Item item = ce.items[i];
            selected_items[i].SetActive(true);

            // UI click sets selected item
            selected_items[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("SELECTING ITEM");
                    selectedItem = item;
                }
                );

            // Fill items text
            selected_items_text[i][0].text = item.item_stats.item_name;
            selected_items_text[i][2].text = item.ammo.ToString();
            if (item.cooldown_timer > 0)
            {
                Debug.Log(item.cooldown_timer);
                selected_items[i].GetComponent<Button>().interactable = false;
                selected_items_text[i][1].text = item.cooldown_timer.ToString();
            }
            else
            {
                selected_items[i].GetComponent<Button>().interactable = true;
                selected_items_text[i][1].text = item.item_stats.cooldown_period.ToString();
            }
        }
        for (int i = ce.items.Count; i < 5; i++)
        {
            selected_items[i].SetActive(false);
        }
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
                        GameObject.Find("Tilemap_L2").layer = 0;
                        characterController.SetCharacterPosition(gridLocation + new Vector2Int(1, 0));
						GameObject.Find("Tilemap_L1").layer = 6;
					} else {
						GameObject.Find("Tilemap_L1").layer = 0;
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(1, 0));
						GameObject.Find("Tilemap_L2").layer = 6;
					}
					
                }
                break;
            case 8: // right
				if(selectedCharacter && selectedCharacter.TryGetComponent<CharacterController>(out characterController)) {
                    characterController.characterSelected = false;
					if(board.GetTile((int)characterController.transform.position.x, (int)characterController.transform.position.y).tileData.height == 0) {
						GameObject.Find("Tilemap_L2").layer = 0;
						characterController.SetCharacterPosition(gridLocation + new Vector2Int(-1, 0));
						GameObject.Find("Tilemap_L1").layer = 6;
					} else {
						GameObject.Find("Tilemap_L1").layer = 0;
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(-1, 0));
						GameObject.Find("Tilemap_L2").layer = 6;
					}

				}
				break;
            case 9: // bottom
				if(selectedCharacter && selectedCharacter.TryGetComponent<CharacterController>(out characterController)) {
					characterController.characterSelected = false;
					if(board.GetTile((int)characterController.transform.position.x, (int)characterController.transform.position.y).tileData.height == 0) {
						GameObject.Find("Tilemap_L2").layer = 0;
						characterController.SetCharacterPosition(gridLocation + new Vector2Int(0, 1));
                        GameObject.Find("Tilemap_L1").layer = 6;
					} else {
						GameObject.Find("Tilemap_L1").layer = 0;
						characterController.SetCharacterPosition(gridLocation - new Vector2Int(0, 1));
						GameObject.Find("Tilemap_L2").layer = 6;
					}
				}
				break;
            default: break;
        }

        pathfinder.Scan();
    }

    public void ToggleDialog(string speaker, string text) {
        TMP_Text[] dialog = dialog_canvas.GetComponentsInChildren<TMP_Text>();
        dialog[0].text = speaker;
        dialog[1].text = text;

		dialog_canvas.SetActive(!dialog_canvas.activeInHierarchy);
    }

    public void ClearDialog() {
		dialog_canvas.SetActive(false);
	}

    public int UseSelectedItem(CombatEntity self, CombatEntity target) {
        if(selectedItem != null) {
            selectedItem.Use(self, target);
            return selectedItem.item_stats.use_cost;
        }

        return 0;
    }

    public void UpdateBattlePoints(int pointsUsed) {
        battleController.UpdatePlayerActions(pointsUsed);
    }

	private void Update() {
		switch(gameState) {
			case GameState.RPG:
				// TODO : Free Move
				break;

			case GameState.PLAYER_TURN:
				// TODO : Item Interaction

				// Left Click On Board - Use Item
				// Right Click - Cancel Item

				// End turn when no actions remaining
				if(battleController.currentPlayerPoints <= 0) {
					battleController.PlayerTurnEnd();
				}
				break;

			case GameState.ENEMY_TURN:
				// Wait
				break;

			default:
				break;
		}
	}
}

