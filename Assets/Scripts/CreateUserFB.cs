using System.Collections;
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
    //var myTask = Task.Run(() => {return GetData(path));
    //string result = await myTask;
    //return result;

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
            Debug.Log(friend.Name+": "+friend.Status);
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

        FirebaseDatabase.DefaultInstance.GetReference("users/"+currentUser+"/friends/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(playerName).SetValueAsync("1");
                DataSnapshot snapshot = task.Result;
                //string data = snapshot.Children;
              
                foreach ( var child in snapshot.Children)
                { 
                    //Debug.Log(child.Key + ": " + child.Value);
                    
                    friendslist.Add(new Friends(child.Key.ToString(), child.Value.ToString()));
                }
                return friendslist;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        /*
        int milliseconds = 2000;
        Thread.Sleep(milliseconds);*/
        return friendslist;
    }

    public string GetData(string path)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        //string GetPath = string.Concat("1Test/", path);
        string data = "error1";

        FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return "error3";
            }
            else if (task.IsCompleted)
            {
                task.Wait();
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(playerName).SetValueAsync("1");
                DataSnapshot snapshot = task.Result;
                data = snapshot.GetRawJsonValue().ToString();
                data = data.Remove(0, 1);
                data = data.Remove((data.Length) - 1, 1);
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("TestGetData").SetValueAsync(data);
                return data;
            }
            else
            {
                Debug.Log("ELSE");
                return "error2";
            }
        });
        //b.Wait();
        return "error1";
    }
}
