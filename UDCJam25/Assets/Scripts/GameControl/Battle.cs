using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public int playerPointsPerTurn;
	public int enemyPointsPerTurn;
	public List<GameObject> enemies;

    private int currentPlayerPoints;
    private int currentEnemyPoints;
	public List<GameObject> players;

	// Start is called before the first frame update
	void Start()
    {
        GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
        currentPlayerPoints = playerPointsPerTurn;
        currentEnemyPoints = enemyPointsPerTurn;

		foreach(GameObject obj in players) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnEnemyKilled);
		}
		foreach(GameObject obj in enemies) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnEnemyKilled);
		}
	}

    void Update() {
        if(GameManager.Instance.gameState == GameState.PLAYER_TURN) {
			Debug.Log("Player Turn");
			PlayerTurn();
        } else if(GameManager.Instance.gameState == GameState.ENEMY_TURN) {
			Debug.Log("Enemy Turn");
			Invoke("EnemyTurn", 2f);
        }

		if(enemies.Count == 0) {
			WinBattle();
		}

		if(players.Count == 0) {
			LoseBattle();
		}
    }

	void EnemyTurn() {
		AIInput ai = GameManager.Instance.selectedCharacter.GetComponent<AIInput>();
		if(ai != null) {
			int moves = ai.Move();
			currentEnemyPoints -= moves;
		}

		if(currentEnemyPoints <= 0) {
			GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
			currentPlayerPoints = playerPointsPerTurn;
			GameManager.Instance.selectedCharacter = GameObject.Find("Player");
		}
	}

	void PlayerTurn() {
		PlayerInput player = GameManager.Instance.selectedCharacter.GetComponent<PlayerInput>();
		if(player) {
			player.inputActive = true;
			currentPlayerPoints -= player.lastMovement;
			player.lastMovement = 0;
		}
		

		if(currentPlayerPoints <= 0) {
			currentEnemyPoints = enemyPointsPerTurn;
			if(player) {
				player.inputActive = false;
			}
			GameManager.Instance.SwitchGameState(GameState.ENEMY_TURN);
			GameManager.Instance.selectedCharacter = GameObject.Find("Enemy");
		}
	}

	void WinBattle() {
		GameManager.Instance.selectedCharacter = GameObject.Find("Player");
		GameManager.Instance.SwitchGameState(GameState.RPG);
		Destroy(gameObject);
	}

	void LoseBattle() {
		Destroy(gameObject);
	}

	void OnEnemyKilled(GameObject enemy) {
		enemies.Remove(enemy);
	}
}
