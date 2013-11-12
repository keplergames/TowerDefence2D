using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotButton : MonoBehaviour {
	
	private static List<SlotButton> slotButtons = new List<SlotButton>();
	
	public Unit unit;
	public UISprite fillSprite;
	
	private bool isSelected;
	
	public bool IsSelected {
		get { return isSelected; }
		set {
			isSelected = value;
			if (isSelected) StartAnimation();
			else StopAnimation ();
		}
	}
	
	private TweenColor tweenColor;
	
	void OnEnable () {
		tweenColor = GetComponent <TweenColor>();
		tweenColor.enabled = false;
		slotButtons.Add (this);
	}
	
	void OnDisable () {
		slotButtons.Remove (this);	
	}
	
	void OnPress ( bool isPressed ) {
		if (isPressed) {
			if (Game.GamePhase == GamePhase.Selection) {
				UnitManager.MoveUnitToStock ( unit );
				SlotUIManager.Refresh ();
				StockUIManager.Refresh ();
			} else {
				DisableOtherButtons (this);
				IsSelected = !IsSelected;
				UnitCreator.UnitToCreate = (IsSelected) ? unit : null;
			}
		}
	}
	
	void DisableOtherButtons ( SlotButton slotButton ) {
		for (int i = 0; i < slotButtons.Count; i++) {
			if ( slotButtons[i] != slotButton ) {
				//Debug.Log (slotButtons[i].unit.name);
				slotButtons[i].IsSelected = false;
			}
			
		}
	}
	
	void StartAnimation () {
		tweenColor.enabled = true;
	}
	
	void StopAnimation () {
		tweenColor.color = Color.white;
		tweenColor.enabled = false;
	}
	
	void Update () {
		if (!unit.CheckRecharged())
			fillSprite.fillAmount = unit.GetRechargedValue();
	}
}
