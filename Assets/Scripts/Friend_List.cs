using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Friend_List : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image FriendPrefab;
    [SerializeField]
    Image Grid;
    [SerializeField] 
    Button friend;
    [SerializeField] 
    Button pending;
    [SerializeField] 
    Button requesting;
    
    
    public List<Friends> friends;
    public List<Image> entries;

    private CreateUserFB frScript;
    private string sceneStatus;

    private void Awake()
    {
    }
    void Start()
    {   
        if (PlayerPrefs.HasKey("FriendScene"))
        {
            this.sceneStatus = PlayerPrefs.GetString("FriendScene");
        }
        else
        {
            PlayerPrefs.SetString("FriendScene", "Friend");
            PlayerPrefs.Save();
            this.sceneStatus = "Friend";
        }

        this.frScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        this.friends = new List<Friends>();

        this.friends = frScript.GetFriendsList();
        
        StartCoroutine(Test());

        /*this.friends.Add(new Friends("friend1","Friend"));
        this.friends.Add(new Friends("friend2", "Friend"));
        this.friends.Add(new Friends("friend3", "Friend"));
        this.friends.Add(new Friends("friend4", "Requesting"));
        this.friends.Add(new Friends("friend5", "Requesting"));
        this.friends.Add(new Friends("friend6", "Requesting"));
        this.friends.Add(new Friends("friend7", "Pending"));
        this.friends.Add(new Friends("friend8", "Pending"));
        this.friends.Add(new Friends("friend9", "Pending"));*/

        //Set_Category(this.sceneStatus);

    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);

        this.Set_Category(this.sceneStatus);
    }

    public void Set_Category(string Cat)
    {
        Destroy_Entries();
        PlayerPrefs.SetString("FriendScene", Cat);
        PlayerPrefs.Save();

        if (Cat == "Friend")
        {
            friend.GetComponent<Image>().color = Color.white;
            pending.GetComponent<Image>().color = Color.gray;
            requesting.GetComponent<Image>().color = Color.gray;
        }
        
        else if (Cat == "Requesting")
        {
            friend.GetComponent<Image>().color = Color.gray;
            pending.GetComponent<Image>().color = Color.gray;
            requesting.GetComponent<Image>().color = Color.white;
        }
        
        else if (Cat == "Pending")
        {
            friend.GetComponent<Image>().color = Color.gray;
            pending.GetComponent<Image>().color = Color.white;
            requesting.GetComponent<Image>().color = Color.gray;
        }

        foreach (Friends friend in this.friends)
        {
            if (Cat == friend.Status)
            {
                Image I = Instantiate(FriendPrefab, Grid.transform); 
                I.SendMessage("SetName", friend);
                entries.Add(I);
            }
        }
    }

    public void Destroy_Entries()
    {
        foreach(Image entry in this.entries)
        {
            Destroy(entry.gameObject);
        }

        entries.Clear();
    }

    public void testUpdate()
    {
        Destroy_Entries();
        this.friends = frScript.GetFriendsList();
    }
}


/*
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
        friends.Add(F);*/
