using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	
	public string spriteName;
	public int positionID;
	
	public float health = 100;
	
	
	public float damage = 5;
	public int range = 0; //Zero means it'll hit the whole lane.
	
	public bool fireProjectile;
	
	public float rateOfFire = 2;
	public float setupTime;
	
	public bool explosive;
	public float explosionDelay = 0.5f; // Applies only if the explosive is set true.
	public bool triggeredExplosive; // Applies only if explosive is set true. (Mine)
	
	public GameObject explosionPrefab;
	public GameObject ammoPrefab;
	
	public TerrainBlock terrainBlock;
	
	void OnEnable () {
		Game.OnGamePhaseChanged += OnGamePhaseChanged;
	}
	
	void OnDisable () {
		Game.OnGamePhaseChanged -= OnGamePhaseChanged;
	}

	void OnGamePhaseChanged (GamePhase previous, GamePhase current) {
		switch ( current ) {
		case GamePhase.Building:
		case GamePhase.GameOver:
		case GamePhase.Selection:
			if ( IsInvoking ("Fire") )
				CancelInvoke ("Fire");
			break;
		}
	}
	
	
	
	private bool unitPlaced;
	
	public bool UnitPlaced {
		get { return unitPlaced; }
		set {
			unitPlaced = value;
			UnitStart ();
		}
	}
	
	void UnitStart () {
		if ( !triggeredExplosive && explosive )
			Invoke ( "Explode", explosionDelay );
		
		if ( fireProjectile )
			InvokeRepeating ( "Fire", 1, rateOfFire );
	}
	
	public float Health {
		get { return health; }
		set {
			health = value;
			
			if ( health <= 0 )
				Killed();
		}
	}
	
	void Killed () {
		//Debug.Log ("Killed");
		Destroy (gameObject);
	}
	
	void Explode () {
		Instantiate ( explosionPrefab, new Vector3 (transform.position.x, transform.position.y, -5f), Quaternion.identity );
		if ( range > 0 ) {
			var terrainBlocks = TerrainManager.GetSquareTerrain ( terrainBlock, range );
			foreach ( TerrainBlock terrain in terrainBlocks )
				terrain.KillAllEnemies();
		}
		Destroy (gameObject);
	}
	
	RaycastHit rayHit;
	public LayerMask layerMask = ~ ( (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Unit")) | (1 << LayerMask.NameToLayer("Ammo")) );
	bool IsEnemyInLane () {
		 if ( Physics.Raycast (transform.position, Vector3.right, out rayHit, Mathf.Infinity, layerMask) ) {
			//Debug.Log ("Detected " + rayHit.collider );
			return true;
		}
		return false;
	}
	
	void Fire () {
		if (!IsEnemyInLane ())
			return;
		
		GameObject ammoGO = Instantiate ( ammoPrefab, transform.position, Quaternion.identity ) as GameObject;
		Ammo ammo = ammoGO.GetComponent<Ammo>();
		ammo.Damage = damage;
	}
	
	#region Recharge Logic
	public float slotRechargeTime = 3;
	
	private bool recharged;
	public bool Recharged {
		get { return recharged; }
		private set {
			recharged = value;
		}
	}
	
	private float lastUsedTime;
	public float LastUsedTime {
		private get { return lastUsedTime; }
		set {
			lastUsedTime = value;	
		}
	}
	
	private float timeLeftToRecharge;
	
	public bool CheckRecharged () {
		if ( Time.time > LastUsedTime + slotRechargeTime ) {
			Recharged = true;
			return true;
		}
		else {
			Recharged = false;
			timeLeftToRecharge = (LastUsedTime + slotRechargeTime) - Time.time;
			return false;
		}
		return false;
	}
	
	public float GetRechargedValue () {
		return timeLeftToRecharge / slotRechargeTime;
	}
	
	public void ResetRecharge () {
		LastUsedTime = -1;
		timeLeftToRecharge = 0;
	}
	#endregion
	
	void HitDamage (Object damageSource) {
		Enemy enemy = (Enemy) damageSource;
		
		if ( explosive ) {
			Destroy (enemy.gameObject);
			Explode ();
		}
		else
			Health -= enemy.damage;
	}
	
	
}
