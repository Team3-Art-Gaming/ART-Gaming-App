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

class Entity
{
    public int type;
    public float xPos;
    public float yPos;
    public int rot;
}

public class MasterARScript : MonoBehaviour
{
    Quaternion[] quats = {Quaternion.LookRotation(Vector3.right,Vector3.up),
                          Quaternion.LookRotation(Vector3.forward,Vector3.up),
                          Quaternion.LookRotation(Vector3.left,Vector3.up),
                          Quaternion.LookRotation(Vector3.back,Vector3.up),
                          Quaternion.LookRotation(Vector3.up,Vector3.forward)};
        
    GameObject[] meshes;
    List<Room> map;
    //List<Entity> entities;
    List<SpriteRenderer> entities; 

    const float scale = 0.002f;
    const float offset = 0.315f;
    
    const int descriptorSize = 8;
    const int numOfIcons = 3;
    
    const float baseX = 5 * -offset;
    const float baseY = 2 * -offset;
    const float speed = 1.25f;

    private Sprite[] heroSprites;
    private Sprite[] monsterSprites;
    static readonly int[] heroes = { 0, 1 };
    static readonly int[] monsters = {0,4,5,19,23,24,25,26,28,30,46,52};
    
    Color[] colors = { Color.black, Color.white };

    int mapSizeX;
    int mapSizeY;

    int mapPosX;
    int mapPosY;

    int icoPosX;
    int icoPosY;

    int selectedCellX;
    int selectedCellY;

    int selectedIcon;

    int interval;

    int selectedMonster;
    int pointingAtMonster;

    [SerializeField]
    GameObject pointer;
    [SerializeField]
    SpriteRenderer monster;


    void Start()
    {
        map = new List<Room>();
        entities = new List<SpriteRenderer>();
        loadMeshes();
        heroSprites = Resources.LoadAll<Sprite>("Characters/AR_Heroes");
        monsterSprites = Resources.LoadAll<Sprite>("Characters/AR_Monsters");

        selectedCellX = 0;
        selectedCellY = 0;
        selectedIcon = 0;
        interval = 0;
        mapSizeX = 128;
        mapSizeY = 128;
        mapPosX = 0;
        mapPosY = 0;
        icoPosX = 0;
        icoPosY = 0;
        selectedMonster = 0;
        pointingAtMonster = -1;
        
        if (PlayerPrefs.HasKey("TempLevel"))
        {
            stringToMap(PlayerPrefs.GetString("TempLevel"));
            displayLevel();
        }
        pointer.SendMessage("SetMonster", monsterSprites[monsters[selectedMonster]]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) == true)
        {
            Debug.Log("A");
            if(selectedIcon == 1)
            {
                Room r = map[selectedCellX * mapSizeX + selectedCellY];
                r.visible = !r.visible;
                setMeshColor(r.model, Convert.ToInt32(r.visible));
            }
            else if(selectedIcon == 2)
            {
                Vector3 vec = new Vector3(pointer.transform.position.x, 0.05f, pointer.transform.position.z);
                SpriteRenderer sr = Instantiate<SpriteRenderer>(monster,vec, quats[4], this.transform);
                sr.sprite = monsterSprites[monsters[selectedMonster]];
                entities.Add(sr);
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) == true)
        {
            Debug.Log("C");
            if (pointingAtMonster >= 0)
            {
                if (selectedIcon == 2) selectedIcon = 3;
                else if (selectedIcon == 3) selectedIcon = 4;
                else if (selectedIcon == 4) selectedIcon = 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3) == true)
        {
            Debug.Log("B");
            
        }
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
            if(selectedIcon == 2)
            {
                selectedMonster++;
                if (selectedMonster > monsters.Length) selectedMonster = 0; 
                pointer.SendMessage("SetMonster", monsterSprites[monsters[selectedMonster]]);
            }
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

        if (selectedIcon == 0 && interval < 0 && (digitalH != 0 || digitalV != 0)) //World
        {
            interval = 10;
            //Debug.Log("Horizontal " + horizontal);
            mapPosX += digitalH;
            mapPosY += digitalV;

            if (mapPosX > 0) mapPosX = 0;
            if (mapPosY > 0) mapPosY = 0;

            // Debug.Log("MapX: " + mapPosX + "MapY: " + mapPosY);
            //transform.localPosition += new Vector3(analogH * offset, 0, 0);
            //transform.localPosition += new Vector3(0, 0, -analogV * offset);

            transform.localPosition = new Vector3(baseX + offset * mapPosX, 0, baseY + offset * mapPosY);
        }
        else if (selectedIcon == 1 && interval < 0 && (digitalH != 0 || digitalV != 0)) //Light
        {
            interval = 10;
            //Debug.Log("Horizontal " + horizontal);

            icoPosX += digitalH;
            icoPosY += digitalV;

            //Debug.Log("IcoX: " + icoPosX + "IcoY: " + icoPosY);
            pointer.transform.localPosition = new Vector3(baseX + offset * icoPosX, 0.32f, baseY + offset * icoPosY);
        }
        else if (selectedIcon >= 2)  //Monster
        {
            Vector3 proposedMove = pointer.transform.localPosition;
            proposedMove += new Vector3(digitalH * speed * Time.deltaTime, 0, 0);
            proposedMove += new Vector3(0, 0, digitalV * speed * Time.deltaTime);
            if(isInBounds(proposedMove) && selectedIcon != 4) pointer.transform.localPosition = proposedMove;
            if (selectedIcon == 2)
            {
                //Debug.Log("Moving");
                //pointer.transform.localPosition = proposedMove;
                int count = 0;
                bool foundMonster = false;
                foreach (SpriteRenderer entity in entities)
                {
                    Collider collider = entity.GetComponent<Collider>();
                    proposedMove.y = 0.1f;
                    Debug.Log(collider.bounds);
                    Debug.Log(proposedMove);
                    if (collider.bounds.Contains(proposedMove))
                    {
                        Debug.Log("Colliding");
                        highlighEntity(count);
                        pointingAtMonster = count;
                        foundMonster = true;
                        break;
                    }
                    count++;
                }
                if (!foundMonster)
                {
                    highlighEntity(-1);
                    pointingAtMonster = -1;
                }
            }
            else if (selectedIcon == 3)
            {
                Debug.Log("in3");
                entities[pointingAtMonster].transform.position = new Vector3(proposedMove.x, 0.1f, proposedMove.z);
            }
            else if(selectedIcon == 4)
            {
                entities[pointingAtMonster].transform.Rotate(new Vector3(0, 0, digitalH*speed));
                Debug.Log(entities[pointingAtMonster].transform.rotation.eulerAngles.y);
            }
        }
        selectedCellX = icoPosX - mapPosX;
        selectedCellY = icoPosY - mapPosY;
    }

    private void highlighEntity(int ent)
    {
        if(pointingAtMonster >= 0) entities[pointingAtMonster].color = Color.white;
        if(ent >= 0) entities[ent].color = Color.red;
    }

    private bool isInBounds(Vector3 vec)
    {
        if (vec.x > -1.7f && vec.x < 1.4f && vec.z > -0.8f && vec.z < 0.8f) return true;
        return false;
    }

    private void stringToMap(string level)
    {
        int stringMarker = 0;
        for (int i = 0; i < mapSizeX * mapSizeY; ++i)
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
                int x = i / mapSizeX;
                int y = i % mapSizeX;
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
        string mapString = "";

        for (int x = startX; x < startX + lengthX; ++x)
        {
            for (int y = startY; y < startY + lengthY; ++y)
            {
                int r = x * mapSizeX + y;
                if (map[r] != null && map[r].cat >= 0)
                {
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
        for (int i = 0; i < mapSizeX * mapSizeY; ++i)
        {
            if (map[i].cat >= 0) setMeshColor(map[i].model, 0);
        }
        int baseX = Math.Abs(mapPosX);
        int baseY = Math.Abs(mapPosY);
        int bottomCorner = baseX * mapSizeX + baseY;
        GameObject bo = map[bottomCorner].model;

        for (int x = baseX; x < baseX + 10; ++x)
        {
            for (int y = baseY; y < baseY + 5; ++y)
            {
                int r = x * mapSizeX + y;
                if (map[r] != null && map[r].cat >= 0) setMeshColor(map[r].model, 1);
            }
        }
    }

    private void displayLevel()
    {
        for(int x = 0; x < mapSizeX; ++x)
        {
            for(int y = 0; y < mapSizeY; ++y)
            {
                Room room = map[y * mapSizeX + x];
                if(room.cat >= 0)
                {
                    GameObject go = Instantiate(meshes[room.itm], new Vector3(y * offset, 0, x * offset), quats[room.rot], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    setMeshColor(go, Convert.ToInt32(room.visible));
                    room.model = go;
                }
            }
        }
        transform.localPosition = new Vector3(baseX, 0, baseY);
        pointer.transform.localPosition = new Vector3(baseX, 0.32f, baseY);
    }

    private void setMeshColor(GameObject go, int color)
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
