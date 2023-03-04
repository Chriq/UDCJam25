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

		selectedCharacter = GameObject.Find("Player");

        gameState = GameState.RPG;

		DontDestroyOnLoad(gameObject);
	}

    public void SwitchGameState(GameState gameState) {
        this.gameState = gameState;
    }
}

public enum GameState {
    RPG,
    PLAYER_TURN,
    ENEMY_TURN
}