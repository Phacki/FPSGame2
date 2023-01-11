using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{

    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField]
    Behaviour[] partsToDisable;
    public GameObject player;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    GameScripts gameScripts;

    private void Start()
    {
        //Disables all Components of other players i.e HUD and Camera
        if (!IsOwner)
        {
            DisableParts();
            IssueRemoteLayer();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        string netID = GetComponent<NetworkObject>().OwnerClientId.ToString();
        PlayerManager user = GetComponent<PlayerManager>();
        GameManager.CreateUniqueUser(netID, user);
        gameScripts.allPlayers.Add(player);
        GetComponent<PlayerManager>().SetUpServerRpc();
    }

    void DisableParts()
    {
        for (int i = 0; i < partsToDisable.Length; i++)
        {
            partsToDisable[i].enabled = false;
        }
    }

    void IssueRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        GameManager.DeleteUniqueUser(transform.name);
    }
}
