using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading;


class Room
{
    public GameObject model;
    public int cat = -1;
    public int itm = -1;
    public int rot = -1;
    public float xPos = 0.0f;
    public float yPos = 0.0f; 
    public bool visible = false;
}

class Entity
{
    public int owner;
    public int type;
    public float startX;
    public float startZ;
    public int startRot;
    public Collider collider;
    public SpriteRenderer sr;
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
    List<Entity> entities; 

    const float scale = 0.002f;
    const float offset = 0.315f;
    const float floor = 0.05f;
    const float pointerHeight = 0.32f;
    
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

    string mapString;

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
    Transform target;
    Vector3 targetPos;
    Quaternion targetRot;

    [SerializeField]
    GameObject pointer;
    Collider pointerCollider;
    [SerializeField]
    Transform entityHolder;
    [SerializeField]
    SpriteRenderer monster;


    //string debugGuestString = "0030020000300230NNN00300210003000000030023000300300NN0030021000300A0000300A0000300B10NN0030010000300B0000300400NN0030010000300B0000300410NN0030010000300B00NNN00300C3000300C00NNNNNNNNNNNNNNNN";
    //string[] debugEntityStrings = { "0t0x-0.9750006z0.06999996r354", "0t1x-0.275001z0.295r354" };

    void Start()
    {
        mapString = "";
        map = new List<Room>();
        entities = new List<Entity>();
        meshes = Resources.LoadAll<GameObject>("Sets/Dungeon") as GameObject[];
        heroSprites = Resources.LoadAll<Sprite>("Characters/AR_Heroes");
        monsterSprites = Resources.LoadAll<Sprite>("Characters/AR_Monsters");
        pointerCollider = GameObject.Find("PointerStick").GetComponent<Collider>();


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

        pointer.SendMessage("SetMonster", monsterSprites[monsters[selectedMonster]]);
        StartCoroutine(checkDB());
    }

    IEnumerator checkDB()
    {
        while(true)
        {
            //SET mapString
            //SET entities
            DestroyLevel();
            DestroyEntities();
            mapString = GetHostMapString();
            List<string> ents = GetEntitiesString();
            FoundTarget();
            foreach (string e in ents)
            {
                StringToEntity(e);
            }
            Debug.Log("HELLO");
            yield return new WaitForSeconds(10f);
        }
    }

    public List<string> GetEntitiesString()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        List<string> Entities = new List<string>();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActivesGames/" + CurrentSession + "/Entites/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    //Debug.Log(child.Key + ": " + child.Value);

                    Entities.Add(child.Value.ToString());
                }

                return Entities;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });

        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return Entities;
    }

public string GetGuestMapString()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        List<string> GuestMap = new List<string>();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActivesGames/" + CurrentSession + "/guestMap/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                string data = snapshot.ToString();

                GuestMap.Add(data);

                return GuestMap;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });

        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return GuestMap[0];
    }

public string GetHostMapString()
	{
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        List<string> HostMap = new List<string>();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActivesGames/" + CurrentSession + "/MapString/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {
                
                DataSnapshot snapshot = task.Result;
                string data = snapshot.ToString();
                Debug.Log("TEST " + data);
                HostMap.Add(data);

                return HostMap;
            }
            else
            {
                Debug.Log("ELSE");
                return null;
            }
        });
        
        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return HostMap[0];
    }

    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
        //FirebaseDatabase.DefaultInstance.GetReference("/1Test/0Users/").Child("blip").SetValueAsync(data);
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
                Entity ent = new Entity();
                Vector3 vec = pointer.transform.localPosition;
                vec.y = floor;
                SpriteRenderer sr = Instantiate<SpriteRenderer>(monster,entityHolder.transform);
                sr.sprite = monsterSprites[monsters[selectedMonster]];
                sr.transform.localPosition = vec;
                ent.sr = sr;
                ent.collider = sr.GetComponent<Collider>();
                ent.type = selectedMonster;
                entities.Add(ent);
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

            string CurrentSession = PlayerPrefs.GetString("CurrentSession");

            string hostMap = mapToString(0, 0, mapSizeY, mapSizeX);
            Debug.Log(hostMap);


            FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + CurrentSession).Child("hostMap").SetValueAsync(hostMap);
            //FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/").Child(SessionName).SetValueAsync(SessionName);

            string guestMap = mapToString(Math.Abs(mapPosY), Math.Abs(mapPosX), 10, 5);
            Debug.Log(guestMap);

            FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + CurrentSession).Child("guestMap").SetValueAsync(guestMap);

            List<string> ents = new List<string>();
            foreach (Entity ent in entities)
            {
                string concat = EntityToString(ent);
                ents.Add(concat);
            }

            int i = 0;
            foreach (string str in ents)
			{
                FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + CurrentSession + "/Entities").Child("entities" + i.ToString()).SetValueAsync(str);
                i++;
			}
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton7) == true)
        {
            Debug.Log("T1");
            ToggleActiveIcon(1);
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) == true)
        {
            Debug.Log("T2");
            if (selectedIcon == 2)
            {
                selectedMonster++;
                if (selectedMonster > monsters.Length) selectedMonster = 0; 
                pointer.SendMessage("SetMonster", monsterSprites[monsters[selectedMonster]]);
            }
        }
    }

    void FixedUpdate()
    {
        if (interval >= 0) interval--; 
        float horizontal = Input.GetAxis("Axis 1");
        int digitalH = AnalogToDigital(horizontal);

        float vertical = -Input.GetAxis("Axis 2");
        int digitalV = AnalogToDigital(vertical);
        if ((digitalH != 0 || digitalV != 0))
        {
            if (selectedIcon == 0 && interval < 0) //World
            {
                interval = 10;
                mapPosX += digitalH;
                mapPosY += digitalV;

                if (mapPosX > 0) mapPosX = 0;
                if (mapPosY > 0) mapPosY = 0;


                transform.localPosition = new Vector3(baseX + offset * mapPosX, 0.0f, baseY + offset * mapPosY);
            }
            else if (selectedIcon == 1 && interval < 0) //Light
            {
                interval = 10;
                icoPosX += digitalH;
                icoPosY += digitalV;
                pointer.transform.localPosition = new Vector3(baseX + offset * icoPosX, pointerHeight, baseY + offset * icoPosY);
            }
            else if (selectedIcon >= 2)  //Monster
            {
                Vector3 proposedMove = pointer.transform.localPosition;
                proposedMove += new Vector3(digitalH * speed * Time.deltaTime, 0, 0);
                proposedMove += new Vector3(0, 0, digitalV * speed * Time.deltaTime);
                if (isInBounds(proposedMove) && selectedIcon != 4) pointer.transform.localPosition = proposedMove;
                if (selectedIcon == 2)
                {
                    int count = 0;
                    bool foundMonster = false;
                    foreach (Entity entity in entities)
                    {
                        if (entity.collider.bounds.Intersects(pointerCollider.bounds))
                        {
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
                    entities[pointingAtMonster].sr.transform.localPosition = new Vector3(proposedMove.x, floor, proposedMove.z);
                }
                else if (selectedIcon == 4)
                {
                    entities[pointingAtMonster].sr.transform.Rotate(new Vector3(0, 0, digitalH * speed));
                }
            }
        }
        selectedCellX = icoPosX - mapPosX;
        selectedCellY = icoPosY - mapPosY;
    }

    public void FoundTarget()
    {
        if (PlayerPrefs.HasKey("TempLevel"))
        {
            //stringToMap(PlayerPrefs.GetString("TempLevel"));
            Debug.Log("BLAH"+mapString);
            if (mapString != "")
            {
                stringToMap(mapString);
                //mapSizeX = 5;
                //mapSizeY = 10;
                //stringToMap(debugGuestString);
                displayLevel();
                //foreach(string e in debugEntityStrings)
                //{
                //    StringToEntity(e);
                //}
            }
        }
    }

    private void highlighEntity(int ent)
    {
        if(pointingAtMonster >= 0) entities[pointingAtMonster].sr.color = Color.white;
        if(ent >= 0) entities[ent].sr.color = Color.red;
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
                room.xPos = x * offset;
                int y = i % mapSizeX;
                room.yPos = y * offset;
                string descriptor = level.Substring(stringMarker, descriptorSize);
                stringMarker += descriptorSize;
                string selCatHex = descriptor.Substring(0, 3);
                room.cat = int.Parse(selCatHex, System.Globalization.NumberStyles.HexNumber);
                 
                string selHex = descriptor.Substring(3, 3);
                room.itm = int.Parse(selHex, System.Globalization.NumberStyles.HexNumber);
                room.rot = int.Parse(descriptor.Substring(6, 1));
                map.Add(room);
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
        return mapString;
    }

    private string EntityToString(Entity ent)
    {
        string concat = "";
        concat += ent.owner;
        concat += "t" + ent.type;
        concat += "x" + ent.sr.transform.localPosition.x;
        concat += "z" + ent.sr.transform.localPosition.z;
        concat += "r" + Math.Floor(ent.sr.transform.rotation.eulerAngles.z);
        Debug.Log(concat);
        return concat;
    }

    private Entity StringToEntity(string entityString)
    {
        Entity ent = new Entity();
        char[] keys = { 't', 'x', 'z', 'r' };
        string[] info = entityString.Split(keys);
        foreach(string s in info)
        {
            Debug.Log(s);
        }
        ent.owner = Convert.ToInt32(info[0]);
        ent.type = Convert.ToInt32(info[1]);
        ent.startX = Convert.ToSingle(info[2]);
        ent.startZ = Convert.ToSingle(info[3]);
        ent.startRot = Convert.ToInt32(info[4]);
        ent.sr = Instantiate<SpriteRenderer>(monster, entityHolder.transform);
        ent.sr.sprite = monsterSprites[monsters[ent.type]];
        ent.sr.transform.localPosition = new Vector3(ent.startX, floor, ent.startZ);
        ent.sr.transform.rotation = Quaternion.Euler(0,0,ent.startRot);
        ent.collider = ent.sr.GetComponent<Collider>();
        entities.Add(ent);
        return ent;
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
                    GameObject go = Instantiate(meshes[room.itm], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    go.transform.localPosition = new Vector3(y * offset, 0, x * offset);
                    go.transform.localRotation = quats[room.rot];
                    room.model = go;
                }
            }
        }
        transform.localPosition = new Vector3(baseX + offset * mapPosX, 0, baseY + offset * mapPosY);
        pointer.transform.localPosition = new Vector3(baseX, pointerHeight, baseY);
    }

    public void DestroyLevel()
    {
        if (map.Count > 0)
        {
            foreach (Room r in map)
            {
                if (r.model) Destroy(r.model);
            }
            map.Clear();
        }
    }

    public void DestroyEntities()
    {
        if (entities.Count > 0)
        {
            foreach (Entity e in entities)
            {
                if (e.collider) Destroy(e.collider);
                if (e.sr) Destroy(e.sr);
            }
            entities.Clear();
        }
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

    private int AnalogToDigital(float inp)
    {
        int analog = 0;
        if (inp > 0.1) analog = 1;
        else if (inp < -0.1) analog = -1;
        return analog;
    }

    private void ToggleActiveIcon(int toggledir)
    {
        selectedIcon += toggledir;
        if (selectedIcon >= numOfIcons) selectedIcon = 0;
        else if (selectedIcon < 0) selectedIcon = numOfIcons - 1;
        pointer.SendMessage("SetIcon", selectedIcon);
    }
}
