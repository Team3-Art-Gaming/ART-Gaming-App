using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class MapButtonGenerator : MonoBehaviour
{
    [SerializeField]
    private Button mapBTNPrefab;
    private int mapSize = 100;
    private GridLayoutGroup layout;
    //private List<Button> buttons;

    // Start is called before the first frame update
    void Awake()
    {
        layout = GetComponent<GridLayoutGroup>();
        layout.constraintCount = mapSize;
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)
            {
                Button b = Instantiate(mapBTNPrefab);
                b.transform.SetParent(this.transform);
                b.name = (i * mapSize + j).ToString();
                //buttons.Add(b);
            }
        }
        layout.transform.position = Vector3.zero;
    }
}
