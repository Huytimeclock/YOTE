using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using TMPro;

public class LoginScript : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    private string email;
    private string password;

    private FirebaseAuth auth;

    private void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;
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
        }
        catch (FirebaseException exception)
        {
            // Handle authentication errors
            Debug.LogError("Login failed: " + exception.Message);
        }
    }
}
