using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private static GameManager _instance;

    TMP_Text selected_name;
    TMP_Text selected_desc;
    GameObject[] selected_items;
    List<List<TMP_Text>> selected_items_text;

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

        SetSelectedCharacter(GameObject.FindGameObjectWithTag("Player"));

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

    private void Update()
    {
        switch (gameState)
        {
            case GameState.RPG:
                // TODO : Free Move
                break;
            case GameState.PLAYER_TURN:
                // TODO : Item Interaction

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
