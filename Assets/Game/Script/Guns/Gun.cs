using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerAttack;

public class Gun : MonoBehaviour
{
    public GunData gunData;

    public delegate void GunReloadDoneEvent();
    public static GunReloadDoneEvent onReloadDone;

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

            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f / (gunData.FireRate / 60));
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
        if(gunData.CurrentAmmo > 0 && gunData.UsesMagasine)
        {
            gunData.CurrentAmmo = gunData.MaxAmmo + 1;
        } else
        {
            gunData.CurrentAmmo = gunData.MaxAmmo;
        }
        
        if(onReloadDone != null)
        {
            onReloadDone.Invoke();
        }

        gunData.Reloading = false;
    }

    private void OnDestroy()
    {
        onReloadDone = null;
    }
}
