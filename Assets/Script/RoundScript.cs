//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Netcode;
//using TMPro;
//using static UnityEditor.Experimental.GraphView.GraphView;

//public class RoundScript : MonoBehaviour
//{
//    private bool playerzeroConnected = false;
//    private bool playeroneConnected = false;
//    private bool playertwoConnected = false;
//    private bool playerthreeConnected = false;
//    private bool playerfourConnected = false;
//    private bool playerfiveConnected = false;
//    private bool playersixConnected = false;
//    private bool playersevenConnected = false;

//    // Game object for walls which stop player from leaving during round
//    public GameObject spawnwalls;
//    // The maximum number of players that can exist in the game and how many are currently connected
//    public int currentPlayers = 0;
//    public int maxPlayers = 8;
//    // Round Settings
//    public int currentRound = 1;
//    public int maxRounds = 7;
//    // Team Point Ints
//    public int TeaPoints = 0;
//    public int SeaTeaPoints = 0;
//    // Team Point Text
//    public TextMeshProUGUI txtTea;
//    public TextMeshProUGUI txtSeaTea;

//    public void SearchPlayerConnection()
//    {
//        // Iterate through the players
//        for (int i = 0; i < maxPlayers; i++)
//        {
//            // Try to find the player GameObject by name
//            GameObject player = GameObject.Find("Player" + i);

//            // Check if the player GameObject was found
//            if (player != null)
//            {
//                // Set the player connection flag to true
//                switch (i)
//                {
//                    case 0:
//                        playerzeroConnected = true;
//                        break;
//                    case 1:
//                        playeroneConnected = true;
//                        break;
//                    case 2:
//                        playertwoConnected = true;
//                        break;
//                    case 3:
//                        playerthreeConnected = true;
//                        break;
//                    case 4:
//                        playerfourConnected = true;
//                        break;
//                    case 5:
//                        playerfiveConnected = true;
//                        break;
//                    case 6:
//                        playersixConnected = true;
//                        break;
//                    case 7:
//                        playersevenConnected = true;
//                        break;
//                }
//            }
//        }
//    }

// A function to handle the OnDeath event
//void OnPlayerDeath()
//{
//    // Check if all players are dead
//    if (AllPlayersDead())
//    {
//        // If all players are dead, start the new round script
//        StartNewRound();
//    }
//}

//    // A function to check if all players are dead
//    bool AllPlayersDead()
//    {
//        // Iterate through the players
//        for (int i = 0; i < maxPlayers; i++)
//        {
//            // Try to find the player GameObject by name
//            GameObject player = GameObject.Find("Player" + i);

//            // Check if the player GameObject was found
//            if (player != null)
//            {
//                // Check if the player is not dead
//                if (!player.GetComponent<Health>().isDead)
//                {
//                    // If the player is not dead, return false
//                    return false;
//                }
//            }
//        }

//        // If all players are dead, return true
//        return true;
//    }
//    public void StartNewRound()
//    {

//        StartCoroutine(round());
//    }

//    private void CheckRoundWin()
//    {

//    }

//    IEnumerator roundClock()
//    {
//        yield return new WaitForSeconds(15f);
//        spawnwalls.SetActive(false);
//        yield return new WaitForSeconds(5f);
//        spawnwalls.SetActive(true);
//        StopCoroutine(roundClock());
//        CheckRoundWin();
//    }
//}

