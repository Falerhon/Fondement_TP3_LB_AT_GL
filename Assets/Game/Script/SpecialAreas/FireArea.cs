using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
	[SerializeField] float MaxDamage;
	float elapsedTime = 0;
	SphereCollider colliderTrigger;

	private void Awake()
	{
		colliderTrigger=GetComponent<SphereCollider>();
	}

	private void OnTriggerStay(Collider other)
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > 0.3f)
		{
			elapsedTime = elapsedTime - 0.3f;
			Character character = other.gameObject.GetComponent<Character>();
			if (character != null)
			{
				float damageMultiplier = 1.1f - Mathf.InverseLerp(0, colliderTrigger.radius, Vector3.Distance(other.gameObject.transform.position, transform.position));
				character.TakeDamage(MaxDamage * damageMultiplier);
			}
		}
	}
}
