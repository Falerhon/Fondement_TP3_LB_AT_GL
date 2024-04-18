using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapons/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    [Header("Gun attack data")]
    public float Damage;
    public float FireRate;
    [Header("Ammo data")]
    public float CurrentAmmo;
    public float MaxAmmo;
    public GameObject Projectile;
    public bool UsesMagasine;
    [Header("Reload data")]
    public float ReloadTime;
    public bool Reloading;
    
}
