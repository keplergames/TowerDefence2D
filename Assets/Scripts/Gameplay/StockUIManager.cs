using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StockUIManager : MonoBehaviour {
	
	
	#region Singleton Logic
	private static StockUIManager instance;
	
	void Awake () {
		instance = this;
	}
	
	private static bool IsInstance () {
		if (instance == null) {
			instance = FindObjectOfType (typeof (StockUIManager)) as StockUIManager;
			
			if (instance == null) {
				Debug.LogError ("Can't seem to find any Gameobject that has StockUIManger class");
				return false;
			}
		}
		
		return true;
	}
	#endregion
	
	public GameObject stockUIItem;
	
	private UIGrid uiGrid;
	
	void OnEnable () {
		LocalRefresh ();
	}
	
	
	void LocalRefresh () {
		Util.DestroyChildren (instance.gameObject);
		
		foreach (Unit unit in UnitManager.GetUnitsInStock()) {
			GameObject uiItem = Instantiate ( stockUIItem ) as GameObject;
			uiItem.transform.parent = transform;
			uiItem.GetComponent <StockButton>().unit = unit;
			uiItem.GetComponent <UISprite>().spriteName = unit.spriteName;
		}
		
		uiGrid = GetComponent <UIGrid>();
		uiGrid.Reposition();
	}
	
	public static void Refresh () {
		instance.LocalRefresh();
	}
	
	public void BeginBuilding () {
		Game.GamePhase = GamePhase.Building;
	}
}

