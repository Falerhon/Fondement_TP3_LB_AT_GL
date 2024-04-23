using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationAmmo : Projectile
{
	[SerializeField] float explosionDelay;
	[SerializeField] float delayToDestroySelf;
	[SerializeField] float radius;
	[SerializeField] float explosionStrenght;
	[SerializeField] int amountOfFragmentToSpawn;

    [SerializeField] float launchForce;

    public GameObject fragmentToSpawn;
	//Used to disable movement and physics for explosion
	SphereCollider myCollider;
	Rigidbody myRigidbody;
	MeshRenderer myMesh;
	GameObject[] fragments;

    [SerializeField] private LayerMask layermask;

    public override void GiveDirection(Vector3 Direction)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb != null)
			rb.AddForce(Direction * launchForce, ForceMode.Impulse);
	}

	private void Awake()
	{
		myCollider=GetComponent<SphereCollider>();
		myRigidbody=GetComponent<Rigidbody>();	
		myMesh=GetComponent<MeshRenderer>();
		fragments= new GameObject[amountOfFragmentToSpawn];
	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(ExplosionDelay());
    }


	private void OnCollisionEnter(Collision collision)
	{

	//	StopAllCoroutines();
		//Explode();
	}

	void Explode()
	{
		StopAllCoroutines();

		if (myRigidbody != null)
		{
			myRigidbody.velocity = Vector3.zero;
			myRigidbody.useGravity = false;
		}

		if(myCollider != null)
		{
			myCollider.enabled = false;
		}

		if (myMesh != null)
		{
			myMesh.enabled = false;
		}

		

		for (int i = 0; i < amountOfFragmentToSpawn; i++)
		{
			//We only check 10 times so we don't infinite loop
			for (int j = 0; j < 10; j++)
			{
				Vector3 spawnPos = transform.position + Random.insideUnitSphere * myCollider.radius;
				if (!Physics.CheckBox(spawnPos, new Vector3(0.05f, 0.05f, 0.05f), Quaternion.identity, layermask))
				{
					fragments[i] = Instantiate(fragmentToSpawn, spawnPos, Random.rotation, transform);
					break;
				}
			}
		}
		//Start couroutine to delay application of physics
		StartCoroutine(ProjectFragments());

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

	IEnumerator ProjectFragments()
	{
		yield return null;
		for(int i = 0;i < fragments.Length; i++)
		{
			if (fragments[i] != null)
			{
				Rigidbody rb = fragments[i].GetComponent<Rigidbody>();
				if (rb != null)
					rb.AddExplosionForce(explosionStrenght, transform.position, radius);
			}
		}
	}
}
