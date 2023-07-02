using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using TMPro;

public class LoginScript : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI usernameText;

    private string email;
    private string password;

    private FirebaseAuth auth;
    private DatabaseReference databaseRef;

    private void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;

        // Get a reference to the database root
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void Login()
    {
        // Get the email and password entered by the user
        email = emailInputField.text;
        password = passwordInputField.text;

        try
        {
            // Sign in with email and password
            var userCredential = await auth.SignInWithEmailAndPasswordAsync(email, password);

            // User is successfully signed in
            FirebaseUser user = userCredential.User;
            Debug.Log("User signed in successfully: " + user.DisplayName);

            // Retrieve the username from the database based on the user's UID
            GetUsername(user.UserId);
            Debug.Log(user.UserId);
        }
        catch (FirebaseException exception)
        {
            // Handle authentication errors
            Debug.LogError("Login failed: " + exception.Message);
        }
    }

    private async void GetUsername(string uid)
    {
        // Get a reference to the user's data in the database
        var userRef = databaseRef.Child("users").Child(uid).Child("username");

        // Retrieve the username value from the database
        var dataSnapshot = await userRef.GetValueAsync();

        // Check if the username exists in the database
        if (dataSnapshot.Exists)
        {
            // Get the username value
            string username = dataSnapshot.Value.ToString();

            // Set the username text in the UI
            usernameText.text = username;
        }
        else
        {
            Debug.LogWarning("Username not found in the database");
        }
    }
}
