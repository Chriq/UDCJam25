using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{
	[SerializeField] int playerPointsPerTurn;
	[SerializeField] int enemyPointsPerTurn;
	public List<GameObject> enemies;
	List<GameObject> enemies_prioritized;

	[SerializeField] public int currentPlayerPoints;
	[SerializeField] int currentEnemyPoints;
	public List<GameObject> players;

	[SerializeField] TMP_Text UI_ActionsRemaining;

	void Start()
    {
		foreach (GameObject obj in players) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnAllyKilled);
		}
		foreach(GameObject obj in enemies) {
			obj.GetComponent<CombatEntity>().onKilledEvent.AddListener(OnEnemyKilled);
		}
	}

    private void Awake()
    {
		UI_ActionsRemaining = GameObject.FindGameObjectWithTag("UI_ActionsRemaining").GetComponent<TMP_Text>();

		GameObject.FindGameObjectWithTag("UI_EndTurn").GetComponent<Button>().onClick.AddListener(PlayerTurnEnd);

		// Initialize all items
		foreach (GameObject obj in enemies)
		{
			foreach (Item item in obj.GetComponent<CombatEntity>().items)
			{
				item.Init();
			}
		}
		foreach (GameObject obj in players)
		{
			foreach (Item item in obj.GetComponent<CombatEntity>().items)
			{
				item.Init();
			}
		}
	}
	
	/*
    void InitBattle() {
        if(GameManager.Instance.gameState == GameState.PLAYER_TURN) {
			PlayerTurn();
        } else if(GameManager.Instance.gameState == GameState.ENEMY_TURN) {
			Invoke("EnemyTurn", 2f);
        }
    }
	*/

	void UpdatePlayerActions(int current, int sub)
    {
		currentPlayerPoints = current - sub;
		UI_ActionsRemaining.text = currentPlayerPoints.ToString();
	}

	void PlayEenmy_PriorityQueue()
    {
		GameObject enemy = enemies_prioritized[0];
		enemies_prioritized.RemoveAt(0);

		GameManager.Instance.SetSelectedCharacter(enemy);
		enemy.GetComponent<CombatEntity>().AutoPlay(players, ref currentEnemyPoints);
	}

	void EnemyTurn() {
		// Start Turn
		currentEnemyPoints = enemyPointsPerTurn;

		// Combat AI
		//float[] enemy_potential = enemies.Select(enemy => enemy.GetComponent<CombatEntity>().CombatPotential(players)).ToArray();
		enemies_prioritized = enemies.OrderByDescending(enemy => enemy.GetComponent<CombatEntity>().CombatPotential(players)).ToList();

		float delay = 1f;
		foreach (GameObject enemy in enemies_prioritized)
		{
			Invoke("PlayEenmy_PriorityQueue", delay++);
        }

		// End Turn
		Invoke("PlayerTurnStart", delay);
	}

	void PlayerTurnStart() {
		Debug.Log("Player Turn Start");
		// Start Turn
		GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
		GameManager.Instance.SetSelectedCharacter(GameObject.Find("Player"));
		UpdatePlayerActions(playerPointsPerTurn, 0);

		// Cycle all Items
		foreach (GameObject obj in enemies)
		{
			foreach (Item item in obj.GetComponent<CombatEntity>().items)
			{
				item.Cycle();
			}
		}
		foreach (GameObject obj in players)
		{
			foreach (Item item in obj.GetComponent<CombatEntity>().items)
			{
				item.Cycle();
			}
		}

		// To be implemented in movement as an item
		/*PlayerInput player = GameManager.Instance.selectedCharacter.GetComponent<PlayerInput>();
		if(player) {
			player.inputActive = true;
			UpdatePlayerActions(currentPlayerPoints, player.lastMovement);
			player.lastMovement = 0;
		}*/
	}
	public void PlayerTurnEnd()
    {
		Debug.Log("Player Turn End");
		// End Turn
		if (GameManager.Instance.gameState == GameState.PLAYER_TURN)
        {
			GameManager.Instance.SwitchGameState(GameState.ENEMY_TURN);
			Invoke("EnemyTurn", 2f);
		}
	}

	void WinBattle() {
		Debug.Log("Battle Won");
		GameManager.Instance.SetSelectedCharacter(GameObject.Find("Player"));
		GameManager.Instance.SwitchGameState(GameState.RPG);
		Destroy(gameObject);
	}

	void LoseBattle() {
		Debug.Log("Battle Lost");
		Destroy(gameObject);
	}

	void OnAllyKilled(GameObject ally)
    {
		players.Remove(ally);

		if (players.Count == 0)
		{
			LoseBattle();
		}
	}
	void OnEnemyKilled(GameObject enemy) {
		enemies.Remove(enemy);

		if (enemies.Count == 0)
		{
			WinBattle();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Debug.Log("Test");
		if(collision.gameObject.tag == "Player") {
			// Sstart Combat
			PlayerTurnStart();
		}
	}
}
