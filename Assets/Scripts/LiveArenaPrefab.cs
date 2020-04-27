using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LiveArenaPrefab : MonoBehaviour
{
    [SerializeField]
    Button la_OK;
    [SerializeField]
    Button la_Cancel;
    // Start is called before the first frame update

    public void activateLiveArena()
    {
        transform.position = new Vector3(540, 960, 0);
    }

    public void deactivateLiveArena()
    {
        transform.position = new Vector3(-10000, -10000, 0);
    }

    public void ok_Click()
    {
        SceneManager.LoadScene(8);
    }

    public void cancel_Click()
    {
        this.deactivateLiveArena();
    }
}
