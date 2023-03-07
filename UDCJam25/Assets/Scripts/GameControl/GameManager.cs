using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public Board board;
    public GameObject selectedCharacter;
    public GameState gameState;

    private static GameManager _instance;

    [SerializeField]
    TMP_Text selected_name;
    [SerializeField]
    TMP_Text selected_desc;
    [SerializeField]
    GameObject[] selected_items;
    [SerializeField]
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

		selected_name = GameObject.FindGameObjectWithTag("UI_Selected_Name").GetComponent<TMP_Text>();
        selected_desc = GameObject.FindGameObjectWithTag("UI_Selected_Desc").GetComponent<TMP_Text>();
        selected_items = GameObject.FindGameObjectsWithTag("UI_Items");
        selected_items_text = selected_items.Select(
            obj => obj.GetComponentsInChildren<TMP_Text>().ToList<TMP_Text>()
            ).ToList<List<TMP_Text>>();

        SetSelectedCharacter(GameObject.FindGameObjectWithTag("Player"));

        gameState = GameState.RPG;
	}

    public void SetSelectedCharacter(GameObject sel)
    {
        selectedCharacter = sel;
        List<Item> char_items = selectedCharacter.GetComponent<CombatEntity>().items;

        for (int i = 0; i < char_items.Count; i++)
        {
            Item item = char_items[i];
            selected_items_text[i][0].text = item.item_name;
            selected_items_text[i][2].text = item.ammunition.ToString();
            if (item.cooldown_timer > 0)
            {
                selected_items[i].GetComponent<Button>().interactable = false;
                selected_items_text[i][1].text = item.cooldown_timer.ToString();
            }
            else
            {
                selected_items[i].GetComponent<Button>().interactable = true;
                selected_items_text[i][1].text = item.cooldown_period.ToString();
            }
        }
        for (int i = char_items.Count; i < 5; i++)
        {
            selected_items[i].SetActive(false);
        }
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
}

public enum GameState {
    RPG,
    PLAYER_TURN,
    ENEMY_TURN
}