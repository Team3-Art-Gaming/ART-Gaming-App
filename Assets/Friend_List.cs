using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friends
{
    public string Name;
    public string Status; 
    public Friends(string name, string status)
    {
        Name = name;
        Status = status; 
    }
}

public class Friend_List : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image FriendPrefab;
    [SerializeField]
    Image Grid;
    
    public List<Friends> friends;
    void Start()
    {
        friends = new List<Friends>();
        Friends F = new Friends("John", "Pending");
        friends.Add(F);
        F = new Friends("Alice", "Requesting");
        friends.Add(F);
        F = new Friends("Mike", "Pending");
        friends.Add(F);
        F = new Friends("Oscar", "Requesting");
        friends.Add(F);
        F = new Friends("Diana", "Friend");
        friends.Add(F);
        F = new Friends("Max", "Friend");
        friends.Add(F);
        F = new Friends("George", "Friend");
        friends.Add(F);
        F = new Friends("James", "Requesting");
        friends.Add(F);
        F = new Friends("Hector", "Pending");
        friends.Add(F);
        F = new Friends("Jessie", "Requesting");
        friends.Add(F);
        F = new Friends("Ashley", "Pending");
        friends.Add(F);

        foreach(Friends friend in friends)
        {
            Image I = Instantiate(FriendPrefab, Grid.transform);
            I.SendMessage("SetName", friend.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
