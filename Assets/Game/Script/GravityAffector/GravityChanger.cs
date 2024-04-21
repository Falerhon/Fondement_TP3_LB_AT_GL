using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    public float strenght;
	float radius;

	private void Awake()
	{
		SphereCollider collider= GetComponent<SphereCollider>();
		if(collider != null )
			radius = collider.radius;
	}

	private void OnTriggerStay(Collider other)
	{
		
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if (rb != null)
		{
			Debug.Log("has rigidBody");
			rb.AddExplosionForce(strenght, transform.position, radius);
		}
	}
}
