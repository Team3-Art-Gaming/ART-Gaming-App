using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class testRegHandler : MonoBehaviour
{
    public InputField email;
    public InputField username;
    public InputField password0;
    public InputField password1;

    public void registerHandler()
    {
        if (checkInputs())
        {
            authReg();
            Debug.Log("check thread here");
            
        }
    }

    async Task authReg()
    {
        //Debug.Log("Firebase Auth here");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.DefaultInstance);
        await auth.CreateUserWithEmailAndPasswordAsync(email.text, password0.text).ContinueWith(task =>
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
            Debug.LogFormat("User registered successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            sceneLoader(0);
        });
    }

    private bool checkInputs()
    {
        if (checkEmail() && checkUsername() && checkPasswords())
        {
            Debug.Log("Registration Successful part0 init");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void sceneLoader(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private bool checkEmail()
    {
        if (!string.IsNullOrEmpty(email.text))
        {
            return true;
        }
        else
        {
            Debug.Log("email field is empty!");
            return false;
        }
    }

    private bool checkUsername()
    {
        if (!string.IsNullOrEmpty(username.text))
        {
            return true;
        }
        else
        {
            Debug.Log("username field is empty!");
            return false;
        }
    }

    private bool checkPasswords()
    {
        if (!string.IsNullOrEmpty(password0.text))
        {
            if (!string.IsNullOrEmpty(password1.text) && password0.text == password1.text)
            {
                return true;
            }
            else
            {
                Debug.Log("passwords do not match or empty field");
                return false;
            }
        }
        else
        {
            Debug.Log("password field is empty!");
            return false;
        }
    }
}
