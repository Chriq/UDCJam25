using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public int playerPointsPerTurn;
	public int enemyPointsPerTurn;

    private int currentPlayerPoints;
    private int currentEnemyPoints;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
        currentPlayerPoints = playerPointsPerTurn;
        currentEnemyPoints = enemyPointsPerTurn;
    }

    // Update is called once per frame
    void Update() {
        if(GameManager.Instance.gameState == GameState.PLAYER_TURN) {
			PlayerTurn();
        } else if(GameManager.Instance.gameState == GameState.ENEMY_TURN) {
			Invoke("EnemyTurn", 2f);
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
		player.inputActive = true;
		currentPlayerPoints -= player.lastMovement;
		player.lastMovement = 0;

		if(currentPlayerPoints <= 0) {
			currentEnemyPoints = enemyPointsPerTurn;
			if(player) {
				player.inputActive = false;
			}
			GameManager.Instance.SwitchGameState(GameState.ENEMY_TURN);
			GameManager.Instance.selectedCharacter = GameObject.Find("Enemy");
		}
	}

	/*IEnumerator EnemyTurn() {
        yield return new WaitForSeconds(2);
        AIInput ai = GameManager.Instance.selectedCharacter.GetComponent<AIInput>();
        if(ai != null) {
            int moves = ai.Move();
            currentEnemyPoints -= moves;
        }
        
		yield return new WaitForSeconds(2);

		if(currentEnemyPoints <= 0) {
			GameManager.Instance.SwitchGameState(GameState.PLAYER_TURN);
			currentPlayerPoints = playerPointsPerTurn;
		}
	}

    IEnumerator PlayerTurn() {
		PlayerInput player = GameManager.Instance.selectedCharacter.GetComponent<PlayerInput>();
        player.inputActive = true;
		currentPlayerPoints -= player.lastMovement;
        player.lastMovement = 0;

        yield return new WaitForSeconds(2);

        if(currentPlayerPoints <= 0) {
            currentEnemyPoints = enemyPointsPerTurn;
            if(player) {
                player.inputActive = false;
            }
			GameManager.Instance.SwitchGameState(GameState.ENEMY_TURN);
		}
	}*/
}
