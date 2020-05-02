using UnityEngine;
using UnityEngine.SceneManagement;

public class navControllerTester : MonoBehaviour
{
    public void sceneLoader(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
