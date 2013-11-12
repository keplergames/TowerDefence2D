using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotUIManager : MonoBehaviour {
	
	
	#region Singleton Logic
	private static SlotUIManager instance;
	
	void Awake () {
		instance = this;
	}
	
	private static bool IsInstance () {
		if (instance == null) {
			instance = FindObjectOfType (typeof (SlotUIManager)) as SlotUIManager;
			
			if (instance == null) {
				Debug.LogError ("Can't seem to find any Gameobject that has SlotUIManager class");
				return false;
			}
		}
		
		return true;
	}
	#endregion
	
	public GameObject slotUIItem;
	
	private UIGrid uiGrid;
	
	void OnEnable () {
		LocalRefresh();
	}
	
	void LocalRefresh () {
		Util.DestroyChildren (instance.gameObject);
		
		foreach (Unit unit in UnitManager.GetUnitsInSlots()) {
			GameObject uiItem = Instantiate ( slotUIItem ) as GameObject;
			uiItem.transform.parent = transform;
			uiItem.GetComponent <SlotButton>().unit = unit;
			uiItem.GetComponent <UISprite>().spriteName = unit.spriteName;
		}
		
		uiGrid = GetComponent <UIGrid>();
		uiGrid.Reposition();
	}
	
	public static void Refresh () {
		instance.LocalRefresh();
	}

}

