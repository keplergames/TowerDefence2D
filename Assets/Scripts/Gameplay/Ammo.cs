using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	
	public GameObject explosion;
	public float speed = 5;
	
	private float damage;
	
	public float Damage {
		get { return damage; }
		set {
			damage = value;	
		}
	}
	
	void Start () {
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
	}
	
	void Update () {
		transform.Translate ( -Vector3.left * Time.deltaTime * speed );
	}
	
	/*
	void OnTriggerEnter(Collider other) {
        Instantiate ( explosion, transform.position, Quaternion.identity );
		Destroy ( gameObject );
		other.gameObject.SendMessage ( "HitDamage", this, SendMessageOptions.RequireReceiver );
    }*/
	
	void OnCollisionEnter(Collision collision) {
		Instantiate ( explosion, transform.position, Quaternion.identity );
		collision.gameObject.SendMessage ( "HitDamage", this, SendMessageOptions.RequireReceiver );
		Destroy ( gameObject );
	}
	
	void OnBecameInvisible() {
        Destroy ( gameObject );
    }
	
}
