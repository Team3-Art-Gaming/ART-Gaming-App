using UnityEngine;
using UnityEngine.UI;

public class SelectorBTNBehaviourScript : MonoBehaviour
{
    private MasterBehaviourScript master;
    void Start()
    {
        master = GetComponentInParent<MasterBehaviourScript>();
    }

    public void OnClick()
    {
        master.SetSelectors(int.Parse(name));
    }

    public void SelectionChanged(bool active)
    {
        if(active)
        {
            this.GetComponent<Image>().color = Color.red;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
        }
    }
}
