using UnityEngine;
using System.Collections;

public class StockButton : MonoBehaviour {

	public Unit unit;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPress ( bool isPressed ) {
		if (isPressed) {
			UnitManager.MoveUnitToSlot ( unit );
			StockUIManager.Refresh ();
			SlotUIManager.Refresh ();
		}
	}
}
