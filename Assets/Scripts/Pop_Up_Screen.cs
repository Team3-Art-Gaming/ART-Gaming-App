using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pop_Up_Screen : MonoBehaviour
{
    public Text textbox;
    private GameObject caller;
    private Button ok_btn, c_btn;
    private int scene = 0;

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
        this.caller = caller;
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

    public void setOkButton(string s)
    {
        switch (s)
        {
            case "ChangeScene":
                ok_btn.onClick.RemoveListener(okay_Click);
                ok_btn.onClick.AddListener(okay_Click_Scene);
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
            default:
                break;
        }
    }
}
