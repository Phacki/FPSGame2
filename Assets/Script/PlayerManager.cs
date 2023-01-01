using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    public NetworkVariable<float> Health = new NetworkVariable<float>();
    public float MaxHealth = 150f;

    takeDamage TakeDamage;

    private void Awake()
    {
        Health.Value = MaxHealth;
    }

    public void RegisterDamage (float amount)
    {
        Debug.Log(amount + "has been registered");
        TakeDamage.TakeDamage(amount);
    }
}
