using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
using System.Threading;

public class requestHandler : MonoBehaviour
{
    private User registerThisUser;
    private CreateUserFB userToDB;
    private GameObject popUp;
    private int counter;

    public void authRegister(string name, string email, string pass)
    {
        popUp = GameObject.Find("Pop-Up(Clone)");
        requestListOfNames(name, email, pass);
    }

    public void authLogin(string email, string pass)
    {
        popUp = GameObject.Find("Pop-Up(Clone)");
        preLoginRequest(email, pass);
    }

    private void authenticateRegistration(string name, string email, string pass)
    {
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
                popUp.SendMessage("activatePopUp", "Error! " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User registered successfully: {0} ({1})", name, newUser.UserId);
            registerThisUser = ScriptableObject.CreateInstance("User") as User;
            registerThisUser.init(name, email, newUser.UserId);
            updateUserProfile(newUser, name, "https://i.ya-webdesign.com/images/blue-slime-png-8.png");
            sendEmailVerification(newUser, name);
        });
    }

    private void requestListOfNames(string name, string email, string pass)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.GetReference("users");
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error");
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child(name).Value == null)
                {
                    authenticateRegistration(name, email, pass);
                }
                else
                {
                    Debug.Log("Username already taken!");
                    popUp.SendMessage("activatePopUp", "Username already taken!");
                }
            }
            else
            {
                Debug.Log("Error: " + task.Exception);
                return;
            }
        });
    }

    private void sendEmailVerification(Firebase.Auth.FirebaseUser user, string name)
    { 
        user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendEmailVerification cancelled.");
                popUp.SendMessage("activatePopUp", "Error! " + task.Exception);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendEmailVerification error: " + task.Exception);
                popUp.SendMessage("activatePopUp", "Error! " + task.Exception);
                return;
            }

            Debug.Log("SendEmailVerification to: " + user.Email);
            popUp.SendMessage("setSceneChange", (int) 0);
            popUp.SendMessage("setOkButton","ChangeScene");
            popUp.SendMessage("activatePopUp","Register Success! Check Email to Verify");
            addToUserDB(registerThisUser);
        });
    }

    private void authenticateLogin(string name, string email, string pass)
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
                popUp.SendMessage("activatePopUp", "Error! " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser currentUser = task.Result;
            if (currentUser.IsEmailVerified)
            {
                Debug.Log("Email is verified!!");

                PlayerPrefs.SetString("Username", name);
                PlayerPrefs.Save();
                PushData("users/" + name + "/Status/", "Active");
                Debug.Log("Saved " + name + " to PlayerPrefs");
                DownloadMaps();
                SceneManager.LoadScene(2);
            }
            else
            {
                Debug.Log("Email not verified");
                popUp.SendMessage("activatePopUp", "Email not verified, please check and verify email!");
            }

        });
    }

    public void DownloadMaps()
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string name = PlayerPrefs.GetString("Username");
        string MapName;
        string MapString;

        FirebaseDatabase.DefaultInstance.GetReference("users/" + name + "/CreatedMaps/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    MapName = child.Key.ToString();
                    MapString = child.Key.ToString();

                    PlayerPrefs.SetString(MapName, MapString);
                    PlayerPrefs.Save();
                }
                return;
            }
            else
            {
                return;
            }
        });
        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return;

    }

    public List<Friends> GetFriendsList()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        List<Friends> friendslist = new List<Friends>();

        FirebaseDatabase.DefaultInstance.GetReference("users/" + currentUser + "/friends/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                return null;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    friendslist.Add(new Friends(child.Key.ToString(), child.Value.ToString()));
                }
                return friendslist;
            }
            else
            {
                return null;
            }
        });
        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return friendslist;
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
    }

    private void preLoginRequest(string name, string password)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.GetReference("users");
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child(name).Value == null)
                {
                    Debug.Log("No Valid Username");
                    popUp.SendMessage("activatePopUp", "There exists no account by that username!");
                }
                else
                {
                    authenticateLogin(name, snapshot.Child(name).Child("email").Value.ToString(), password);
                }
            }
            else
            {
                Debug.Log("Error: " + task.Exception);
                return;
            }
        });
    }

    private void addToUserDB(User user)
    {
        Debug.Log("Writing User info to database");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string jsonDataBlock = JsonUtility.ToJson(user);
        Debug.Log("user: "+user.FriendsList);
        DBreference.Child("users").Child(user.getUsername()).SetRawJsonValueAsync(jsonDataBlock);
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
            string data = snapshot.GetRawJsonValue().ToString();
            Debug.Log(data);

        });
    }

    
}
