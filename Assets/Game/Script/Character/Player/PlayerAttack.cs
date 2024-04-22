using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void PlayerAttackEvent();
    public delegate void PlayerReloadEvent();
    public static PlayerAttackEvent onAttack;
    public static PlayerAttackEvent onReload;
    [SerializeField] private KeyCode attackInput;
    [SerializeField] private KeyCode reloadInput;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(attackInput))
        {
            if(onAttack != null)
            {
                onAttack.Invoke();
            }
        }

        if(Input.GetKeyDown(reloadInput))
        {
            if(onReload != null) { onReload.Invoke(); }
        }
    }

    private void OnDestroy()
    {
        onAttack = null;
        onReload = null;
    }
}
