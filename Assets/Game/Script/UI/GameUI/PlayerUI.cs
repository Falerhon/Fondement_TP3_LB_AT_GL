using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerGunHolder;
    public GameObject playerCharacter;
    [SerializeField] private TextMeshProUGUI ammoTextBox;
    [SerializeField] private TextMeshProUGUI healthTextBox;

    void Start()
    {
        PlayerAttack.onAttack += RefreshBullets;
        Gun.onReloadDone += RefreshBullets;
        PlayerWeaponHolder.onWeaponChange += RefreshBullets;
        Character.onHealthChange += RefreshHealth;

        RefreshHealth();
        RefreshBullets();
    }

    void RefreshBullets()
    {
        if(playerGunHolder.GetComponent<PlayerWeaponHolder>().currentGun == null)
        {
            ammoTextBox.text = "No weapon";
            return;
        }    
        GunData data = playerGunHolder.GetComponent<PlayerWeaponHolder>().currentGun.gunData;
        string ammoText = "Ammo : ";
        ammoText += data.CurrentAmmo + "/" + data.MaxAmmo;
        ammoTextBox.text = ammoText;
    }

    void RefreshHealth()
    {
        CharacterData character = playerCharacter.GetComponent<Character>().charData;
        healthTextBox.text = character.currentHealth + "/" + character.maxHealth;
    }
}
