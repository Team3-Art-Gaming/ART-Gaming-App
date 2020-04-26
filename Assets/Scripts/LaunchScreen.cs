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

    public GameObject popupprefab;
    public GameObject popUp;
    public GameObject sessionNamePrefab;
    public GameObject sessionPopUp;

    public GameObject parent;

    public List<Friends> friends;
    public List<Image> entries;

    private GamesManager gamesScript;


    void Start()
    {
        popUp = Instantiate(popupprefab, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        popUp.SendMessage("deactivatePopUp");
        popUp.SendMessage("setPrefab", popUp);

        sessionPopUp = Instantiate(sessionNamePrefab, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        sessionPopUp.SendMessage("deactivatePopUp");
        sessionPopUp.SendMessage("setPrefab", sessionPopUp);

        this.gamesScript = GameObject.Find("SceneManager").GetComponent<GamesManager>() as GamesManager;
        this.friends = new List<Friends>();
        this.friends = gamesScript.GetFriendsList();

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);

        this.ShowFriends();
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
        //Maximum size of game lobby is 6 players excluding Host
        if(invitedFriends.Count > 6)
        {
            Debug.Log("Exceeded maximum number of players! Maximum number of players is SIX.");
            popUp.SendMessage("activatePopUp", "Exceeded maximum number of players! Maximum number of players is SIX.");
        }
        else if(invitedFriends.Count < 1)
        {
            Debug.Log("You have no friends invited to this session!");
            popUp.SendMessage("activatePopUp", "You have no friends invited to this session!");
        }
        else
        {
            sessionPopUp.SendMessage("saveInvitedFriends", invitedFriends);
            sessionPopUp.SendMessage("activatePopUp", "Inviting: ");
        }
    }
}
