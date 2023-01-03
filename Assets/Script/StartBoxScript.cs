using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StartBoxScript : NetworkBehaviour
{

    [SerializeField]
    private GameObject Player;

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
