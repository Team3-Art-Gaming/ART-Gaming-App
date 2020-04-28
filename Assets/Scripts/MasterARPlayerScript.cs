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

public class MasterARPlayerScript : MonoBehaviour
{

    Quaternion[] quats = {Quaternion.LookRotation(Vector3.right,Vector3.up),
                          Quaternion.LookRotation(Vector3.forward,Vector3.up),
                          Quaternion.LookRotation(Vector3.left,Vector3.up),
                          Quaternion.LookRotation(Vector3.back,Vector3.up),
                          Quaternion.LookRotation(Vector3.up,Vector3.forward)};


    GameObject[] meshes;
    List<Room> map;
    List<Entity> entities;
    List<Entity> heroes;

    int myIndexNum;

    const float scale = 0.002f;
    const float offset = 0.315f;
    const float floor = 0.05f;
    const float pointerHeight = 0.32f;
    
    const int descriptorSize = 8;
    
    const float baseX = 5 * -offset;
    const float baseY = 2 * -offset;
    const float speed = 1.25f;

    private Sprite[] heroSprites;
    private Sprite[] monsterSprites;
    static readonly int[] monsters = {0,4,5,19,23,24,25,26,28,30,46,52};
    
    Color[] colors = { Color.black, Color.white };

    string mapString;

    int mapSizeX;
    int mapSizeY;

    bool rotatePlayerEnabled;

    [SerializeField]
    Transform entityHolder;
    [SerializeField]
    SpriteRenderer monster;

    string debugGuestString = "0030020100300231NNN00300211003000010030023100300301NN0030021000300A0000300A0000300B10NN0030010000300B0000300400NN0030010000300B0000300410NN0030010000300B00NNN00300C3000300C00NNNNNNNNNNNNNNNN";
    string[] debugEntityStrings = { "0t0x-0.9750006z0.06999996r354", "0t1x-0.275001z0.295r354" };
    string[] debugHeroStrings = { "0t5x0z0r0", "1t3x1z1r90" };

    void Start()
    {
        mapString = "";

        map = new List<Room>();
        entities = new List<Entity>();
        heroes = new List<Entity>();
        meshes = Resources.LoadAll<GameObject>("Sets/Dungeon") as GameObject[];
        heroSprites = Resources.LoadAll<Sprite>("Characters/AR_Heroes");
        monsterSprites = Resources.LoadAll<Sprite>("Characters/AR_Monsters");

        mapSizeX = 5;
        mapSizeY = 10;

        rotatePlayerEnabled = false;

        transform.localPosition = new Vector3(baseX, 0.0f, baseY);

        StartCoroutine(checkDB());
    }

    IEnumerator checkDB()
    {
        while(true)
        {
            Debug.Log("In While Loop");
            RefreshAR();
            yield return new WaitForSeconds(10f);
        }
    }

    public void RefreshAR()
    {
        //mapString = GetGuestMapString();
        //DEBUG
        mapString = debugGuestString;
        List<string> ents = GetEntitiesString("Entities");
        List<string> hero = GetEntitiesString("Heroes");

        StartCoroutine(LoadData(ents, hero));
    }
    IEnumerator LoadData(List<string> ents,List<string>hero)
    {
        Debug.Log("Entered LoadData");
        yield return new WaitForSeconds(1f);
        Debug.Log("Waited 1 second");

        DestroyLevel();
        DestroyEntities(ref entities);
        DestroyEntities(ref heroes);

        mapString = debugGuestString; //DEBUG
        stringToMap(mapString);
        
        displayLevel();

        foreach (string e in debugEntityStrings)  //DEBUG
        {
            Entity ent = StringToEntity(e);
            entities.Add(ent);
        }
        foreach (string h in debugHeroStrings)  //DEBUG
        {
            Entity ent = StringToEntity(h,true);
            heroes.Add(ent);
        }
    }

    public List<string> GetEntitiesString(string entType)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        List<string> Entities = new List<string>();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/" + entType +"/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                //Debug.Log("BLARG");
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
                //Debug.Log("ELSE");
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

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/guestMap/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                //Debug.Log("BLARG");
                return null;
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                string data = snapshot.ToString();

                GuestMap.Add(data);

                return GuestMap[0];
            }
            else
            {
                //Debug.Log("ELSE");
                return null;
            }
        });

        int milliseconds = 2000;
        Thread.Sleep(milliseconds);
        return GuestMap[0];
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
            rotatePlayerEnabled = !rotatePlayerEnabled;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) == true)
        {
            Debug.Log("C");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3) == true)
        {
            Debug.Log("B");
            
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4) == true)
        {
            Debug.Log("D");

            string CurrentSession = PlayerPrefs.GetString("CurrentSession");

            string concat = EntityToString(heroes[myIndexNum]);
            FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + CurrentSession + "/Heroes").Child("entities" + myIndexNum.ToString()).SetValueAsync(concat);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton7) == true)
        {
            Debug.Log("T1");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) == true)
        {
            Debug.Log("T2");
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Axis 1");
        int digitalH = AnalogToDigital(horizontal);

        float vertical = -Input.GetAxis("Axis 2");
        int digitalV = AnalogToDigital(vertical);
     
        if ((digitalH != 0 || digitalV != 0))
        {
            if (!rotatePlayerEnabled)
            {
                Vector3 proposedMove = heroes[myIndexNum].sr.transform.localPosition;
                proposedMove += new Vector3(digitalH * speed * Time.deltaTime, 0, 0);
                proposedMove += new Vector3(0, 0, digitalV * speed * Time.deltaTime);
                if (isInBounds(proposedMove)) heroes[myIndexNum].sr.transform.localPosition = proposedMove;
            }
            else
            {
                heroes[myIndexNum].sr.transform.Rotate(new Vector3(0, 0, digitalH * speed));
            }
        }
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
                if (descriptor.Substring(7, 1) == "1") room.visible = true;
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

    private Entity StringToEntity(string entityString, bool isHero = false)
    {
        Entity ent = new Entity();
        char[] keys = { 't', 'x', 'z', 'r' };
        string[] info = entityString.Split(keys);
        ent.owner = Convert.ToInt32(info[0]);
        ent.type = Convert.ToInt32(info[1]);
        ent.startX = Convert.ToSingle(info[2]);
        ent.startZ = Convert.ToSingle(info[3]);
        ent.startRot = Convert.ToInt32(info[4]);
        
        ent.sr = Instantiate<SpriteRenderer>(monster, entityHolder.transform);
        if (isHero) ent.sr.sprite = heroSprites[monsters[ent.type]];
        else ent.sr.sprite = monsterSprites[monsters[ent.type]];
        ent.sr.transform.localPosition = new Vector3(ent.startX, floor, ent.startZ);
        ent.sr.transform.localRotation = Quaternion.Euler(0,0,0);
        ent.collider = ent.sr.GetComponent<Collider>();
        return ent;
    }



    private void displayLevel()
    {
        for(int x = 0; x < mapSizeX; ++x)
        {
            for(int y = 0; y < mapSizeY; ++y)
            {
                Room room = map[y * mapSizeX + x];
                if(room.cat >= 0 && room.visible)
                {
                    GameObject go = Instantiate(meshes[room.itm], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    go.transform.localPosition = new Vector3(y * offset, 0, x * offset);
                    go.transform.localRotation = quats[room.rot];
                    room.model = go;
                }
            }
        }
        
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

    public void DestroyEntities(ref List<Entity> ents)
    {
        if (ents.Count > 0)
        {
            foreach (Entity e in ents)
            {
                if (e.collider) Destroy(e.collider);
                if (e.sr) Destroy(e.sr);
            }
            ents.Clear();
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
}
