using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Room
{
    public GameObject model;
    public int cat = -1;
    public int hex = -1;
    public int rot = -1;
    public bool visible = false;
}

public class MasterARScript : MonoBehaviour
{
    Quaternion[] quats = {Quaternion.LookRotation(Vector3.right,Vector3.up),
                          Quaternion.LookRotation(Vector3.forward,Vector3.up),
                          Quaternion.LookRotation(Vector3.left,Vector3.up),
                          Quaternion.LookRotation(Vector3.back,Vector3.up)};
    GameObject[] meshes;
    List<Room> map;
    const float scale = 0.002f;
    const float offset = 0.315f;
    const int mapSize = 128;
    const int descriptorSize = 8;
    const int numOfIcons = 2;
    const float baseX = 5 * -offset;
    const float baseY = 2 * -offset;

    int mapPosX;
    int mapPosY;

    int icoPosX;
    int icoPosY;

    int selectedCellX;
    int selectedCellY;

    int selectedIcon;

    int interval;

    [SerializeField]
    GameObject pointer;


    // Start is called before the first frame update
    void Start()
    {
        map = new List<Room>();
        loadMeshes();
        if (PlayerPrefs.HasKey("TempLevel"))
        {
            loadLevel(PlayerPrefs.GetString("TempLevel"));
            displayLevel();
        }
        selectedCellX = 0;
        selectedCellY = 0;
        selectedIcon = 0;
        interval = 0;
        mapPosX = 0;
        mapPosY = 0;

        icoPosX = 0;
        icoPosY = 0;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) == true)
        {
            Debug.Log("A");
            if(selectedIcon == 1)
            {
                Light light = map[selectedCellX * mapSize + selectedCellY].model.GetComponent<Light>();
                if (light) Debug.Log("Found Light");
                light.enabled = !light.enabled;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) == true) Debug.Log("C");

        if (Input.GetKeyDown(KeyCode.JoystickButton3) == true) Debug.Log("B");
        if (Input.GetKeyDown(KeyCode.JoystickButton4) == true) Debug.Log("D");

        if (Input.GetKeyDown(KeyCode.JoystickButton7) == true)
        {
            Debug.Log("T1");
            ToggleActiveIcon(1);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) == true)
        {
            Debug.Log("T2");
            ToggleActiveIcon(-1);
        }
    }

    void FixedUpdate()
    {
        if (interval >= 0) interval--; 
        float horizontal = Input.GetAxis("Axis 1");
        int digitalH = AnalogToDigital(horizontal);

        float vertical = -Input.GetAxis("Axis 2");
        int digitalV = AnalogToDigital(vertical);

        if (selectedIcon == 0 && interval < 0 && (digitalH != 0 || digitalV != 0))
        {
            interval = 10;
            //Debug.Log("Horizontal " + horizontal);
            mapPosX += digitalH;
            mapPosY += digitalV;

            Debug.Log("MapX: " + mapPosX + "MapY: " + mapPosY);
            //transform.localPosition += new Vector3(analogH * offset, 0, 0);
            //transform.localPosition += new Vector3(0, 0, -analogV * offset);

            transform.localPosition = new Vector3(baseX + offset * mapPosX, -offset, baseY + offset * mapPosY);
        }
        else if (selectedIcon == 1 && interval < 0 && (digitalH != 0 || digitalV != 0))
        {
            interval = 10;
            //Debug.Log("Horizontal " + horizontal);

            icoPosX += digitalH;
            icoPosY += digitalV;

            Debug.Log("IcoX: " + icoPosX + "IcoY: " + icoPosY);
            pointer.transform.localPosition = new Vector3(baseX + offset * icoPosX, 0.5f, baseY + offset * icoPosY);
        }
        
        else if (false) //Not implimented yet
        {
            float speed = 5.0f;
            if (Mathf.Abs(horizontal) > 0.01)
            {
                //move in the direction of the camera
                //transform.position = transform.position + Camera.main.transform.right * -horizontal * speed * Time.deltaTime;
                transform.localPosition += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
                //readout.text = vertical.ToString();
            }
            if (Mathf.Abs(vertical) > 0.01)
            {
                //strafe sideways
                transform.localPosition += new Vector3(0, 0, -vertical * speed * Time.deltaTime);
                //transform.position = transform.position + Camera.main.transform.forward * -vertical * speed * Time.deltaTime;
            }
        }

        selectedCellX = icoPosX - mapPosX;
        selectedCellY = icoPosY - mapPosY;
    }

    private void loadLevel(string level)
    {
        int stringMarker = 0;
        for (int i = 0; i < mapSize * mapSize; ++i)
        {
            char c = level[stringMarker];
            if (c == 'N')
            {
                stringMarker++;
                map.Add(new Room());
            }
            else
            {
                Room room = new Room();
                int x = i / mapSize;
                int y = i % mapSize;
                string descriptor = level.Substring(stringMarker, descriptorSize);
                stringMarker += descriptorSize;
                //Debug.Log(descriptor);
                string selCatHex = descriptor.Substring(0, 3);
                room.cat = int.Parse(selCatHex, System.Globalization.NumberStyles.HexNumber);
                 
                string selHex = descriptor.Substring(3, 3);
                room.hex = int.Parse(selHex, System.Globalization.NumberStyles.HexNumber);
                room.rot = int.Parse(descriptor.Substring(6, 1));
                map.Add(room);
                //rot+=1;
                //Debug.Log("Hex: " + selHex + " Int: " + selInt + " Rot: " + rot);
            }
        }
    }

    private void displayLevel()
    {
        for(int x = 0; x < mapSize; ++x)
        {
            for(int y = 0; y < mapSize; ++y)
            {
                Room room = map[y * mapSize + x];
                if(room.cat >= 0)
                {
                    GameObject go = Instantiate(meshes[room.hex], new Vector3(y * offset, 0, x * offset), quats[room.rot], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    Light light = go.AddComponent<Light>();
                    light.intensity = 20.0f;
                    light.range = offset;
                    light.transform.localPosition += new Vector3(0,offset,0);
                    room.model = go;
                }
            }
        }
        transform.localPosition = new Vector3(baseX, -offset, baseY);
        pointer.transform.localPosition = new Vector3(baseX, 0.5f, baseY);
    }

    private void loadMeshes()
    {
        meshes = Resources.LoadAll<GameObject>("Sets/Dungeon") as GameObject[];
        foreach(GameObject mesh in meshes)
        {
            Debug.Log(mesh.name);
        }
        Debug.Log("Found " + meshes.Length + " objs");
    }

    private int AnalogToDigital(float inp)
    {
        int analog = 0;
        if (inp > 0.1) analog = 1;
        else if (inp < -0.1) analog = -1;
        return analog;
    }

    private void ToggleActiveIcon(int toggledir)
    {
        //icons[curIcon].SetActive(false);

        selectedIcon += toggledir;
        if (selectedIcon >= numOfIcons) selectedIcon = 0;
        else if (selectedIcon < 0) selectedIcon = numOfIcons - 1;

        //icons[curIcon].SetActive(true);
        pointer.SendMessage("SetIcon", selectedIcon);
    }
}
