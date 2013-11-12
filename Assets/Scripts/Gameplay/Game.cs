using UnityEngine;
using System.Collections;

public static class Game  {
	
	private static GamePhase gamePhase;
	
	public static GamePhase GamePhase {
		get { return gamePhase; }
		set {
			if ( OnGamePhaseChanged != null )
				OnGamePhaseChanged ( gamePhase, value );
			
			gamePhase = value;	
		}
	}
	
	public delegate void GamePhaseHandler ( GamePhase previous, GamePhase current );
	public static event GamePhaseHandler OnGamePhaseChanged;
	
}
