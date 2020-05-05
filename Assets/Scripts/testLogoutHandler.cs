using UnityEngine;
using UnityEngine.SceneManagement;

public class testLogoutHandler : MonoBehaviour
{

    public void authSignOut()
    { 
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.DefaultInstance);
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        Debug.LogFormat("User: " + PlayerPrefs.GetString("Username") + " signing out");
        auth.SignOut();
        PlayerPrefs.SetString("Username", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
    
}
