using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : Projectile
{
	float explosionDelay;

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
		//if(has tag destroyable) 
			//Destroy
		//IF is Character
		//Give Damage

		
		Destroy(gameObject);
	}


	IEnumerator ExplosionDelay()
	{
		yield return new WaitForSeconds(explosionDelay);
		Explode();
	}
}
