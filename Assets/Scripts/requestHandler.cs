using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class requestHandler : MonoBehaviour
{
    private User registerThisUser;
    public requestHandler() { }
    public requestHandler(string name, string email, string pass)
    {
        authenticateRegistration(name, email, pass);
    }

    private void authenticateRegistration(string name, string email, string pass)
    {
        //Debug.Log("Firebase Auth here");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.DefaultInstance);
        auth.CreateUserWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPassword cancelled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPassword error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User registered successfully: {0} ({1})", name, newUser.UserId);
            registerThisUser = new User(name, newUser.UserId, email, pass);
            createNewUserProfile(newUser, name);
        });
    }

    private void createNewUserProfile(Firebase.Auth.FirebaseUser user, string name)
    {
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = name,
                PhotoUrl = new System.Uri("https://i.ya-webdesign.com/images/blue-slime-png-8.png"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.LogFormat("User profile updated successfully: {0} ({1})", user.DisplayName, user.UserId);
                registerThisUser.setUsername(user.DisplayName);
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            Debug.Log("Registration failed and no user profile can be created. ");
        }
    }
}
