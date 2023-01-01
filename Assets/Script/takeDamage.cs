using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class takeDamage : NetworkBehaviour
{
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip criticalSound;
    public AudioClip offlineSound;
    
    public enum collisionType { head , body}
    public collisionType bulletcollision;

    public PlayerManager health;
    public PlayerMovement controller;
    public float multiplier = 1f;
    public void TakeDamage(float amount)
    {
        audioSource.PlayOneShot(damageSound, 0.2f);
        if (bulletcollision == collisionType.head)
        {
            multiplier = 2f;
        }
        else if (bulletcollision == collisionType.body)
        {
            multiplier = 1f;
        }

        Debug.Log(amount + "has been registered");

        health.Health.Value -= amount * multiplier;
        if (health.Health.Value <= 50f)
        {
            audioSource.PlayOneShot(criticalSound);
        }
            if (health.Health.Value <= 0f)
            {
            audioSource.PlayOneShot(offlineSound);
            controller.Die(); 
            }
    }
}
