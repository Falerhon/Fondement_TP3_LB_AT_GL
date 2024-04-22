using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalAmmo : Projectile
{
	[SerializeField] float destructionDelay;
	[SerializeField] float launchForce;
	[SerializeField] float baseDamage;
	// Start is called before the first frame update

	public override void GiveDirection(Vector3 Direction)
	{
		Debug.Log(Direction);
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
			rb.AddForce(Direction * launchForce, ForceMode.Impulse);
	}

	void Start()
	{
		StartCoroutine(DestructionDelay());
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		
			StopAllCoroutines();
		Character character = collision.gameObject.GetComponent<Character>();
		if (character != null)
		{
			character.TakeDamage(baseDamage);
		}
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
