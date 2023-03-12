using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData _instance;

	public bool carnegieObjectiveComplete = false;
	public bool rockefellerObjectiveComplete = false;
	public bool jpMorganObjectiveComplete = false;

	public bool carnegieKilled = false;
	public bool rockefellerKilled = false;
	public bool jpMorganKilled = false;

	public static GameData Instance {
		get {
			if(_instance == null) {
				_instance = new GameObject("GameData").AddComponent<GameData>();
			}
			return _instance;
		}
	}

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}
}
