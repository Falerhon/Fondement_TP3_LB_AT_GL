using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalAmmo : MonoBehaviour
{
	float destructionDelay;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(DestructionDelay());
	}

	private void OnCollisionEnter(Collision collision)
	{
		
			StopAllCoroutines();
			//if has HP
			//Damage em	
	}

	private void OnCollisionExit(Collision collision)
	{
		Destroy(gameObject);
	}

	IEnumerator DestructionDelay()
	{
		yield return new WaitForSeconds(destructionDelay);
		Destroy(gameObject);
	}
}
