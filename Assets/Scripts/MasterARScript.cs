using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Room
{
    public GameObject model;
    public int cat = -1;
    public int itm = -1;
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
    const int numOfIcons = 3;
    const float baseX = 5 * -offset;
    const float baseY = 2 * -offset;
    const float speed = 2.0f;
    Color[] colors = { Color.black, Color.white };

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
            stringToMap(PlayerPrefs.GetString("TempLevel"));
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
                Room r = map[selectedCellX * mapSize + selectedCellY];
                r.visible = !r.visible;
                setAllMeshesColor(r.model, Convert.ToInt32(r.visible));
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) == true) Debug.Log("C");

        if (Input.GetKeyDown(KeyCode.JoystickButton3) == true) Debug.Log("B");
        if (Input.GetKeyDown(KeyCode.JoystickButton4) == true) 
        {
            Debug.Log("D");
            mapToString(Math.Abs(mapPosX), Math.Abs(mapPosY), 10, 5);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton7) == true)
        {
            Debug.Log("T1");
            ToggleActiveIcon(1);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) == true)
        {
            //Debug.Log("T2");
            //ToggleActiveIcon(-1);
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

            if (mapPosX > 0) mapPosX = 0;
            if (mapPosY > 0) mapPosY = 0;

            Debug.Log("MapX: " + mapPosX + "MapY: " + mapPosY);
            //transform.localPosition += new Vector3(analogH * offset, 0, 0);
            //transform.localPosition += new Vector3(0, 0, -analogV * offset);

            transform.localPosition = new Vector3(baseX + offset * mapPosX, -0.16f, baseY + offset * mapPosY);
        }
        else if (selectedIcon == 1 && interval < 0 && (digitalH != 0 || digitalV != 0))
        {
            interval = 10;
            //Debug.Log("Horizontal " + horizontal);

            icoPosX += digitalH;
            icoPosY += digitalV;

            Debug.Log("IcoX: " + icoPosX + "IcoY: " + icoPosY);
            pointer.transform.localPosition = new Vector3(baseX + offset * icoPosX, 0.0f, baseY + offset * icoPosY);
        }
        else if(selectedIcon == 2)
        {  
            Vector3 proposedMove = pointer.transform.localPosition;
            proposedMove += new Vector3(digitalH * speed * Time.deltaTime, 0, 0);
            proposedMove += new Vector3(0, 0, digitalV * speed * Time.deltaTime);

            Debug.Log(proposedMove);
            if (proposedMove.x > -1.7f && proposedMove.x < 1.4f && proposedMove.z > -0.8f && proposedMove.z < 0.8f)
            {
                //Debug.Log("Moving");
                pointer.transform.localPosition = proposedMove;
            }
            
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

    private void stringToMap(string level)
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
                room.itm = int.Parse(selHex, System.Globalization.NumberStyles.HexNumber);
                room.rot = int.Parse(descriptor.Substring(6, 1));
                map.Add(room);
                //rot+=1;
                //Debug.Log("Hex: " + selHex + " Int: " + selInt + " Rot: " + rot);
            }
        }
    }

    private string mapToString(int startX, int startY, int lengthX, int lengthY)
    {
        //for (int i = 0; i < mapSize * mapSize; ++i)
        //{
        //    if (map[i].cat >= 0) setAllMeshesColor(map[i].model, 0);
        //}


        string mapString = "";

        for (int x = startX; x < startX + lengthX; ++x)
        {
            for (int y = startY; y < startY + lengthY; ++y)
            {
                int r = x * mapSize + y;
                if (map[r] != null && map[r].cat >= 0)
                {
                    //mapString += "code";
                    //setAllMeshesColor(map[r].model, 1);
                    mapString += map[r].cat.ToString("X3");
                    mapString += map[r].itm.ToString("X3");
                    mapString += map[r].rot.ToString();
                    if (map[r].visible) mapString += "1";
                    else mapString += "0";
                }
                else
                {
                    mapString += "N";
                }
            }
        }
        Debug.Log(mapString);
        return mapString;
    }

    private void getAllUnderFrame()
    {
        for (int i = 0; i < mapSize * mapSize; ++i)
        {
            if (map[i].cat >= 0) setAllMeshesColor(map[i].model, 0);
        }
        int baseX = Math.Abs(mapPosX);
        int baseY = Math.Abs(mapPosY);
        int bottomCorner = baseX * mapSize + baseY;
        GameObject bo = map[bottomCorner].model;

        for (int x = baseX; x < baseX + 10; ++x)
        {
            for (int y = baseY; y < baseY + 5; ++y)
            {
                int r = x * mapSize + y;
                if (map[r] != null && map[r].cat >= 0) setAllMeshesColor(map[r].model, 1);
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
                    GameObject go = Instantiate(meshes[room.itm], new Vector3(y * offset, 0, x * offset), quats[room.rot], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    setAllMeshesColor(go, Convert.ToInt32(room.visible));
                    room.model = go;
                }
            }
        }
        transform.localPosition = new Vector3(baseX, -offset, baseY);
        pointer.transform.localPosition = new Vector3(baseX, 0.0f, baseY);
    }

    private void setAllMeshesColor(GameObject go, int color)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform g = go.transform.GetChild(i);
            MeshRenderer mr = g.GetComponent<MeshRenderer>();
            mr.material.SetColor("_Color", colors[color]);
        }
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
