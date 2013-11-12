using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCreator : MonoBehaviour {
	
	public Unit unitToCreate;
	
	public static Unit UnitToCreate {
		get { return instance.unitToCreate; }
		set {
			instance.unitToCreate = value;	
		}
	}
	
	#region Singleton Logic
	private static UnitCreator instance;
	
	void Awake () {
		instance = this;
	}
	
	private static bool IsInstance () {
		if (instance == null) {
			instance = FindObjectOfType (typeof (UnitCreator)) as UnitCreator;
			
			if (instance == null) {
				Debug.LogError ("Can't seem to find any Gameobject that has UnitCreator class");
				return false;
			}
		}
		
		return true;
	}
	#endregion
	
	public bool canCreate;
	
	void OnEnable () {
		Game.OnGamePhaseChanged += OnGamePhaseChanged;
	}
	
	void OnDisable () {
		Game.OnGamePhaseChanged -= OnGamePhaseChanged;
	}

	void OnGamePhaseChanged (GamePhase previous, GamePhase current) {
		switch ( current ) {
		case GamePhase.Selection:
			DisableCreate();
			break;
		case GamePhase.Building:
		case GamePhase.Defending:
			Invoke ("EnableCreate", 0.5f);
			break;
		}	
	}
	
	void EnableCreate () {
		canCreate = true;
	}
	
	void DisableCreate () {
		canCreate = false;	
	}
	
	#region Unit Placement and Hightlighting Terrain
	GameObject selectedTerrainBlockGO;
	TerrainBlock selectedTerrainBlock;
	List<TerrainBlock> highlightedTerrainBlocks;
	RaycastHit hit;
	float yPos;
	
	private GameObject instiantedUnit;
	
	void OnTap ( Gesture e ) {
		
		if ( unitToCreate == null || !canCreate ) return;
		
		if ( !unitToCreate.Recharged ) return;
		
		selectedTerrainBlockGO = e.Selection;
		if ( selectedTerrainBlockGO == null ) return;
		
		selectedTerrainBlock = selectedTerrainBlockGO.GetComponent<TerrainBlock>();
		if ( selectedTerrainBlock == null ) return;
		
		if ( selectedTerrainBlock.HasUnit ) return;
		
		if ( instiantedUnit == null )
			instiantedUnit = Instantiate ( unitToCreate.gameObject ) as GameObject;
		
		instiantedUnit.SetXY ( selectedTerrainBlockGO.GetX(), selectedTerrainBlockGO.GetY() );
		
		if ( selectedTerrainBlock.HasUnit )
			Destroy ( instiantedUnit );
		else {
			selectedTerrainBlock.unitOnTerrain = instiantedUnit.GetComponent<Unit>();
			selectedTerrainBlock.unitOnTerrain.terrainBlock = selectedTerrainBlock;
			selectedTerrainBlock.unitOnTerrain.UnitPlaced = true;
		}
		
		selectedTerrainBlockGO = null;
		selectedTerrainBlock = null;
		instiantedUnit = null;
		unitToCreate.LastUsedTime = Time.time;
	}
	
	void OnDrag ( DragGesture e ) {
		if ( unitToCreate == null || !canCreate ) return;
		
		if ( !unitToCreate.Recharged ) return;
		
		switch ( e.State ) {
		case GestureRecognitionState.Started:
			if ( instiantedUnit == null ) {
				instiantedUnit = Instantiate ( unitToCreate.gameObject ) as GameObject;
				instiantedUnit.renderer.enabled = false;
			}
			
			break;
			
		case GestureRecognitionState.InProgress:
			selectedTerrainBlockGO = e.Selection;
			
			if ( selectedTerrainBlockGO == null ) {
				if ( instiantedUnit != null )
					instiantedUnit.renderer.enabled = false;
				return;
			}
			
			selectedTerrainBlock = selectedTerrainBlockGO.GetComponent<TerrainBlock>();
			if ( selectedTerrainBlock == null ) { 
				if ( instiantedUnit != null )
					instiantedUnit.renderer.enabled = false;
				return;
			}
			
			if ( instiantedUnit != null )
				instiantedUnit.renderer.enabled = true;
			
			if ( highlightedTerrainBlocks != null )
			foreach ( TerrainBlock block in highlightedTerrainBlocks )
				block.SpriteRenderer.VertexColor = block.DefaultColor;
			
			// Highlight selection of the row and column from the terrain block.
			highlightedTerrainBlocks = TerrainManager.GetTerrainRowAndColumn (selectedTerrainBlock);
			
			foreach ( TerrainBlock block in highlightedTerrainBlocks ) {
				block.SpriteRenderer.VertexColor = Color.red;
			}
			
			if ( instiantedUnit != null )
				instiantedUnit.SetXY ( selectedTerrainBlockGO.GetX(), selectedTerrainBlockGO.GetY() );
			
			break;
			
		case GestureRecognitionState.Ended:
			if ( highlightedTerrainBlocks != null )
				foreach ( TerrainBlock block in highlightedTerrainBlocks )
					block.SpriteRenderer.VertexColor = block.DefaultColor;
			
			selectedTerrainBlockGO = e.Selection;
			if ( selectedTerrainBlockGO == null ) {
				if ( instiantedUnit != null ) {
					Destroy ( instiantedUnit );
				}
				return; 
			}
			
			selectedTerrainBlock = selectedTerrainBlockGO.GetComponent<TerrainBlock>();
			if ( selectedTerrainBlock == null ) { 
				if ( instiantedUnit != null ) {
					instiantedUnit.renderer.enabled = false;
					Destroy ( instiantedUnit );
				}
				return;
			}
			
			if ( selectedTerrainBlock.HasUnit )
				Destroy ( instiantedUnit );
			else {
				if ( selectedTerrainBlock == null )
					return;
				
				selectedTerrainBlock.unitOnTerrain = instiantedUnit.GetComponent<Unit>();
				selectedTerrainBlock.unitOnTerrain.terrainBlock = selectedTerrainBlock;
				selectedTerrainBlock.unitOnTerrain.UnitPlaced = true;
			}
			
			selectedTerrainBlockGO = null;
			selectedTerrainBlock = null;
			instiantedUnit = null;
			UnitToCreate.LastUsedTime = Time.time;
			
			break;
		}
	}
	#endregion
}
