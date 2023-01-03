using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameScripts : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject Player;
    private GameObject Player1;
    private GameObject Player4;
    public int currentRound = 1;
    public int maxRounds = 7;
    public int TeaPoints = 0;
    public int SeaTeaPoints = 0;

    public TextMeshProUGUI txtTea;
    public TextMeshProUGUI txtSeaTea;

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
        playerManager.SpawnPoint();
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
        //GameObject Player1 = GameObject.Find("Player1");
        //GameObject Player2 = GameObject.Find("Player2");
        //GameObject Player3 = GameObject.Find("Player3");
        //GameObject Player4 = GameObject.Find("Player4");
        //GameObject Player5 = GameObject.Find("Player5");
        //GameObject Player6 = GameObject.Find("Player6");
        //GameObject Player7 = GameObject.Find("Player7");
        //GameObject Player8 = GameObject.Find("Player8");

        //if (Player1.GetComponent<PlayerManager>().IsDead == true) //&& Player3.GetComponent<PlayerManager>().IsDead == true && Player5.GetComponent<PlayerManager>().IsDead == true && Player7.GetComponent<PlayerManager>().IsDead == true
        //{
        //    SeaTeaPoints++;
        //    EndRound();
        //}
        //else if(Player2.GetComponent<PlayerManager>().IsDead == true) //&& Player4.GetComponent<PlayerManager>().IsDead == true && Player6.GetComponent<PlayerManager>().IsDead == true && Player8.GetComponent<PlayerManager>().IsDead == true
        //{
        //    TeaPoints++;
        //    EndRound();
        //}
        Player1 = GameObject.Find("Player1");
        Player4 = GameObject.Find("Player4");

        if (Player1 != null && Player4 != null)
        {
            Debug.Log("Null Passed");
            if (Player4.GetComponent<PlayerManager>().Health.Value <= 0 && Player1.GetComponent<PlayerManager>().Health.Value > 0)
            {
                Debug.Log("Player 4 died");
                //SeaTea wins round! (Player2)
                SeaTeaPoints++;
                EndRound();
            }
            else if (Player1.GetComponent<PlayerManager>().Health.Value <= 0 && Player4.GetComponent<PlayerManager>().Health.Value > 0)
            {
                Debug.Log("Player 1 died");
                //Tea wins round! (Player1)
                TeaPoints++;
                EndRound();
            }
        }
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
