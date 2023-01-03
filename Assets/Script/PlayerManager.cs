using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerManager : NetworkBehaviour
{
    public GunScript aR;
    public GunScript sMG;
    public GunScript snotGun;
    public GunScript pistol;
    public StimScript stim;
    public GameObject Player;
    public PlayerMovement healthBar;
    public int killCount = 0;
    public TextMeshProUGUI txtTags;

    public NetworkVariable<float> Health = new NetworkVariable<float>();
    public float MaxHealth = 150f;

    [SerializeField]
    private takeDamage TakeDamage;
    private bool firstSetUp = true;

    public void SetUp()
    {
        StreamNewPlayerSetUpServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void StreamNewPlayerSetUpServerRpc()
    {
        SetPlayerUpOnAllClientRpc(); 
    }

    [ClientRpc]
    private void SetPlayerUpOnAllClientRpc()
    {
        if (firstSetUp)
        {
            firstSetUp = false;
            SpawnPoint();
        }

        StartUp();
    }


    [ClientRpc]
    public void RegisterDamageClientRpc (float amount)
    {
        Debug.Log(amount + "has been registered");
        TakeDamage.TakeDamage(amount);
    }
    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer)
    //        Debug.Log("override solution");
    //    SpawnPoint();
    //}

    public void Die()
    {
        //Respawn
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        aR.ammo = 60;
        sMG.ammo = 50;
        snotGun.ammo = 10;
        pistol.ammo = 45;
        stim.stimAmount = 1;
        yield return new WaitForSeconds(0.1f);
        SpawnPoint();
        yield return new WaitForSeconds(0.1f);
        SetUp();
    }

    public void StartUp()
    {
        Debug.Log("ServerRpc Entered");
        Health.Value = MaxHealth;
        healthBar.HealthBar();
    }

    public void AddKill()
    {
        killCount++;
        txtTags.text = killCount + "";
    }

    public void SpawnPoint()
    {
        if (Player.name == "Player1")
        {
            Player.transform.position = new Vector3(55, -15, -15);
        }
        if (Player.name == "Player2")
        {
            Player.transform.position = new Vector3(-42, -15, 55);
        }
        if (Player.name == "Player3")
        {
            Player.transform.position = new Vector3(55, -15, -22);
        }
        if (Player.name == "Player4")
        {
            Player.transform.position = new Vector3(-36, -15, 55);
        }
        if (Player.name == "Player5")
        {
            Player.transform.position = new Vector3(55, -15, -28);
        }
        if (Player.name == "Player6")
        {
            Player.transform.position = new Vector3(-29, -15, 55);
        }
        if (Player.name == "Player7")
        {
            Player.transform.position = new Vector3(55, -15, -34);
        }
        if (Player.name == "Player8")
        {
            Player.transform.position = new Vector3(-22, -15, 55);
        }
    }
}
