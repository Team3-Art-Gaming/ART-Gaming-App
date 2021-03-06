﻿using System.Collections;
using System.Collections.Generic;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading;
//using System.Diagnostics;

public class CreateUserFB : MonoBehaviour
{
    public InputField enterEmail;
    public InputField enterUsername;
    public InputField enterPass;
    public InputField reEnterPass;
    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private Friend_List flScript;

    //Added a public function that pushes the class User as a Json into database.
    public CreateUserFB() { }

    public async Task<string> GetDataAsync(string path)
    {
        //string a = await Task.Run(() => GetData(path));
        var t = await Task.FromResult<string>(GetData(path));

        return t.ToString();
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
        //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("blip").SetValueAsync(data);
    }

    public void DeleteData(string path)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).RemoveValueAsync();
    }

    public void sendFriendRequests(string FriendName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string MyName = PlayerPrefs.GetString("Username");
        string SentPath = string.Concat("/users/", MyName);
        SentPath = string.Concat(SentPath, "/friends/");
        SentPath = string.Concat(SentPath, FriendName);
        PushData(SentPath, "Pending");

        string SendPath = string.Concat("/users/", FriendName);
        string SendRequestPath = string.Concat(SendPath, "/friends/");
        SendRequestPath = string.Concat(SendRequestPath, MyName);
        PushData(SendRequestPath, "Requesting");
    }

    public void updateFriendsList(List<Friends> friendslist)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        foreach (Friends friend in friendslist)
        {
            if (friend.Status == "Friend")
            {
                setFriend(friend.Name);
            }
            else if (friend.Status == "Delete")
            {
                deleteFriend(friend.Name);
            }
        }
    }

    public void setFriend(string friendName)
    {
        string MyName = PlayerPrefs.GetString("Username");
        string SentPath = string.Concat("/users/", MyName);
        SentPath = string.Concat(SentPath, "/friends/");
        SentPath = string.Concat(SentPath, friendName);
        PushData(SentPath, "Friend");

        string SendPath = string.Concat("/users/", friendName);
        string SendRequestPath = string.Concat(SendPath, "/friends/");
        SendRequestPath = string.Concat(SendRequestPath, MyName);
        PushData(SendRequestPath, "Friend");
    }

    public void deleteFriend(string friendName)
    {
        string MyName = PlayerPrefs.GetString("Username");
        string SentPath = string.Concat("/users/", MyName);
        SentPath = string.Concat(SentPath, "/friends/");
        SentPath = string.Concat(SentPath, friendName);
        DeleteData(SentPath);

        string SendPath = string.Concat("/users/", friendName);
        string SendRequestPath = string.Concat(SendPath, "/friends/");
        SendRequestPath = string.Concat(SendRequestPath, MyName);
        DeleteData(SendRequestPath);
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
        return friendslist;
    }

    public void setProfile(string num, string name, string race, string height, string weight, string age)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string MyName = PlayerPrefs.GetString("Username");
        PushData("/users/" + MyName + "/Profile/Picture/", num);
        PushData("/users/" + MyName + "/Profile/CharName/", name);
        PushData("/users/" + MyName + "/Profile/Race/", race);
        PushData("/users/" + MyName + "/Profile/Height/", height);
        PushData("/users/" + MyName + "/Profile/Weight/", weight);
        PushData("/users/" + MyName + "/Profile/Age/", age);
    }

    public Dictionary<string, string> getProfile()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        Dictionary<string, string> profile = new Dictionary<string, string>();

        FirebaseDatabase.DefaultInstance.GetReference("users/" + currentUser + "/Profile/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                return profile;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    profile.Add(child.Key.ToString(), child.Value.ToString());
                }
                return profile;
            }
            else
            {
                return profile;
            }
        });
        return profile;
    }

    public string GetData(string path)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string data = "error1";

        FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                return "error3";
            }
            else if (task.IsCompleted)
            {
                task.Wait();
                DataSnapshot snapshot = task.Result;
                data = snapshot.GetRawJsonValue().ToString();
                data = data.Remove(0, 1);
                data = data.Remove((data.Length) - 1, 1);
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("TestGetData").SetValueAsync(data);
                return data;
            }
            else
            {
                return "error2";
            }
        });
        return "error1";
    }
}
