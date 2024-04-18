using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;

    private bool onCooldown = false;

    private void Start()
    {
        gunData.CurrentAmmo = gunData.MaxAmmo;
    }

    public void Equip()
    {
        GetComponent<MeshRenderer>().enabled = true;
        PlayerAttack.onAttack += Fire;
        PlayerAttack.onReload += Reload;
    }

    public void Unequip()
    {
        GetComponent<MeshRenderer>().enabled = false;
        PlayerAttack.onAttack -= Fire;
        PlayerAttack.onReload -= Reload;
    }

    private void Fire()
    {
        if (gunData.CurrentAmmo > 0 && !gunData.Reloading && !onCooldown)
        {
            //Projectile firing
            //Instantiate(gunData.projectile);
            gunData.CurrentAmmo--;
            onCooldown = true;

            print("Shooting with gun : " + gunData.Name);

            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f / (gunData.FireRate / 60));
        print("Cooldown over");
        onCooldown = false;
    }

    public void Reload()
    {
        if(!gunData.Reloading) 
        {
            gunData.Reloading = true;
            StartCoroutine(ReloadTime());
        }
    }

    private IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(gunData.ReloadTime);
        print("Reload over");
        if(gunData.CurrentAmmo > 0 && gunData.UsesMagasine)
        {
            gunData.CurrentAmmo = gunData.MaxAmmo + 1;
        } else
        {
            gunData.CurrentAmmo = gunData.MaxAmmo;
        }
        
        gunData.Reloading = false;
    }
}
