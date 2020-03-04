using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class testRegHandler : MonoBehaviour
{
    public InputField email;
    public InputField username;
    public InputField password0;
    public InputField password1;

    private requestHandler registerReq;

    public void registerHandler()
    {
        if (checkInputs())
        {
            //Debug.Log("check thread here");
            registerReq = new requestHandler(username.text, email.text, password0.text);
            
        }
    }

    private bool checkInputs()
    {
        if (checkEmail() && checkUsername() && checkPasswords())
        {
            Debug.Log("Registration Successful part0 init");
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
            Debug.Log("email field is empty!");
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
            Debug.Log("username field is empty!");
            return false;
        }
    }

    private bool checkPasswords()
    {
        if (!string.IsNullOrEmpty(password0.text))
        {
            if (!string.IsNullOrEmpty(password1.text) && password0.text == password1.text)
            {
                return true;
            }
            else
            {
                Debug.Log("passwords do not match or empty field");
                return false;
            }
        }
        else
        {
            Debug.Log("password field is empty!");
            return false;
        }
    }
}
