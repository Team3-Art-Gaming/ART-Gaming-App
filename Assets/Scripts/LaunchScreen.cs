using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image LaunchPrefab;
    [SerializeField]
    Image Grid;
    [SerializeField]
    Button launchBTN;
    [SerializeField]
    Button homeBTN;

    public List<Friends> friends;
    public List<Image> entries;


    void Start()
    {
        this.friends = new List<Friends>();
        this.friends.Add(new Friends("friend1","Friend"));
        this.friends.Add(new Friends("friend2", "Friend"));
        this.friends.Add(new Friends("friend3", "Friend"));
        ShowFriends();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFriends()
    {
        foreach (Friends friend in this.friends)
        {
            Image I = Instantiate(LaunchPrefab, Grid.transform);
            I.name = friend.Name;
            I.SendMessage("SetName", friend.Name);
            entries.Add(I);
        }
    }

    public void LaunchButtonClicked()
    {
        List<string> invitedFriends = new List<string>();
        foreach (Image entry in this.entries)
        {
            Toggle tog = entry.GetComponentInChildren<Toggle>() as Toggle;
            if (tog.isOn) invitedFriends.Add(entry.name);
        }
        foreach(string s in invitedFriends)
        {
            Debug.Log(s);
        }
    }
}
