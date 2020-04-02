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


public class CreateUserFB : MonoBehaviour
{
    public InputField enterEmail;
    public InputField enterUsername;
    public InputField enterPass;
    public InputField reEnterPass;
    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    //Added a public function that pushes the class User as a Json into database.
    public CreateUserFB() { }

    public void pushUserJson(User currentUser)
    {
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        //DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        //string json = JsonUtility.ToJson(currentUser);
        //DBreference.Child("users").Child(currentUser.getUserid()).SetRawJsonValueAsync(json);
        //Debug.Log("Writing User info to database");
    }
    //Rest of Alex's code remains intact



    //public void pushUser()
    public async void pushUser()
    {

        string playerName = enterUsername.text;
        string playerEmail = enterEmail.text;
        string playerPass = enterPass.text;

        string path = "/1Test/";
        string NamePath = string.Concat(path, playerName);
        //PushData(NamePath, playerName);

        string EmailPath = string.Concat(NamePath, "/email/");
        //PushData(EmailPath, playerEmail);

        string PassPath = string.Concat(NamePath, "/password/");
        //PushData(PassPath, playerPass);

        string FriendPath = string.Concat(NamePath, "/friends/");
        //PushData(FriendPath, "friends");

        string CurrentGamesPath = string.Concat(NamePath, "/CurrentGames/");
        //PushData(CurrentGamesPath, "CurrentGames");

        //AddUser(playerName);
        //string test = GetData("/1Test/7767alex/email"); returns "{\"RecievedFriendRequests\":\"RecievedFriendRequests\",\"SentFriendRequests\"
        // :{\"david\":\"pending\"},\"email\":\"g@mail.com\",\"password\":\"pass\"}"

        SendFriendRequests("Marc", "kev");
        //SendFriendRequests("alex", "james");
        //AcceptRecievedFriendRequests("james", "alex");

    }

    public void AddUser(string name)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(name).SetValueAsync("1");
    }

    public bool DoesUserExist(string name)
    {
        int exist = 1;// = GetDataAsync(name);
        if (exist == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

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

    public void SendFriendRequests(string MyName, string FriendName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;


        string SentPath = string.Concat("/users/", MyName);
        SentPath = string.Concat(SentPath, "/friends/");
        SentPath = string.Concat(SentPath, FriendName);
        PushData(SentPath, "Pending");

        string SendPath = string.Concat("/users/", FriendName);
        string SendRequestPath = string.Concat(SendPath, "/friends/");
        SendRequestPath = string.Concat(SendRequestPath, MyName);
        PushData(SendRequestPath, "Requesting");
    }

    public void AcceptRecievedFriendRequests(string MyName, string FriendName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string MyPath = "/1Test/";
        MyPath = string.Concat(MyPath, MyName);
        MyPath = string.Concat(MyPath, "/RecievedFriendRequests/");
        MyPath = string.Concat(MyPath, FriendName);
        PushData(MyPath, "Accepted");

        string AcceptPath = "/1Test/";
        AcceptPath = string.Concat(AcceptPath, MyName);
        AcceptPath = string.Concat(AcceptPath, "/RecievedFriendRequests/");
        //AcceptPath = string.Concat(AcceptPath, FriendName);
        FirebaseDatabase.DefaultInstance.GetReference(AcceptPath).Child(FriendName).RemoveValueAsync();
        AcceptPath = "/1Test/";
        AcceptPath = string.Concat(AcceptPath, MyName);
        AcceptPath = string.Concat(AcceptPath, "/friends/");
        FirebaseDatabase.DefaultInstance.GetReference(AcceptPath).Child(FriendName).SetValueAsync("1");

        string FriendPath = "/1Test/";
        FriendPath = string.Concat(FriendPath, FriendName);
        FriendPath = string.Concat(FriendPath, "/SentFriendRequests/");
        //FriendPath = string.Concat(FriendPath, MyName);
        FirebaseDatabase.DefaultInstance.GetReference(FriendPath).Child(MyName).RemoveValueAsync();
        FriendPath = "/1Test/";
        FriendPath = string.Concat(FriendPath, FriendName);
        FriendPath = string.Concat(FriendPath, "/friends/");
        FirebaseDatabase.DefaultInstance.GetReference(FriendPath).Child(MyName).SetValueAsync("1");
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

    // Start is called before the first frame update
    void Start()
    {
        /*
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize {task.Exception}");
                return;
            }
            OnFirebaseInitialized.Invoke();
            string json = JsonUtility.ToJson(enterUsername.text);
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
            DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            DBreference.Child("/users").SetRawJsonValueAsync(json);
            //DBreference.GetReference("/test").SetRawJsonValueAsync(JsonUtility.ToJson(example.text));
        });
        */
    }

    // Update is called once per frame
    void Update()
    {

    }


}
