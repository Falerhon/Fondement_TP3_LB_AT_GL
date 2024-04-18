using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWeaponHolder : MonoBehaviour
{
    [SerializeField] private List<Gun> guns = new List<Gun>();
    [SerializeField] private List<KeyCode> inputs = new List<KeyCode>();
    [SerializeField] private Gun currentGun;
    
    private Dictionary<KeyCode, Gun> gunInventory = new Dictionary<KeyCode, Gun>();


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
            }
        }
    }
}
