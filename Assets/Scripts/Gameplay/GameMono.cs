using UnityEngine;
using System.Collections;

public class GameMono : MonoBehaviour {

	void Start () {
		Application.targetFrameRate = 60;
		Game.GamePhase = GamePhase.Selection;
	}
}
