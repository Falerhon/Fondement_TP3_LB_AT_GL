using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationAmmo : MonoBehaviour
{
    float explosionDelay;
    float delayToDestroySelf;

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
