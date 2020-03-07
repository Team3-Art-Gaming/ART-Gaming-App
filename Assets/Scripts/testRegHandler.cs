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
            registerReq = new requestHandler();
            registerReq.authRegister(username.text, email.text, password0.text);
            
        }
    }

    private bool checkInputs()
    {
        if (checkEmail() && checkUsername() && checkPasswords())
        {
            //Debug.Log("Registration Successful part0 init");
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
                return false;
            }
        }
        else
        {
            Debug.Log("Register: a password field is empty!");
            return false;
        }
    }
}
