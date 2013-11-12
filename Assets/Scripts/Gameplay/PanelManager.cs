using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour {

	public GameObject stockPanel;
	public GameObject slotsPanel;
	public GameObject gameOverPanel;
	
	void OnEnable () {
		Game.OnGamePhaseChanged += OnGamePhaseChanged;
	}
	
	void OnDisable () {
		Game.OnGamePhaseChanged -= OnGamePhaseChanged;
	}

	void OnGamePhaseChanged (GamePhase previous, GamePhase current) {
		switch ( current ) {
		case GamePhase.Selection:
			gameOverPanel.SetActive ( false );
			
			stockPanel.SetLocalX ( -2000f );
			Go.to ( stockPanel.transform, 1f, new GoTweenConfig()
				.setEaseType (GoEaseType.ElasticOut)
				.localPosition ( new Vector3 ( 0, 106.6675f, 0), false));
			
				
			break;
			
		case GamePhase.Building:
			Go.to ( stockPanel.transform, 0.5f, new GoTweenConfig()
				.setEaseType (GoEaseType.BackIn)
				.localPosition ( new Vector3 ( -2000f, 106.6675f, 0), false));
			break;
			
		case GamePhase.Defending:
			
			break;
			
		case GamePhase.GameOver:
			gameOverPanel.transform.localScale = Vector3.zero;
			gameOverPanel.SetActive ( true );
			Go.to ( gameOverPanel.transform, 1f, new GoTweenConfig()
				.setEaseType (GoEaseType.ElasticOut)
				.scale ( Vector3.one, false ) );
			
			break;
		}
	}
	
}
