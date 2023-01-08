using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class UserScript : NetworkBehaviour
{
    // Declare public string variables for the username and password
    public string userName = "";
    public string passWord = "";
    public TextMeshProUGUI accountName;

    void Start()
    {
        try
        {
            // Declare local string variables for the username and password
            string username = "";
            string password = "";

            // Get the command line arguments as an array of strings
            string[] args = System.Environment.GetCommandLineArgs();

            // Iterate through the array of arguments
            foreach (string arg in args)
            {
                // Check if the argument starts with "--username"
                if (arg.StartsWith("--username"))
                {
                    // Set the username variable to the value after the "=" sign
                    username = arg.Split('=')[1];
                }
                // Check if the argument starts with "--password"
                else if (arg.StartsWith("--password"))
                {
                    // Set the password variable to the value after the "=" sign
                    password = arg.Split('=')[1];
                }
            }

            // Check if both the username and password variables have been set
            if (username != "" && password != "")
            {
                // Set the public variables to the values of the local variables
                userName = username;
                passWord = password;
                accountName.text = username;

                // Use the username and password to log in

            }
            else
            {
                // Print an error message
                Debug.LogError("Username and password not provided!");
            }
        }
        catch
        {

        }
    }
}

