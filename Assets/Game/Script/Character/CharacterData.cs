using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]

public class CharacterData : ScriptableObject
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    [Header("Defence")]
    public float defence;
}
