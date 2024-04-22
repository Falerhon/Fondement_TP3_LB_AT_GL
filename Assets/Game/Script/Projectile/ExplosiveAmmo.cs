using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveAmmo : Projectile
{
	public float explosionDelay;
	public float radius;
	public float explosionStrenght;

	public override void GiveDirection(Vector3 Direction)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
			rb.AddForce(Vector3.forward * 60, ForceMode.Acceleration);
	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(ExplosionDelay());
	}

	void Explode()
	{
		//Check in radius
		Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
		foreach (Collider collider in colliders)
		{
			Rigidbody rb=collider.GetComponent<Rigidbody>();
			if (rb != null)
				rb.AddExplosionForce(explosionStrenght, transform.position, radius);


			//Check if can take damage the call it


			Debug.Log("detected");
			if (collider.gameObject.CompareTag("Destructible"))
				Destroy(collider.gameObject);

		}
		
		Destroy(gameObject);
	}


	IEnumerator ExplosionDelay()
	{
		yield return new WaitForSeconds(explosionDelay);
		Explode();
	}
}
