using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeFragment : MonoBehaviour
{
	[SerializeField] float baseDamage;
	private void OnTriggerEnter(Collider other)
	{
		Character character = other.GetComponent<Character>();
		if(character != null)
		{
			character.TakeDamage(baseDamage);
		}
		
		if(other.gameObject.CompareTag("Destructible"))
		{
			Destroy(other.gameObject);
		}
	}
}
