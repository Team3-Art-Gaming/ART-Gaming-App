using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Active_Games : MonoBehaviour
{
    [SerializeField]
    Image ActiveGamesPrefab;
    [SerializeField]
    Image Grid;

    public List<Games> games;
    public List<Image> entries;

    public GameObject popUp;
    public GameObject joinArenaPrefab;
    public GameObject parent;
    private GamesManager gamesScript;

    // Start is called before the first frame update
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void Start()
    {
        joinArenaPrefab = Instantiate(popUp, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        joinArenaPrefab.SendMessage("deactivateLiveArena");

        this.gamesScript = GameObject.Find("SceneManager").GetComponent<GamesManager>() as GamesManager;
        this.games = new List<Games>();

        StartCoroutine(Test(1));
    }

    IEnumerator Test(int seconds)
    {
        this.games = gamesScript.GetGamesList();
        foreach(Games child in this.games)
        {
            Debug.Log(child.SessionName + ": " + child.Status + ": " + child.HostName);
        }
        yield return new WaitForSeconds(seconds);

        this.showActiveGames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showActiveGames()
    {
        Destroy_Entries();
        /*
        Image I = Instantiate(ActiveGamesPrefab, Grid.transform);
        I.SendMessage("setSessionName", "Example Session Name");
        I.SendMessage("setHostName", "Some Random Jackass");
        entries.Add(I);

        I = Instantiate(ActiveGamesPrefab, Grid.transform);
        I.SendMessage("setSessionName", "Example Session Name2");
        I.SendMessage("setHostName", "Some Random Asshole");
        entries.Add(I);
        */
        //Debug.Log("Constructing Prefabs");
        
        foreach (Games child in this.games)
        {
            //Debug.Log(child.mapName + ": " + child.mapString);
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
