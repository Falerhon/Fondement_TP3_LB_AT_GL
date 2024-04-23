using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidArea : MonoBehaviour
{
	[SerializeField] float baseDamage;

	float elapsedTime = 0;
    // Start is called before the first frame update


	private void OnTriggerStay(Collider other)
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime >0.3f)
		{
			elapsedTime = elapsedTime - 0.3f;
			Character character = other.gameObject.GetComponent<Character>();
			AIController ai = other.gameObject.GetComponent<AIController>();

			if (character != null)
			{
				float damageMultiplier = 1; //+1 si grounded
				PlayerMovement player = character.GetComponent<PlayerMovement>();
				if (player.IsGrounded())
					damageMultiplier++;

				character.TakeDamage(baseDamage * damageMultiplier);
			}
			else if(ai != null)
            {
				float damageMultiplier = 0.5f;
				ai.TakeDamage(baseDamage * damageMultiplier);
			}
		}
	}
}
