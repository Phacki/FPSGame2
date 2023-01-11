using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
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
    public Image HealthFill;
    public TextMeshProUGUI CurrentHealth;

    public int killCount = 0;
    public TextMeshProUGUI txtTags;

    public NetworkVariable<float> Health = new NetworkVariable<float>();
    public float MaxHealth = 150f;

    [SerializeField]
    private takeDamage TakeDamage;
    //[SerializeField]
    //private RoundScript roundScript;
    private bool firstSetUp = true;
    public NetworkVariable<int> playerTeam = new NetworkVariable<int>();
    Dictionary<GameObject, Vector3> playerSpawnLocations;

    [ServerRpc(RequireOwnership = false)]
    public void SetUpServerRpc()
    {
        Health.Value = MaxHealth;
        StreamNewPlayerSetUpServerRpc();
        playerSpawnLocations = new Dictionary<GameObject, Vector3>();
        IssueTeamLayer();
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
            //roundScript.SearchPlayerConnection();
            TeamSelectSpawnPoint();
        }

        StartUpServerRpc();
    }


    [ClientRpc]
    public void RegisterDamageClientRpc (float amount)
    {
        Debug.Log(amount + "has been registered");
        TakeDamage.TakeDamageServerRpc(amount);
    }
    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer)
    //        Debug.Log("override solution");
    //    SpawnPoint();
    //}

        [ClientRpc]
        public void DieClientRpc()
        {
            Debug.Log("Client has died");
            //Respawn
            Respawn();
        }
    private void Respawn()
    {
        aR.ammo = 60;
        sMG.ammo = 50;
        snotGun.ammo = 10;
        pistol.ammo = 45;
        stim.stimAmount = 1;
        SpawnPoint(playerTeam.Value);
        SetUpServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void StartUpServerRpc()
    {
        Debug.Log("ServerRpc Entered");
        Health.Value = MaxHealth;
        HealthBar();
    }

    public void AddKill()
    {
        killCount++;
        txtTags.text = killCount + "";
    }

    public void SpawnPoint(int playerTeam)
    {
        // get the current player object
        GameObject currentPlayer = this.gameObject;

        // remove the player from the dictionary if they were already added
        if (playerSpawnLocations.ContainsKey(currentPlayer))
        {
            playerSpawnLocations.Remove(currentPlayer);
        }

        // get the spawn location for the player
        Vector3 spawnLocation = GetSpawnLocation(playerTeam);

        // add the player and their spawn location to the dictionary
        playerSpawnLocations.Add(currentPlayer, spawnLocation);

        // move the player to the spawn location
        currentPlayer.transform.position = spawnLocation;
    }

    Vector3 GetSpawnLocation(int playerTeam)
    {
        // list of spawn locations for team 0
        List<Vector3> team0SpawnLocations = new List<Vector3>()
        {
            new Vector3(55, -15, -15),
            new Vector3(55, -15, -22),
            new Vector3(55, -15, -28),
            new Vector3(55, -15, -34)
        };

        // list of spawn locations for team 1
        List<Vector3> team1SpawnLocations = new List<Vector3>()
        {
            new Vector3(-42, -15, 55),
            new Vector3(-36, -15, 55),
            new Vector3(-29, -15, 55),
            new Vector3(-22, -15, 55)
        };

        // get the list of spawn locations for the player's team
        List<Vector3> spawnLocations = playerTeam == 0 ? team0SpawnLocations : team1SpawnLocations;

        // find the first available spawn location
        foreach (Vector3 spawnLocation in spawnLocations)
        {
            if (!playerSpawnLocations.ContainsValue(spawnLocation))
            {
                return spawnLocation;
            }
        }

        // if all spawn locations are taken, return the first spawn location
        return spawnLocations[0];
    }

    private void TeamSelectSpawnPoint()
    {
        if (Player.name == "Player0")
        {
            Player.transform.position = new Vector3(55, -24, -15);
        }
        if (Player.name == "Player1")
        {
            Player.transform.position = new Vector3(-42, -24, 55);
        }
        if (Player.name == "Player2")
        {
            Player.transform.position = new Vector3(55, -24, -22);
        }
        if (Player.name == "Player3")
        {
            Player.transform.position = new Vector3(-36, -24, 55);
        }
        if (Player.name == "Player4")
        {
            Player.transform.position = new Vector3(55, -24, -28);
        }
        if (Player.name == "Player5")
        {
            Player.transform.position = new Vector3(-29, -24, 55);
        }
        if (Player.name == "Player6")
        {
            Player.transform.position = new Vector3(55, -24, -34);
        }
        if (Player.name == "Player7")
        {
            Player.transform.position = new Vector3(-22, -24, 55);
        }
    }

    void IssueTeamLayer()
    {
        // get the team of the owner
        int ownerTeam = playerTeam.Value;

        // iterate through the list of players
        List<GameObject> allPlayers = new List<GameObject>();
        foreach (GameObject player in allPlayers)
        {
            // get the team of the current player
            int playerTeam = player.GetComponent<PlayerManager>().playerTeam.Value;

            // check if the player is on the same team as the owner
            if (playerTeam == ownerTeam)
            {
                // set the layer of the player to "TeamPlayer"
                player.layer = LayerMask.NameToLayer("TeamPlayer");
            }
        }
    }
    public void HealthBar()
    {
        CurrentHealth.text = Health.Value + "";
        HealthFill.fillAmount = Health.Value / MaxHealth;
    }
}
