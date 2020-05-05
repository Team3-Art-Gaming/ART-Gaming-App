using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pop_Up_Screen : MonoBehaviour
{
    public Text textbox;
    private GameObject caller;
    private Button ok_btn, c_btn;
    private int scene = 0;
    private string thisFriendRequest;
    private CreateUserFB sendFRScript;

    public void setPrefab(GameObject caller)
    {
        this.caller = caller;
        ok_btn = GameObject.Find("OK_Button").GetComponent<Button>();
        c_btn = GameObject.Find("Cancel_Button").GetComponent<Button>();
        ok_btn.onClick.AddListener(okay_Click);
        c_btn.onClick.AddListener(cancel_Click);
    }

    public void activatePopUp(string s)
    {
        transform.position = new Vector3(540, 960, 0);

        textbox.text = s;
    }

    public void deactivatePopUp()
    {
        transform.position = new Vector3(-10000, -10000, 0);
    }

    public void setSceneChange(int sceneNum)
    {
        this.scene = sceneNum;
    }

    public void setFriendRequestName(string name)
    {
        this.thisFriendRequest = name;
    }

    public void setOkButton(string s)
    {
        switch (s)
        {
            case "ChangeScene":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(okay_Click_Scene);
                break;
            case "SendFriendRequest":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(send_Friend_Request_Okay);
                break;
            case "RemoveFromFriends":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(okay_Click_Friend);
                break;
            case "AcceptRequesting":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(okay_Click_Request);
                break;
            case "RemoveFromPending":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(okay_Click_Pending);
                break;
            default:
                Debug.Log("invalid input from changing ok btn");
                break;
        }
    }

    public void setCancelButton(string s)
    {
        switch (s)
        {
            case "RemoveRequesting":
                c_btn.onClick.RemoveListener(cancel_Click);
                c_btn.onClick.AddListener(cancel_Click_Request);
                break;
            default:
                break;
        }

    }

    public void okay_Click()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "OK");
    }

    public void cancel_Click()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "Cancel");
    }

    public void okay_Click_Scene()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "ChangeScene");
    }

    public void send_Friend_Request_Okay()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "SendFriendRequest");
    }

    public void okay_Click_Friend()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "RemoveFromFriends");
    }

    public void okay_Click_Request()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "AcceptRequesting");
    }

    public void okay_Click_Pending()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "RemoveFromPending");
    }

    public void cancel_Click_Request()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "RemoveRequesting");
    }

    public void POP_UP_RESPONSE(string s)
    {
        switch (s)
        {
            case "OK":
                deactivatePopUp();
                break;
            case "Cancel":
                deactivatePopUp();
                break;
            case "ChangeScene":
                deactivatePopUp();
                SceneManager.LoadScene(0);
                break;
            case "SendFriendRequest":
                sendFRScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
                sendFRScript.SendMessage("sendFriendRequests",this.thisFriendRequest);
                break;
            case "RemoveFromFriends":
                sendFRScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
                sendFRScript.SendMessage("removeFromFriends", this.thisFriendRequest);
                break;
            case "AcceptRequesting":
                sendFRScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
                sendFRScript.SendMessage("acceptFromRequesting", this.thisFriendRequest);
                break;
            case "RemoveFromPending":
                sendFRScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
                sendFRScript.SendMessage("removeFromPending", this.thisFriendRequest);
                break;
            case "RemoveRequesting":
                sendFRScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
                sendFRScript.SendMessage("removeFromRequesting", this.thisFriendRequest);
                break;
            default:
                break;
        }
    }
}
