using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameScripts : MonoBehaviour
{
    public PlayerManager playerManager;
    public int currentRound = 1;
    public int maxRounds = 7;
    public int TeaPoints = 0;
    public int SeaTeaPoints = 0;

    public TextMeshProUGUI txtTea;
    public TextMeshProUGUI txtSeaTea;

    public List<GameObject> allPlayers = new List<GameObject>();
    public GameObject spawnwalls;
    // Start is called before the first frame update
    void Start()
    {
        spawnwalls.SetActive(true);
        StartCoroutine(roundClock());
    }

    // Update is called once per frame
    void Update()
    {
        CheckRoundWin();
    }

    void StartRound()
    {
        // Reset game state
        StartCoroutine(roundClock());
        // Reset player positions
        //playerManager.SpawnPoint();
    }

    void EndRound()
    {
        // Check if the maximum number of rounds has been reached
        if (currentRound >= maxRounds)
        {
            // End the game
            if(SeaTeaPoints > TeaPoints)
            {
                //SeaTea wins! (Player2)
            }
            else if(SeaTeaPoints < TeaPoints)
            {
                //Tea wins! (Player2)
            }
        }
        else
        {
            // Increment the round counter
            currentRound++;
            // Start a new round
            StartRound();
        }
    }

    private void CheckRoundWin()
    {
        //if (Player4.GetComponent<PlayerManager>().Health.Value <= 0 && Player1.GetComponent<PlayerManager>().Health.Value > 0)
        //{
        //    Debug.Log("Player 1 died");
        //    //SeaTea wins round! (Player2)
        //    //SeaTeaPoints++;
        //    //EndRound();
        //}
        //else if (Player1.GetComponent<PlayerManager>().Health.Value <= 0 && Player4.GetComponent<PlayerManager>().Health.Value > 0)
        //{
        //    Debug.Log("Player 0 died");
        //    //Tea wins round! (Player1)
        //    TeaPoints++;
        //    EndRound();
        //}
    }

    IEnumerator roundClock()
    {
        yield return new WaitForSeconds(15f);
        spawnwalls.SetActive(false);
        yield return new WaitForSeconds(5f);
        spawnwalls.SetActive(true);
        StopCoroutine(roundClock());
        CheckRoundWin();
    }
}
