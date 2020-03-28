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
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string json = JsonUtility.ToJson(currentUser);
        DBreference.Child("users").Child(currentUser.getUserid()).SetRawJsonValueAsync(json);
        Debug.Log("Writing User info to database");
    }
    //Rest of Alex's code remains intact



    public void pushUser()
	{

        string playerName = enterUsername.text;
        string playerEmail = enterEmail.text;
        string playerPass = enterPass.text;

        string path = "/1Test/";
        string NamePath = string.Concat(path, playerName);
        PushData(NamePath, playerName);

        string EmailPath = string.Concat(NamePath, "/email/");
        PushData(EmailPath, playerEmail);

        string PassPath = string.Concat(NamePath, "/password/");
        PushData(PassPath, playerPass);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(playerName).SetValueAsync("1");
        //string UserListUpdate = string.Concat(path, "0UserList/");
        //PushData(UserListUpdate, playerName);
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
    }

    public void SendFriendRequests(string MyName, string FriendName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string SendPath = string.Concat("/1Test/", FriendName);
        string SentPath = string.Concat("/1Test/7767alex/SentFriendRequests/", FriendName);
        PushData(SentPath, "pending");

        string SendRequestPath = string.Concat(SendPath, "/RecievedFriendRequests/");
        SendRequestPath = string.Concat(SendRequestPath, MyName);
        PushData(SendRequestPath, "pending");

        /*
        if (GetData(SendPath) == null)
		{
            return;
		}
        else
        {
            PushData(SendPath, "pending");
            string SendRequestPath = string.Concat(SendPath, "/RecievedFriendRequests");
        }
        */
    }
    public string GetData(string path)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string GetPath = string.Concat("1Test/", path);


        FirebaseDatabase.DefaultInstance.GetReference(GetPath).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                string data = snapshot.GetRawJsonValue().ToString();
                DBreference.Child("/Test/0").SetValueAsync(data);
                return data;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
