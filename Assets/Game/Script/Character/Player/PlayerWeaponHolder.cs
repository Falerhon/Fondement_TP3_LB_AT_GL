using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerAttack;

public class PlayerWeaponHolder : MonoBehaviour
{
    [SerializeField] private List<Gun> guns = new List<Gun>();
    [SerializeField] private List<KeyCode> inputs = new List<KeyCode>();
    public Gun currentGun;
    
    private Dictionary<KeyCode, Gun> gunInventory = new Dictionary<KeyCode, Gun>();

    public delegate void ChangeWeaponEvent();
    public static ChangeWeaponEvent onWeaponChange;

    private void Start()
    {
        if(guns.Count == inputs.Count)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                gunInventory.Add(inputs[i], guns[i]);
            }
        } else { print("Not the same amount of guns and inputs"); }
    }

    private void Update()
    {
        foreach (KeyCode key in gunInventory.Keys) 
        { 
            if(Input.GetKeyDown(key))
            {
                if(currentGun != null)
                    currentGun.Unequip();

                currentGun = gunInventory[key];
                currentGun.Equip();

                if(onWeaponChange != null)
                {
                    onWeaponChange.Invoke();
                }
                
            }
        }
    }

    private void OnDestroy()
    {
        onWeaponChange = null;
    }
}
