using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {
	
	public List<Unit> units;
	
	public int maximumSlots;
	public List<Unit> unitsInSlots;

	#region Singleton Logic
	private static UnitManager instance;
	
	void Awake () {
		instance = this;
	}
	
	private static bool IsInstance () {
		if (instance == null) {
			instance = FindObjectOfType (typeof (UnitManager)) as UnitManager;
			
			if (instance == null) {
				Debug.LogError ("Can't seem to find any Gameobject that has UnitManager class");
				return false;
			}
		}
		
		return true;
	}
	#endregion
	
	void OnEnable () {
		Game.OnGamePhaseChanged += OnGamePhaseChanged;
	}
	
	void OnDisable () {
		Game.OnGamePhaseChanged -= OnGamePhaseChanged;
	}

	void OnGamePhaseChanged (GamePhase previous, GamePhase current) {
		switch ( current ) {
		case GamePhase.Selection:
			foreach ( Unit unit in units )
				unit.ResetRecharge ();
			break;
		}
	}
	
	public static void MoveUnitToSlot (Unit unit) {
		instance.units.Remove (unit);
		instance.unitsInSlots.Add (unit);
	}
	
	public static void MoveUnitToStock ( Unit unit ) {
		instance.units.Add (unit);
		instance.unitsInSlots.Remove (unit);
	}
	
	public static Unit[] GetUnitsInStock () {
		return instance.units.ToArray();	
	}
	
	public static List<Unit> GetUnitsInSlots () {
		return instance.unitsInSlots;	
	}
}
