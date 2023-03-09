using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CharacterController : MonoBehaviour
{
	public float characterSpeed = 10f;
	public bool characterSelected = false;

	public Seeker pathfinder;
	private Path path;
	private int currentWaypoint = 0;
	private bool reachedEndOfPath = true;
	private float nextWaypointDistance = 0.1f;

	public Vector2Int currentPosition;

	private void Awake() {
		pathfinder = gameObject.GetComponent<Seeker>();
	}

	void Start()
    {		
		currentPosition = new Vector2Int((int) transform.position.x, (int) transform.position.y);

		GameManager.Instance.board.GetTile(currentPosition.x, currentPosition.y).isOccupied = true;
		GameManager.Instance.board.GetTile(currentPosition.x, currentPosition.y).occupant = gameObject;
	}

    public int SetCharacterPosition(Vector2Int pos) {
		Tile currentTile = GameManager.Instance.board.GetTile((int)transform.position.x, (int)transform.position.y);
		Tile nextTile = GameManager.Instance.board.GetTile(pos.x, pos.y);

		currentTile.isOccupied = false;
		nextTile.isOccupied = true;
		nextTile.occupant = gameObject;
		currentPosition = pos;

		if(pathfinder.IsDone()) {
			path = pathfinder.StartPath(transform.position, new Vector3(pos.x, pos.y));
			currentWaypoint = 0;
		}

		return (int) path.GetTotalLength();
	}

	private void Update() {
		if(path == null) {
			return;
		}

		if(currentWaypoint >= path.vectorPath.Count) {
			reachedEndOfPath = true;
			if(path.vectorPath.Count > 0) {
				transform.position = new Vector3(currentPosition.x, currentPosition.y);
			}
			return;
		} else {
			reachedEndOfPath = false;
		}

		Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2) transform.position).normalized;
		transform.Translate(direction * characterSpeed * Time.deltaTime);

		float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
		if(distance < nextWaypointDistance) {
			currentWaypoint++;
		}
	}
}
