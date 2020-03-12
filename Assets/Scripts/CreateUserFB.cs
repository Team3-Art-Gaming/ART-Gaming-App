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
        string PlayerEmail = enterEmail.text;
        string PlayerUsername = enterUsername.text;
        string PlayerPass = enterPass.text;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");



        DBreference.Child("/1Test/0/email").SetValueAsync(PlayerEmail);
        DBreference.Child("/1Test/0/name").SetValueAsync(PlayerUsername);
        DBreference.Child("/1Test/0/password").SetValueAsync(PlayerPass);
        GetData();
        /*
        FirebaseDatabase.DefaultInstance.GetReference("/usersTest/0/name").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            string ss = snapshot.Child("/usersTest/0/name").Value.ToString();
            print(ss);
            print("data retrieved");
            DBreference.Child("/usersTest/0/email").SetValueAsync("UserNumber");
        });
        */
    }
    public void GetData()
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance.GetReference("1Test").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                string data = snapshot.GetRawJsonValue().ToString();
                DBreference.Child("/1Test/0/test").SetValueAsync(data);
            }
            else
            {
                Debug.Log("ELSE");
            }
        });
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
