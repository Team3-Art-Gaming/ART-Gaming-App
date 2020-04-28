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

public class MasterARRequest : MonoBehaviour
{
    public List<string> GetEntitiesString(string entType)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        List<string> Entities = new List<string>();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/" + entType + "/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);

                    Entities.Add(child.Value.ToString());
                }

                return Entities;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });

        return Entities;
    }

    public string GetGuestMapString()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string guestMapString = "";
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return guestMapString;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);
                    if (child.Key.ToString() == "guestMap")
                    {
                        guestMapString = child.Value.ToString();
                    }
                }

                return guestMapString;
            }
            else
            {
                Debug.Log("ELSE");
                return guestMapString;
            }
        });

        return guestMapString;
    }

    public string GetHostMapString()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string mapstring = "";
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return mapstring;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);
                    if(child.Key.ToString() == "MapString")
                    {
                        mapstring = child.Value.ToString();
                    }
                }
                return mapstring;
            }
            else
            {
                Debug.Log("ELSE");
                return mapstring;
            }
        });

        return mapstring;
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
        //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("blip").SetValueAsync(data);
    }
}
