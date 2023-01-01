using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string idPrefix = "Player";
    private static Dictionary<string, PlayerManager> users = new Dictionary<string, PlayerManager>();

    public static void CreateUniqueUser(string netID, PlayerManager user)
    {
        string userID = idPrefix + netID;
        users.Add(userID, user);
        user.transform.name = userID;
    }

    public static void DeleteUniqueUser(string userID)
    {
        users.Remove(userID);
    }

    public static PlayerManager GetUser (string userID)
    {
        return users[userID];
    }
}
