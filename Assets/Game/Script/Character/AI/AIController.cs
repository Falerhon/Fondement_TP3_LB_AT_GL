using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public CharacterData charData;
    public float wanderRadius;
    private NavMeshAgent agent;
    private Vector3 targetPos;

    void Start()
    {
        charData.currentHealth = charData.maxHealth;
        targetPos = ChangeTarget();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 1.5f)
            targetPos = ChangeTarget();
        agent.destination = targetPos;

    }

    private Vector3 ChangeTarget()
    {
        NavMeshHit navHit;

        NavMesh.SamplePosition(((Random.insideUnitSphere * wanderRadius) + transform.position), out navHit, 1000000, -1);
        Vector3 currentDestination = new Vector3();
        if (currentDestination != navHit.position)
            currentDestination = navHit.position;

        return currentDestination;
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - charData.defense;
        if (totalDamage > 0)
        {
            charData.currentHealth -= totalDamage;
        }
        else
        {
            charData.currentHealth -= 1;
        }

        if (charData.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //TODO : Make the character die
        print(gameObject.name + " is dead");

        Destroy(gameObject, 1f);
    }
}
