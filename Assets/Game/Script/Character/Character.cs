using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttack;

public class Character : MonoBehaviour
{
    [Header("Character data")]
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private PlayerCam cameraManager;
    public CharacterData charData;

    //Events
    public delegate void CharacterHealthModified();
    public static CharacterHealthModified onHealthChange;

    public delegate void CharacterDied();
    public static CharacterDied onCharacterDied;

    [Header("Character death")]
    [SerializeField] private GameObject GameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        charData.currentHealth = charData.maxHealth;
    }

    //TODO : Remove this, it's a debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - charData.defense;
        if (totalDamage > 0) 
        {
            charData.currentHealth -= totalDamage;
        } else
        {
            charData.currentHealth -= 1;
        }
        

        if(onHealthChange != null)
        {
            onHealthChange.Invoke();
        }

        if (charData.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float heal) 
    {
        charData.currentHealth += heal;
        if (charData.currentHealth > charData.maxHealth)
            charData.currentHealth = charData.maxHealth;


        if (onHealthChange != null)
        {
            onHealthChange.Invoke();
        }
    }

    private void Die()
    {
        //TODO : Make the character die
        print(gameObject.name + " is dead");

        weaponHolder.SetActive(false);
        gameObject.GetComponent<PlayerAttack>().enabled = false;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        cameraManager.enabled = false;

        Invoke("CharacterDiedInvoke", 2f);
    }

    private void CharacterDiedInvoke()
    {
        if(onCharacterDied != null)
            onCharacterDied.Invoke();

        GameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        onCharacterDied = null;
        onHealthChange = null;
    }
}
