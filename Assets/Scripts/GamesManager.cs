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

public class GamesManager : MonoBehaviour
{
    private Friend_List flScript;
    private Games gScript;

    public void LaunchGame(string SessionName, string MapName, string MapString, List<string> friends)    ///paramters for this function should be map name and unique session name
	{

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string name = PlayerPrefs.GetString("Username");

        //PushData("/1Test/ActiveGames/" + SessionName, SessionName);
        FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/").Child(SessionName).SetValueAsync(SessionName);
        PushData("/ActiveGames/" + SessionName + "/Players/", "Players");
        PushData("/ActiveGames/" + SessionName + "/Players/" + name, "Host");
        PushData("/ActiveGames/" + SessionName + "/MapName/", MapName);
        PushData("/ActiveGames/" + SessionName + "/MapString/", MapString);

        foreach(string child in friends)
        {
            PushData("/ActiveGames/" + SessionName + "/Players/" + child, "Invited");
        }
    }

    public void createInvitedSessionUsers(List<string> friends, string SessionName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        foreach (string child in friends)
        {
            FirebaseDatabase.DefaultInstance.GetReference("/users/" + child + "/Games/").Child(SessionName).SetValueAsync("Invited");
        }

        string user = PlayerPrefs.GetString("Username");
        FirebaseDatabase.DefaultInstance.GetReference("/users/" + user + "/Games/").Child(SessionName).SetValueAsync("Host");
    }

    public List<Games> GetGamesList()
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string name = PlayerPrefs.GetString("Username");
        List<Games> GamesList = new List<Games>();

        FirebaseDatabase.DefaultInstance.GetReference("users/" + name + "/Games/").GetValueAsync().ContinueWithOnMainThread(task => {
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

                foreach (var child in snapshot.Children)
                {
                    string hostName = getHostName(child.Key.ToString());
                    if(child.Value.ToString() == "Invited")
                    {
                        //Debug.Log(child.Key + ": " + child.Value);
                        GamesList.Add(new Games(child.Key.ToString(), child.Value.ToString(), hostName));
                    }
                    
                }
                return GamesList;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        /*
        int milliseconds = 4000;
        Thread.Sleep(milliseconds);*/
        return GamesList;

    }

    public string getHostName(string SessionName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + SessionName + "/Players/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return "NA";
            }
            else if (task.IsCompleted)
            {
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(playerName).SetValueAsync("1");
                DataSnapshot snapshot = task.Result;
                //string data = snapshot.Children;

                foreach (var child in snapshot.Children)
                {
                    if (child.Value.ToString() == "Host")
                    {
                        //Debug.Log(child.Key + ": " + child.Value);
                        return child.Value.ToString();
                    }

                }
                return "NA";
            }
            else
            {
                Debug.Log("ELSE");
                return "NA";
            }
        });
        return "NA";
    }

    public List<Maps> GetCreatedMapsList()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        List<Maps> cmList = new List<Maps>();
        //Debug.Log("GetCreatedMapsList From: " + currentUser);

        FirebaseDatabase.DefaultInstance.GetReference("users/" + currentUser + "/CreatedMaps/").GetValueAsync().ContinueWithOnMainThread(task => {
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
                //Debug.Log("taskCOmplete");
                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);
                    cmList.Add(new Maps(child.Key.ToString(), child.Value.ToString()));
                    //Debug.Log("Map Name : " + child.Key.ToString());
                }

                /*
                foreach (Maps child in cmList)
                {
                    Debug.Log(child.mapName + ": " + child.mapString);
                }*/

                return cmList;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        /*
        int milliseconds = 4000;
        Thread.Sleep(milliseconds);*/
        return cmList;
    }

    public List<Friends> GetFriendsList()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        List<Friends> friendslist = new List<Friends>();
        //Debug.Log("???????" + currentUser);

        FirebaseDatabase.DefaultInstance.GetReference("users/" + currentUser + "/friends/").GetValueAsync().ContinueWithOnMainThread(task => {
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
                //Debug.Log("taskCOmplete");
                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);
                    if (child.Value.ToString() == "Friend")
                    {
                        friendslist.Add(new Friends(child.Key.ToString(), child.Value.ToString()));
                        //Debug.Log("Friend Name : " + child.Key.ToString());
                    }
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

    public string GetDataString(string path)
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        string data = "null";

        FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("faultError");
                return null;
            }
            else if (task.IsCompleted)
            {
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("completed");
                DataSnapshot snapshot = task.Result;
                data = (snapshot.GetRawJsonValue().ToString());
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync(data);
                return data;
            }
            else
            {
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("elseError");
                Debug.Log("ELSE");
                return null;
            }
        });
        return data;
    }

    public List<string> GetData(string path)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string currentUser = PlayerPrefs.GetString("Username");
        List<string> data = new List<string>();

        FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("faultError");
                return null;
            }
            else if (task.IsCompleted)
            {
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("completed");
                DataSnapshot snapshot = task.Result;
                data.Add(snapshot.GetRawJsonValue().ToString());
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync(data[0]);
                return data;
            }
            else
            {
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("blarg").SetValueAsync("elseError");
                Debug.Log("ELSE");
                return null;
            }
        });
        return data;
    }

    public async Task GetAndPushMap(string SessionName, string MapName)
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string data = "error1";

        string name = PlayerPrefs.GetString("Username");
        string path = "/1Test/users/" + name + "/CreatedMaps/" + MapName;
        PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", data);
        FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", data);
                return;
            }
            else if (task.IsCompleted)
            {
                
                //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child(playerName).SetValueAsync("1");
                DataSnapshot snapshot = task.Result;
                data = snapshot.GetRawJsonValue().ToString();
                data = data.Remove(0, 1);
                data = data.Remove((data.Length) - 1, 1);
                data = "test";
                PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", data);
                FirebaseDatabase.DefaultInstance.GetReference("/1Test/ActiveGames/"+SessionName).Child("MapString").SetValueAsync(data);
                return;
            }
            else
            {
                Debug.Log("ELSE");
                PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", data);
                return;
            }
        });
        //return;
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
        //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("blip").SetValueAsync(data);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
art-152
1Test
ActiveGames
SessionName2Players
kev: 
"Invited"
SessionNameUnity
MapName: 
"Map1"
MapString: 
"00300310003001300030013000300130NNNNNNNNNNNNNNN..."
Players
alex: 
"Host"
MAPS
SessionName
users
G90
CreatedMaps
0: 
"{date : name }"
email: 
"gonzalezgerardo@mail.fresnostate.edu"
friends
kev: 
"Requesting"
userid: 
"30LwAET2o0WR0WgjG8oO9B55lNy1"
username: 
"G90"
J42
CreatedMaps
0: 
"{date : name }"
email: 
"g90@yahoo.com"
userid: 
"LHybZo2XWzXoBMuRlVOJwD5RZKG3"
username: 
"J42"
J43
CreatedMaps
0: 
"{date : name }"
email: 
"g90@gmail.com"
friends
kev: 
"Requesting"
userid: 
"IuIwGSidLVT0mNh3vTNWurM0NUi1"
username: 
"J43"
alex
CreatedMaps
Map1: 
"00300310003001300030013000300130NNNNNNNNNNNNNNN..."
TempMapName: 
"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN..."
TestingMapName: 
"003001300030013000300c100030070000100000NNNNNNN..."
email: 
"7767alex@mail.fresnostate.edu"
friends
Marc: 
"Friend"
kev: 
"Friend"
userid: 
"nFMbTmjqjrdHIC9tNlVHmYYVoiJ3"
username: 
"alex"
kev
CreatedMaps
0: 
"{date : name }"
Games
SessionName2: 
"Invited"
email: 
"da.secretaznman@gmail.com"
friends
G90: 
"Pending"
J43: 
"Pending"
Marc: 
"Friend"
alex: 
"Friend"
friend4: 
"Friend"
friend5: 
"Requesting"
friend6: 
"Requesting"
userid: 
"94lCJnX5dJaify7lS3yntRPzSNT2"
username: 
"kev"
*/