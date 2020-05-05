//This script is used in the Home Screen to request any active games the user is invited to and display them

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Active_Games : MonoBehaviour
{
    [SerializeField]
    Image ActiveGamesPrefab;            //A prefabricated piece to display an game name and accept and reject buttons
    [SerializeField]
    Image Grid;                         //A vertical layout grid for holding a created ActiveGamesPrefabs

    public List<Games> games;           //These games the user is invited to
    public List<Image> entries;         //The instaniated ActiveGamesPrefab peices

    public GameObject popUp;            //A popup to give players options
    public GameObject joinArenaPrefab;  //A popup for jpining a game
    public GameObject parent;           //The owner of the object this script is attached to
    private GamesManager gamesScript;   //A instance of the gamemanager class

    //Called as soon as the object holding this this script is in a scene
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait; //Force phones screen into portrait mode for this scene
    }

    //Called as soon and the scene containing this is done building.
    void Start()
    {
        //Create the join arean popup and set deactive to have ready later
        joinArenaPrefab = Instantiate(popUp, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        joinArenaPrefab.SendMessage("deactivateLiveArena");

        //Initialize the GamesMananger and the list to hold games
        this.gamesScript = GameObject.Find("SceneManager").GetComponent<GamesManager>() as GamesManager;
        this.games = new List<Games>();

        //Call DB for active games.
        RequetActivGames();
    }

    //Request all active games for this user from DB, wait n seconds, then call display 
    public void RequetActivGames()
    {
        Destroy_Entries();
        this.games = gamesScript.GetGamesList();
        foreach(Games child in this.games)
        {
            Debug.Log(child.SessionName + ": " + child.Status + ": " + child.HostName);
        }
        StartCoroutine(WaitBeforeDisplay(1));
    }

    IEnumerator WaitBeforeDisplay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.ShowActiveGames();
    }

    //Create the visual display of games for users to select from
    public void ShowActiveGames()
    {
        foreach (Games child in this.games)
        {
            Image I = Instantiate(ActiveGamesPrefab, Grid.transform);
            I.SendMessage("setSessionName", child.SessionName);
            I.SendMessage("setHostName", child.HostName);
            this.entries.Add(I);
        }
    }

    public void Destroy_Entries()
    {
        foreach (Image entry in entries)
        {
            Destroy(entry.gameObject);
        }
        this.entries.Clear();
    }
}
