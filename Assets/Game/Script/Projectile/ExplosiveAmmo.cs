using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : MonoBehaviour
{
	float explosionDelay;
	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(ExplosionDelay());
	}

	void Explode()
	{

		
		Destroy(gameObject);
	}


	IEnumerator ExplosionDelay()
	{
		yield return new WaitForSeconds(explosionDelay);
		Explode();
	}
}
