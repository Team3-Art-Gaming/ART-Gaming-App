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

    public void pushUser()
	{
        string PlayerEmail = enterEmail.text;
        string PlayerUsername = enterUsername.text;
        string PlayerPass = enterPass.text;


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;



        DBreference.Child("/users/0/email").SetValueAsync(PlayerEmail);
        DBreference.Child("/users/0/name").SetValueAsync(PlayerUsername);
        DBreference.Child("/users/0/password").SetValueAsync(PlayerPass);


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
