using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVariance : MonoBehaviour
{
	public float strenghtModifier;

	private void OnTriggerStay(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(Vector3.up* -strenghtModifier,ForceMode.Acceleration);
		}
	}
}
