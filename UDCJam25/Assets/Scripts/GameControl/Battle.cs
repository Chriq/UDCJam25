using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle : MonoBehaviour
{
	[SerializeField] int playerPointsPerTurn;
	[SerializeField] int enemyPointsPerTurn;
	public List<GameObject> enemies;

	[SerializeField] int currentPlayerPoints;
	[SerializeField] int currentEnemyPoints;
	public List<GameObject> players;

	[SerializeField] TMP_Text UI_ActionsRemaining;

	void Start()
    {
        GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
		UpdatePlayerActions(playerPointsPerTurn, 0);
        currentEnemyPoints = enemyPointsPerTurn;

		/*
		foreach (GameObject obj in players) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnEnemyKilled);
		}
		foreach(GameObject obj in enemies) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnEnemyKilled);
		}
		*/
	}

    private void Awake()
    {
		UI_ActionsRemaining = GameObject.FindGameObjectWithTag("UI_ActionsRemaining").GetComponent<TMP_Text>();
	}

    void Update() {
        if(GameManager.Instance.gameState == GameState.PLAYER_TURN) {
			PlayerTurn();
        } else if(GameManager.Instance.gameState == GameState.ENEMY_TURN) {
			Invoke("EnemyTurn", 2f);
        }

		if(enemies.Count == 0) {
			WinBattle();
		}

		if(players.Count == 0) {
			LoseBattle();
		}
    }

	void UpdatePlayerActions(int current, int sub)
    {
		currentPlayerPoints = current - sub;
		UI_ActionsRemaining.text = currentPlayerPoints.ToString();
	}

	void EnemyTurn() {
		// Start Turn
		currentEnemyPoints = enemyPointsPerTurn;

		// Combat AI
		//float[] enemy_potential = enemies.Select(enemy => enemy.GetComponent<CombatEntity>().CombatPotential(players)).ToArray();
		List<GameObject> enemies_prioritized = enemies.OrderByDescending(enemy => enemy.GetComponent<CombatEntity>().CombatPotential(players)).ToList();

		foreach (GameObject enemy in enemies_prioritized)
        {
			currentEnemyPoints -= enemy.GetComponent<CombatEntity>().AutoPlay(players, currentEnemyPoints);

			if (currentEnemyPoints == 0)
				break;
        }
		/*
		AIInput ai = GameManager.Instance.selectedCharacter.GetComponent<AIInput>();
		if(ai != null) {
			int moves = ai.Move();
			currentEnemyPoints -= moves;
		}
		*/

		// End Turn
		GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
		UpdatePlayerActions(playerPointsPerTurn, 0);
		GameManager.Instance.selectedCharacter = GameObject.Find("Player");
	}

	void PlayerTurn() {
		// Start Turn
		//UpdatePlayerActions(playerPointsPerTurn, 0);

		PlayerInput player = GameManager.Instance.selectedCharacter.GetComponent<PlayerInput>();
		if(player) {
			player.inputActive = true;
			UpdatePlayerActions(currentPlayerPoints, player.lastMovement);
			player.lastMovement = 0;
		}
		
		// End Turn

		if(currentPlayerPoints <= 0) {
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
