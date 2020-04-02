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
public class GamesManager : MonoBehaviour
{

    public void LaunchGame() // (string MapName) /// Parameter needed to call the function 
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string MapName = "TempName";

        string name = PlayerPrefs.GetString("Username");
        string MapString = PlayerPrefs.GetString("TempLevel");
        string path = "/1Test/ActiveGames/";
        //path = string.Concat(path, name);
        FirebaseDatabase.DefaultInstance.GetReference(path).Child(name).SetValueAsync(name);
        path = string.Concat(path, name);
        FirebaseDatabase.DefaultInstance.GetReference(path).Child("Host").SetValueAsync(name);
        FirebaseDatabase.DefaultInstance.GetReference(path).Child("MapName").SetValueAsync(MapName);
        FirebaseDatabase.DefaultInstance.GetReference(path).Child("MapString").SetValueAsync(MapString);
        FirebaseDatabase.DefaultInstance.GetReference(path).Child("InGameUsers").SetValueAsync("InGameUsers");


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
MAPS
map: 
"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN..."
mapsize: 
16468
user: 
"alex"
MAPS
LastModified: 
"Today"
MapID: 
"0"
MapString: 
"NNFF000FFA31NFF882FFA33N"
users
G90
CreatedMaps
0: 
"{date : name }"
email: 
"gonzalezgerardo@mail.fresnostate.edu"
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
Marc
CreatedMaps
0: 
"{date : name }"
email: 
"herdmanmarca@gmail.com"
friends
alex: 
"Pending"
kev: 
"Friend"
userid: 
"4vL9FaW0BfORa6kInwqzakZiPw62"
username: 
"Marc"
alex
CreatedMaps
0: 
"{date : name }"
TempMapName: 
"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN..."
email: 
"7767alex@mail.fresnostate.edu"
friends
Marc: 
"Requesting"
kev: 
"Requesting"
userid: 
"nFMbTmjqjrdHIC9tNlVHmYYVoiJ3"
username: 
"alex"
friend4
friends
kev: 
"Friend"
friend5
friends
kev: 
"Friend"
friend6
friends
kev: 
"Friend"
kev
CreatedMaps
0: 
"{date : name }"
email: 
"da.secretaznman@gmail.com"
friends
J43: 
"Pending"
Marc: 
"Friend"
alex: 
"Pending"
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