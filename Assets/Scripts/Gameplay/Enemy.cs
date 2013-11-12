using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	
	public float health = 10;
	public float speed = 1;
	public float damage = 10;
	
	//public bool moving = true;
	
	public List <TerrainBlock> terrainBlocks;
	
	public float Health {
		get { return health; }
		set {
			health = value;
			
			if ( health <= 0 )
				Kill();
		}
	}
	
	void Start () {
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
	}
	
	void Update () {
		if ( Game.GamePhase != GamePhase.Defending ) {
			rigidbody.isKinematic = true;
			return;
		}
		transform.Translate ( Vector3.left * Time.deltaTime * speed );
	}
	
	void HitDamage (Object damageSource) {
		Ammo ammo = (Ammo) damageSource;
		Health -= ammo.Damage;
	}
	
	public void Kill () {
		foreach ( TerrainBlock terrainBlock in terrainBlocks )
			terrainBlock.RemoveEnemyFromTerrain ( this );
		
		Destroy (gameObject);
	}
	
	public void Kill ( float delay ) {
		Invoke ( "Kill", delay );	
	}
	
	float lastDamagedTime;
	float damageInterval = 1;
	
	void OnTriggerEnter ( Collider other ) {
		if ( other.gameObject.layer == LayerMask.NameToLayer ("Terrain" ) ) {
			terrainBlocks.Add (other.GetComponent<TerrainBlock>());
			other.GetComponent<TerrainBlock>().AddEnemyToTerrain (this);	
		}
		else if ( other.gameObject.layer == LayerMask.NameToLayer ("GameOver") ){
			Game.GamePhase = GamePhase.GameOver;	
		}
	}
	
	void OnTriggerExit ( Collider other ) {
		if ( other.gameObject.layer == LayerMask.NameToLayer ("Terrain" ) ) {
			terrainBlocks.Remove (other.GetComponent<TerrainBlock>());
			other.GetComponent<TerrainBlock>().RemoveEnemyFromTerrain (this);	
		}
	}
	
	void OnCollisionStay (Collision collision) {
		if ( Time.time > lastDamagedTime + damageInterval) {
			collision.gameObject.SendMessage ( "HitDamage", this, SendMessageOptions.RequireReceiver );
			lastDamagedTime = Time.time;
		}
	}
	
}
