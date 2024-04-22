using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationAmmo : Projectile
{
    float explosionDelay;
    float delayToDestroySelf;

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

    
	private void OnTriggerEnter(Collider other)
	{
		Explode();
	}
	void Explode()
	{
		StopAllCoroutines();
		//Play effect

		//GetList of NormalAmmo

		//Delay before destroy self
		StartCoroutine(destroySelf());
	}

	IEnumerator destroySelf()
	{
		yield return new WaitForSeconds(delayToDestroySelf);
		Destroy(gameObject);
	}

	IEnumerator ExplosionDelay()
	{
		yield return new WaitForSeconds(explosionDelay);
		Explode();
	}

	
}
