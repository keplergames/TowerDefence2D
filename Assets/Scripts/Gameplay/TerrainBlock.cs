using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof (SpriteRenderer))]
public class TerrainBlock : MonoBehaviour {
	
	public int row, column;
	public List<Enemy> enemiesOnTerrain = new List<Enemy>();
	
	private Uni2DSprite spriteRenderer;
	
	public Uni2DSprite SpriteRenderer {
		get { return spriteRenderer; }	
	}
	
	
	private Color defaultColor;
	
	public Color DefaultColor {
		get {
			return defaultColor;
		}
	}
	
	void OnEnable () {
		TerrainManager.RegisterTerrainBlock ( row, column, this );
		spriteRenderer = GetComponent <Uni2DSprite>();
		defaultColor = spriteRenderer.VertexColor;
	}
	
	public void SetRowAndColumn (int row, int column) {
		this.row = row;
		this.column = column;
	}
	
	public Unit unitOnTerrain;
	
	public Unit UnitOnTerrain {
		get { return unitOnTerrain; }
		set {
			unitOnTerrain = value;
		}
	}
	
	public bool HasUnit {
		get {
			if ( unitOnTerrain != null )
				return true;
			else
				return false;
		}
	}
	
	public void AddEnemyToTerrain ( Enemy enemy ) {
		enemiesOnTerrain.Add (enemy);
	}
	
	public void RemoveEnemyFromTerrain ( Enemy enemy ) {
		enemiesOnTerrain.Remove (enemy);
	}
	
	public void KillAllEnemies () {
		foreach ( Enemy enemy in enemiesOnTerrain )
			if (enemy != null)
				enemy.Kill (0.3f);
	}
}
