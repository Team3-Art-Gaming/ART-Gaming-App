using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launch_Screen : MonoBehaviour
{
    [SerializeField]
    Button Home;
    [SerializeField]
    Button Launch;
    [SerializeField]
    Image Grid; 
    [SerializeField] 
    Image Grid_Prefab;

    public List<Image> Prefab;
    public List<Friends> friends;
    
    // Start is called before the first frame update
    void Start()
    {
        this.friends.Add(new Friends("friend1","Friend"));
        this.friends.Add(new Friends("friend2", "Friend"));
        this.friends.Add(new Friends("friend3", "Friend"));

        foreach (Friends f in friends)
        {
            Image I = Instantiate(Grid_Prefab, Grid.transform); 
            //I.SendMessage("SetName", friend);
            Prefab.Add(I);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
