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
    public void authRegister(string name, string email, string pass)
    {
        authenticateRegistration(name, email, pass);
    }

    public void authLogin(string email, string pass)
    {
        authenticateLogin(email, pass);
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
            updateUserProfile(newUser, name, "https://i.ya-webdesign.com/images/blue-slime-png-8.png");
            sendEmailVerification(newUser);
        });
    }

    private void sendEmailVerification(Firebase.Auth.FirebaseUser user)
    { 
        user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendEmailVerification cancelled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendEmailVerification error: " + task.Exception);
                return;
            }

            Debug.Log("SendEmailVerification to: " + user.Email);
            SceneManager.LoadScene(0);
        });
    }

    private void authenticateLogin(string email, string pass)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.DefaultInstance);
        auth.SignInWithEmailAndPasswordAsync(email, pass).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser currentUser = task.Result;
            if (currentUser.IsEmailVerified)
            {
                Debug.Log("Email is verified");
                registerThisUser = new User(currentUser.DisplayName, currentUser.UserId, currentUser.Email, pass);
                CreateUserFB userToDB = new CreateUserFB();
                userToDB.pushUserJson(registerThisUser);
                SceneManager.LoadScene(2);
            }
            else
            {
                Debug.Log("Email not verified");
            }

        });
    }

    public void updateUserProfile(Firebase.Auth.FirebaseUser user, string name, string photoUrl)
    {
        if (!photoUrl.Contains("http"))
        {
            Debug.Log("Profile Picture is not an http(s) link");
            return;
        }
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = name,
                PhotoUrl = new System.Uri(photoUrl),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
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

                Debug.Log("User profile updated successfully.");
            });
        }

    }

    public void getUserProfile()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("fauled");
            }
            if (task.IsCanceled)
            {
                Debug.Log("canceled");
            }

            DataSnapshot snapshot = task.Result;
            //Debug.Log(snapshot.Child("user"));
            string data = snapshot.GetRawJsonValue().ToString();
            Debug.Log(data);

        });
    }

    
}
