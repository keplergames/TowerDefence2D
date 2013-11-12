using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainManager : MonoBehaviour {
	
	#region Create Base Blocks
	public GameObject terrainPrefab;
	public int rows = 2, columns = 3;
	
	private GameObject terrainContainer;
	
	void CreateBlocks () {
		Debug.Log ("Building Terrain Blocks");
		terrainContainer = GameObject.Find ("_TerrainBlocks");
		
		if (terrainContainer != null)
			DestroyImmediate (terrainContainer);
		
		terrainContainer = new GameObject ("_TerrainBlocks");
		
		terrainArray = new TerrainBlock[rows, columns];
		
		for ( int row = 0; row < rows; row ++) {
			for ( int column = 0; column < columns; column ++) {
				GameObject terrainGO = Instantiate (terrainPrefab, new Vector3 ( column * 1.74f, row * -1.74f,  0), Quaternion.identity) as GameObject;
				terrainGO.transform.parent = terrainContainer.transform;
				terrainGO.name = "TerrainBlock_" + row.ToString ("00") + "x" + column.ToString ("00");
				
				TerrainBlock terrainBlock = terrainGO.AddComponent<TerrainBlock> ();
				terrainBlock.SetRowAndColumn (row, column);
				
				terrainArray[row, column] = terrainBlock;
			}
		}
	
	}
	#endregion
	
	#region Singleton Logic
	private static TerrainManager instance;
	
	void Awake () {
		instance = this;
	}
	
	private static bool IsInstance () {
		if (instance == null) {
			instance = FindObjectOfType (typeof (TerrainManager)) as TerrainManager;
			
			if (instance == null) {
				Debug.LogWarning ("Can't seem to find any Gameobject that has TerrainManager class");
				return false;
			}
		}
		
		return true;
	}
	
	#endregion
	
	#region Terrain Base Management
	private TerrainBlock[,] terrainArray;
	
	/// <summary>
	/// Registers a terrain block to the TerrainManager.
	/// </summary>
	/// <param name='row'>
	/// Row.
	/// </param>
	/// <param name='column'>
	/// Column.
	/// </param>
	/// <param name='terrainBlock'>
	/// Terrain block.
	/// </param>
	public static void RegisterTerrainBlock (int row, int column, TerrainBlock terrainBlock) {
		if (!IsInstance ())
			return;
		
		if ( instance.terrainArray == null )
			instance.terrainArray = new TerrainBlock [instance.rows, instance.columns];
		
		instance.terrainArray [row, column] = terrainBlock;
	}
	
	/// <summary>
	/// Gets a terrain block.
	/// </summary>
	/// <returns>
	/// The terrain block.
	/// </returns>
	/// <param name='row'>
	/// Row.
	/// </param>
	/// <param name='column'>
	/// Column.
	/// </param>
	public static TerrainBlock GetTerrainBlock ( int row, int column ) {
		return instance.terrainArray[row, column];
	}
	
	/// <summary>
	/// Gets the entire row of the terrain block.
	/// </summary>
	/// <returns>
	/// The terrain row.
	/// </returns>
	/// <param name='terrainBlock'>
	/// Terrain block.
	/// </param>
	public static List<TerrainBlock> GetTerrainRow ( TerrainBlock terrainBlock ) {
		List<TerrainBlock> terrainBlocks = new List<TerrainBlock>();
		
		for (int column = 0; column < instance.columns; column++)
			terrainBlocks.Add (instance.terrainArray [ terrainBlock.row, column ]);
		
		return terrainBlocks;
	}
	
	/// <summary>
	/// Gets the entire column of the terrain block.
	/// </summary>
	/// <returns>
	/// The terrain column.
	/// </returns>
	/// <param name='terrainBlock'>
	/// Terrain block.
	/// </param>
	public static List<TerrainBlock> GetTerrainColumn ( TerrainBlock terrainBlock ) {
		List<TerrainBlock> terrainBlocks = new List<TerrainBlock>();
		
		for (int row = 0; row < instance.rows; row++)
			terrainBlocks.Add (instance.terrainArray [ row, terrainBlock.column ]);
		
		return terrainBlocks;
	}
	
	/// <summary>
	/// Gets the entire row and column of the terrain block.
	/// </summary>
	/// <returns>
	/// The terrain row and column.
	/// </returns>
	/// <param name='terrainBlock'>
	/// Terrain block.
	/// </param>
	public static List<TerrainBlock> GetTerrainRowAndColumn ( TerrainBlock terrainBlock ) {
		List <TerrainBlock> terrainBlocksRow = GetTerrainRow (terrainBlock);
		List <TerrainBlock> terrainBlocksColumn = GetTerrainColumn (terrainBlock);
		
		List <TerrainBlock> terrainBlocks = new List<TerrainBlock>();
		
		for ( int i = 0; i < terrainBlocksRow.Count; i++)
			if ( !terrainBlocks.Contains ( terrainBlocksRow[i] ) )
				terrainBlocks.Add (terrainBlocksRow[i]);
		
		for ( int j = 0; j < terrainBlocksColumn.Count; j++) 
			if ( !terrainBlocks.Contains ( terrainBlocksColumn[j] ) )
				terrainBlocks.Add (terrainBlocksColumn[j]);
		
		
		return terrainBlocks;
	}
	
	public static List<TerrainBlock> GetSquareTerrain ( TerrainBlock terrainBlock, int size ) {
		List <TerrainBlock> returnTerrainBlocks = new List<TerrainBlock>();
		
		int rowFrom, rowTo, columnFrom, columnTo;
		
		rowFrom = Mathf.Max (0, terrainBlock.row - size + 1);
		rowTo = Mathf.Min (instance.rows, terrainBlock.row + size);
		
		columnFrom = Mathf.Max (0, terrainBlock.column - size + 1);
		columnTo = Mathf.Min (instance.columns, terrainBlock.column + size);
		
		for ( int i = columnFrom; i < columnTo; i++ )
			returnTerrainBlocks.Add ( instance.terrainArray [terrainBlock.row, i] );
		
		for ( int j = rowFrom; j < rowTo; j++ )
			returnTerrainBlocks.Add (instance.terrainArray [j, terrainBlock.column] );
		
		return returnTerrainBlocks;
		
	}
	
	public static int GetRows () {
		return instance.rows;	
	}
	
	public static int GetColumns () {
		return instance.columns;	
	}
	
	public static float[] GetRowPositions () {
		float[] rowPositions = new float[instance.rows];
		for ( int row = 0; row < instance.rows; row ++ )
			rowPositions[row] = instance.terrainArray [row, 0].transform.position.y;
		
		return rowPositions;
	}
	#endregion
}
