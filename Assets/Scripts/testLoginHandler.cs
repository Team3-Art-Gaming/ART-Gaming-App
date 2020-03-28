using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class testLoginHandler : MonoBehaviour
{
    public InputField username;
    public InputField password;

    public GameObject popupprefab;
    public GameObject popUp;
    public GameObject parent;

    void Start()
    {
        popUp = Instantiate(popupprefab, new Vector3(540, 960, 0), Quaternion.identity, parent.transform);
        popUp.SendMessage("deactivatePopUp");
        popUp.SendMessage("setPrefab", popUp);
    }

    public void loginHandler()
    {
        if (checkInputFields())
        {
            requestHandler loginReq = new requestHandler();
            loginReq.authLogin(username.text, password.text);
        }
    }

    private bool checkInputFields()
    {
        if (checkEmailInput() && checkPasswordInput())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkEmailInput()
    {
        if (!string.IsNullOrEmpty(username.text))
        {
            return true;
        }
        else
        {
            Debug.Log("Login: email field is empty!");
            popUp.SendMessage("activatePopUp", "Email field is empty!");
            return false;
        }
    }

    private bool checkPasswordInput()
    {
        if (!string.IsNullOrEmpty(password.text))
        {
            return true;
        }
        else
        {
            Debug.Log("Login: password field is empty!");
            popUp.SendMessage("activatePopUp", "Password field is empty!");
            return false;
        }
    }
}
