using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Friend_List : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image FriendPrefab;
    [SerializeField]
    Image Grid;
    
    public List<Friends> friends;
    public List<Image> entries;
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

    }

    public void Set_Category(string Cat)
    {
        Destroy_Entries();
        
        foreach(Friends friend in friends)
        {
            if (Cat == friend.Status)
            {
                Image I = Instantiate(FriendPrefab, Grid.transform); 
                I.SendMessage("SetName", friend.Name);
                entries.Add(I);
            }

            
        }
    }

    public void Destroy_Entries()
    {
        foreach(Image entry in entries)
        {
            Destroy(entry.gameObject);
        }

        entries.Clear();
    }
}
