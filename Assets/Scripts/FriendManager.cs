using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FriendManager : MonoBehaviour
{
    public InputField userDB;

    public GameObject popupprefab;
    public GameObject popUp;
    public GameObject parent;

    void Start()
    {
        popUp = Instantiate(popupprefab, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        popUp.SendMessage("deactivatePopUp");
        popUp.SendMessage("setPrefab", popUp);
    }

    public void searchThisUser()
    {
        if (!string.IsNullOrEmpty(userDB.text))
        {
            string searchUsername = userDB.text;
            requestSearchUser(searchUsername);
        }
        else
        {
            Debug.Log("SearchFriend: username field is empty!");
            popUp.SendMessage("activatePopUp", "Username field is empty!");
        }
    }

    private void requestSearchUser(string name)
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
                    Debug.Log("No Valid Username");
                    popUp.SendMessage("activatePopUp", "Username does not exist");
                }
                else
                {
                    Debug.Log("User exists, would you like to add to Friends?");
                    popUp.SendMessage("setFriendRequestName", name);
                    popUp.SendMessage("setOkButton", "SendFriendRequest");
                    popUp.SendMessage("activatePopUp", "User exists, would you like to add to Friends?");
                }
            }
            else
            {
                Debug.Log("Error: " + task.Exception);
                return;
            }
        });
    }
}
