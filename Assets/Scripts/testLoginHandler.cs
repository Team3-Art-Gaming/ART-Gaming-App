using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class testLoginHandler : MonoBehaviour
{
    public InputField email;
    public InputField password;

    public void loginHandler()
    {
        if (checkInputFields())
        {
            requestHandler loginReq = new requestHandler();
            loginReq.authLogin(email.text, password.text);
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
        if (!string.IsNullOrEmpty(email.text))
        {
            return true;
        }
        else
        {
            Debug.Log("Login: email field is empty!");
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
            return false;
        }
    }
}
