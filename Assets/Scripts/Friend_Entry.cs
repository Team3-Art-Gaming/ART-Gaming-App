using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Friend_Entry : MonoBehaviour
{
    [SerializeField] 
    Text Friend_Name;
    [SerializeField] 
    Image Friend_Icon;
    [SerializeField] 
    Button Send_Button;

    public Button okBtn, cBtn;

    private string name;
    private string status;
    private Friend_List FLscript;
    private CreateUserFB cufbScript;

    public void SetName(Friends F)
    {
        Friend_Name.text = F.Name;
        this.name = F.Name;
        this.status = F.Status;
        if (this.status == "Friend")
        {
            cBtn.onClick.AddListener(c_btn_friend);
        }
        else if (this.status == "Requesting")
        {
            okBtn.onClick.AddListener(ok_btn_request);
            cBtn.onClick.AddListener(c_btn_request);
        }
        else if (this.status == "Pending")
        {
            cBtn.onClick.AddListener(c_btn_pending);
        }
    }

    public string GetName()
    {
        return this.name;
    }

    public string GetStat()
    {
        return this.status;
    }

    public void SetIcon()
    {
        
    }

    public Image GetIcon()
    {
        return Friend_Icon;
    }

    public void SetButton()
    {
        
    }

    public Button GetButton()
    {
        return Send_Button;
    }

    private void ok_btn_friend()
    {
        Debug.Log("Friend OK");
    }

    private void c_btn_friend()
    {
        cufbScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        cufbScript.deleteFriend(this.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ok_btn_request()
    {
        cufbScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        cufbScript.setFriend(this.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void c_btn_request()
    {
        cufbScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        cufbScript.setFriend(this.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ok_btn_pending()
    {
        Debug.Log("Pending OK");
    }

    private void c_btn_pending()
    {
        cufbScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        cufbScript.deleteFriend(this.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
