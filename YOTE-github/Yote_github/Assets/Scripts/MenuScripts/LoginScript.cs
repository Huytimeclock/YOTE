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

    public GameObject LoginSection;

    private string email;
    private string password;

    private bool isLogin = false;

    private string usernameNow;
    private string UIDNow;

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
            await GetUsername(user.UserId);
            UIDNow = user.UserId;
            LoginSection.SetActive(false);

            isLogin = true;
            Debug.Log("islogin =: " + isLogin);
            Debug.Log("username la: " + usernameNow);
            Debug.Log("UID la: " + UIDNow);
        }
        catch (FirebaseException exception)
        {
            // Handle authentication errors
            Debug.LogError("Login failed: " + exception.Message);
        }
    }

    private async Task GetUsername(string uid)
    {
        // Get a reference to the user's data in the database
        var userRef = databaseRef.Child("users").Child(uid).Child("username");

        // Retrieve the username value from the database
        var dataSnapshot = await userRef.GetValueAsync();

        // Check if the username exists in the database
        if (dataSnapshot.Exists)
        {
            // Get the username value
            usernameNow = dataSnapshot.Value.ToString();

            // Set the username text in the UI
            usernameText.text = usernameNow;
        }
        else
        {
            Debug.LogWarning("Username not found in the database");
        }
    }

    public string getUsername()
    {
        return usernameNow;
    }
    public string getUID()
    {
        return UIDNow;
    }
    public bool getisLogin()
    { return isLogin; }
}
