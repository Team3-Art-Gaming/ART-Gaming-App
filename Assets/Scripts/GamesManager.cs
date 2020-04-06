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
    public InputField SessionNameIN;
    public InputField MapNameIN;
    private Friend_List flScript;
    private Games gScript;

    public void LaunchGame()    ///paramters for this function should be map name and unique session name
	{

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string SessionName = SessionNameIN.text;
        string MapName = MapNameIN.text;

        string name = PlayerPrefs.GetString("Username");

        //PushData("/1Test/ActiveGames/" + SessionName, SessionName);
        FirebaseDatabase.DefaultInstance.GetReference("/1Test/").Child("ActiveGames").Child(SessionName).SetValueAsync(SessionName);
        PushData("/1Test/ActiveGames/" + SessionName + "/Players/", "Players");
        PushData("/1Test/ActiveGames/" + SessionName + "/Players/" + name, "Host");
        PushData("/1Test/ActiveGames/" + SessionName + "/MapName/", MapName);

        string MapString = PlayerPrefs.GetString(MapName);
        PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", MapString);

        List<Friends> FriendsList = new List<Friends>();
        FriendsList = GetFriendsList();
        FriendsList.Add(new Friends("kev", "Friend"));

        string FriendName;
        for(var i = 0; i < FriendsList.Count;i++)
		{
            Debug.Log("For LOOP: "+FriendsList[i].Name);
            FriendName = FriendsList[i].Name;
            //FirebaseDatabase.DefaultInstance.GetReference("/1Test/ActiveGames/" + SessionName + "Players").Child(FriendName).SetValueAsync("Invited");
            PushData("/1Test/ActiveGames/" + SessionName + "/Players/" + FriendName, "Invited");
            FirebaseDatabase.DefaultInstance.GetReference("/users/" + FriendName + "/Games/").Child(SessionName).SetValueAsync("Invited");

        }

        //List<string> data = new List<string>();
        //string data;
        //data = GetData("/1Test/users/" + name + "/MapName/");
        //string map = (data.Count).ToString();
        //PushData("/1Test/ActiveGames/" + SessionName + "/MapString/", map);

        //data = GetDataString("/1Test/users/alex/email/");
        //string test = "testDef";//((data[0]).ToString());
        //string test = data[0];
        //PushData("/1Test/test/", data);
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
                    //Debug.Log(child.Key + ": " + child.Value);
                    GamesList.Add(new Games(child.Key.ToString(), child.Value.ToString()));
                }
                return GamesList;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        int milliseconds = 4000;
        Thread.Sleep(milliseconds);
        return GamesList;

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
                    //Debug.Log(child.Key + ": " + child.Value);
                    if (child.Value.ToString() == "Friend")
                    {
                        friendslist.Add(new Friends(child.Key.ToString(), child.Value.ToString()));
                        Debug.Log("Friend Name : " + child.Key.ToString());
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
        int milliseconds = 4000;
        Thread.Sleep(milliseconds);
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