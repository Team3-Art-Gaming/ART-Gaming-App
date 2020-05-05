//Control active game prefabs with names along with the accept button for the homescreen

using UnityEngine;
using UnityEngine.UI;

public class ActiveGamePrefab : MonoBehaviour
{
    [SerializeField]
    Text SessionName;               //The readout the display the session name
    [SerializeField]
    Text HostName;                  //The readout the display the host name
    [SerializeField]
    Button joinActiveGameButton;    //The button to join the game

    private string gameName;
    private string hostName;
    private GameObject popUp;

    // Start is called before the first frame update
    public void setSessionName(string name)
    {
        this.gameName = name;
        this.SessionName.text = name;
    }

    public void setHostName(string name)
    {
        this.hostName = name;
        this.HostName.text = name;
    }

    public void getArenaPrefab()
    {
        this.popUp = GameObject.Find("LIVE_Arena(Clone)");
        this.popUp.SendMessage("activateLiveArena");
        PlayerPrefs.SetString("CurrentSession", this.SessionName.text);
        PlayerPrefs.Save();
    }
}
