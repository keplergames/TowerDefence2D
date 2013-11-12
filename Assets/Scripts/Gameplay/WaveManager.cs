using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
	
	public GameObject[] enemyPrefabs;
	public float interval;
	
	private float[] rowPositions;

	// Use this for initialization
	void Start () {
		rowPositions = TerrainManager.GetRowPositions();
		
	}
	
	void OnEnable () {
		Game.OnGamePhaseChanged += OnGamePhaseChanged;
	}
	
	void OnDisable () {
		Game.OnGamePhaseChanged -= OnGamePhaseChanged;
	}

	void OnGamePhaseChanged (GamePhase previous, GamePhase current) {
		switch ( current ) {
		case GamePhase.Selection:
			if (IsInvoking ("GenerateEnemy"))
				CancelInvoke ("GenerateEnemy");
			break;
		
		case GamePhase.Building:
			if (IsInvoking ("GenerateEnemy"))
				CancelInvoke ("GenerateEnemy");
			
			InvokeRepeating ("GenerateEnemy", 5, interval);
			
			break;
			
		case GamePhase.GameOver:
			if (IsInvoking ("GenerateEnemy"))
				CancelInvoke ("GenerateEnemy");
			break;
		}
	}
	
	void GenerateEnemy () {
		Game.GamePhase = GamePhase.Defending;
		Instantiate (enemyPrefabs[0], new Vector3 ( 10, GetRandomRow(), 0 ), Quaternion.identity);
	}
	
	float GetRandomRow () {
		return rowPositions[ Random.Range (0, rowPositions.Length) ];
	}
				
}
