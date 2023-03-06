using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
	CharacterController controller;

	private void Awake() {
		controller = GetComponent<CharacterController>();
	}
	
	public int Move() {
		return controller.SetCharacterPosition(new Vector2Int (3, 3));
	}

	private void OnTriggerEnter2D(Collider2D other) {
		GameObject.Find("Battle").GetComponent<Battle>().enabled = true;
		;
	}
}
