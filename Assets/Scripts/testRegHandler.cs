using UnityEngine;
using UnityEngine.UI;

public class testRegHandler : MonoBehaviour
{
    public InputField email;
    public InputField username;
    public InputField password0;
    public InputField password1;

    public GameObject popupprefab;
    public GameObject popUp;
    public GameObject parent;

    private requestHandler registerReq;

    
    void Start()
    {
        popUp = Instantiate(popupprefab, new Vector3(540,960,0), Quaternion.identity, parent.transform);
        popUp.SendMessage("deactivatePopUp");
        popUp.SendMessage("setPrefab", popUp);
    }

    public void registerHandler()
    {
        
        if (checkInputs())
        {
            registerReq = new requestHandler();
            registerReq.authRegister(username.text, email.text, password0.text);
        }
    }

    private bool checkInputs()
    {
        if (checkEmail() && checkUsername() && checkPasswords())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkEmail()
    {
        if (!string.IsNullOrEmpty(email.text))
        {
            return true;
        }
        else
        {
            Debug.Log("Register: email field is empty!");
            popUp.SendMessage("activatePopUp", "Email field is empty!");
            return false;
        }
    }

    private bool checkUsername()
    {
        if (!string.IsNullOrEmpty(username.text))
        {
            return true;
        }
        else
        {
            Debug.Log("Register: username field is empty!");
            popUp.SendMessage("activatePopUp", "Username field is empty!");
            return false;
        }
    }

    private bool checkPasswords()
    {
        if (!string.IsNullOrEmpty(password0.text) && !string.IsNullOrEmpty(password1.text))
        {
            if (password0.text == password1.text)
            {
                return true;
            }
            else
            {
                Debug.Log("Register: passwords do not match");
                popUp.SendMessage("activatePopUp", "Passwords DO NOT Match!");
                return false;
            }
        }
        else
        {
            Debug.Log("Register: a password field is empty!");
            popUp.SendMessage("activatePopUp", "Password field is empty!");
            return false;
        }
    }
}
