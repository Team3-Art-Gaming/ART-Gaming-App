﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;


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

    const float scale = 0.002f;
    const float offset = 0.315f;
    const float floor = 0.05f;

    const int descriptorSize = 8;

    const float baseX = 5 * -offset;
    const float baseY = 2 * -offset;
    const float speed = 1.25f;

    const int mapSizeX = 5;
    const int mapSizeY = 10;

    const char parseChar = '%';

    private Sprite[] heroSprites;
    private Sprite[] monsterSprites;
    static readonly int[] monsters = { 0, 4, 5, 19, 23, 24, 25, 26, 28, 30, 46, 52 };

    Color[] colors = { Color.black, Color.white };

    string hostMapString;
    string guestMapString;
    List<string> heroStrings;
    List<string> enemyStrings;

    string mapString;

    int myIndexNum;
    bool rotatePlayerEnabled;

    [SerializeField]
    Transform entityHolder;
    [SerializeField]
    SpriteRenderer monster;
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    void Start()
    {
        mapString = "";
        map = new List<Room>();
        entities = new List<Entity>();
        heroes = new List<Entity>();
        heroStrings = new List<string>();
        enemyStrings = new List<string>();
        meshes = Resources.LoadAll<GameObject>("Sets/Dungeon") as GameObject[];
        heroSprites = Resources.LoadAll<Sprite>("Characters/AR_Heroes");
        monsterSprites = Resources.LoadAll<Sprite>("Characters/AR_Monsters");

        myIndexNum = 0;
        rotatePlayerEnabled = false;

        RefreshAR();
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.JoystickButton3) && Input.GetKey(KeyCode.JoystickButton4))
            ||(Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.V)))
        {
            //END SESSION AND RETURN TO HOME SCREEN
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Z)) //Activate
        {
            Debug.Log("A");
            rotatePlayerEnabled = !rotatePlayerEnabled;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.X)) //Change Control
        {
            Debug.Log("C");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.C)) //Get Latest Info
        {
            Debug.Log("B");
            RefreshAR();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.V)) //Push Latest Info
        {
            Debug.Log("D");

            string CurrentSession = PlayerPrefs.GetString("CurrentSession");
            string concat = EntityToString(heroes[myIndexNum]);
            FirebaseDatabase.DefaultInstance.GetReference("/ActiveGames/" + CurrentSession + "/Heroes").Child(myIndexNum.ToString()).SetValueAsync(concat);

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Q)) //Toggle Input Type
        {
            Debug.Log("T1");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.E)) //Toggle Monster
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) digitalV = 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) digitalH = -1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) digitalV = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) digitalH = 1;

        if ((digitalH != 0 || digitalV != 0))
        {
            if (rotatePlayerEnabled)
            {
                heroes[myIndexNum].sr.transform.Rotate(new Vector3(0, 0, digitalH * speed));
            }
            else
            {
                Vector3 proposedMove = heroes[myIndexNum].sr.transform.localPosition;
                proposedMove += new Vector3(digitalH * speed * Time.deltaTime, 0, 0);
                proposedMove += new Vector3(0, 0, digitalV * speed * Time.deltaTime);
                if (isInBounds(proposedMove)) heroes[myIndexNum].sr.transform.localPosition = proposedMove;
            }
        }
    }
    public void populateMapString()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                return "ERROR";
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    if (child.Key.ToString() == "MapString")
                    {
                        hostMapString = child.Value.ToString();
                    }
                    else if (child.Key.ToString() == "guestMap")
                    {
                        if(child.Value.ToString() != "") guestMapString = child.Value.ToString();
                    }
                }
                return "";
            }
            else
            {
                return "ERROR";
            }
        });
    }

    public void populateEntitiyStrings(string entType)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        if (entType == "Entities") enemyStrings.Clear();
        else heroStrings.Clear();
        string CurrentSession = PlayerPrefs.GetString("CurrentSession");

        FirebaseDatabase.DefaultInstance.GetReference("ActiveGames/" + CurrentSession + "/" + entType + "/").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("FAULTED");
            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                foreach (var child in snapshot.Children)
                {
                    if (entType == "Entities") enemyStrings.Add(child.Value.ToString());
                    else heroStrings.Add(child.Value.ToString());
                }
            }
            else
            {
                Debug.Log("ELSE");
            }
        });
    }

    public void RefreshAR()
    {
        populateMapString();
        populateEntitiyStrings("Entities");
        populateEntitiyStrings("Heroes");
        StartCoroutine(ProceedLoad());
    }

    IEnumerator ProceedLoad()
    {
        yield return new WaitForSeconds(1f);
        DestroyLevel();
        DestroyEntities(ref entities);
        DestroyEntities(ref heroes);

        stringToMap(guestMapString);
        displayLevel();

        foreach (string e in enemyStrings)
        {
            Entity ent = StringToEntity(e);
            entities.Add(ent);
        }
        foreach (string h in heroStrings)
        {
            Entity ent = StringToEntity(h, true);
            heroes.Add(ent);
            if (ent.owner == PlayerPrefs.GetString("Username")) myIndexNum = heroes.Count - 1;
        }
    }
    public void PushData(string path, string data)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        DBreference.Child(path).SetValueAsync(data);
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
        concat += parseChar.ToString() + ent.type;
        concat += parseChar.ToString() + ent.sr.transform.localPosition.x;
        concat += parseChar.ToString() + ent.sr.transform.localPosition.z;
        concat += parseChar.ToString() + Math.Floor(ent.sr.transform.rotation.eulerAngles.z);
        return concat;
    }

    private Entity StringToEntity(string entityString, bool isHero=false)
    {
        Entity ent = new Entity();
        string[] info = entityString.Split(parseChar);
        ent.owner = info[0];
        ent.type = Convert.ToInt32(info[1]);
        ent.startX = Convert.ToSingle(info[2]);
        ent.startZ = Convert.ToSingle(info[3]);
        ent.startRot = Convert.ToInt32(info[4]);
        ent.sr = Instantiate<SpriteRenderer>(monster, entityHolder.transform);
        if(isHero) ent.sr.sprite = heroSprites[ent.type];
        else ent.sr.sprite = monsterSprites[monsters[ent.type]];
        ent.sr.transform.localPosition = new Vector3(ent.startX, floor, ent.startZ);
        ent.sr.transform.localRotation = Quaternion.Euler(90,0, ent.startRot);
        ent.collider = ent.sr.GetComponent<Collider>();
        if (ent.owner == "NONE")
        {
            ent.collider.enabled = false;
            ent.sr.enabled = false;
        }
        return ent;
    }

    private void displayLevel()
    {
        for (int x = 0; x < mapSizeX; ++x)
        {
            for (int y = 0; y < mapSizeY; ++y)
            {
                Room room = map[y * mapSizeX + x];
                if (room.cat >= 0 && room.visible)
                {
                    GameObject go = Instantiate(meshes[room.itm], this.transform);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    go.transform.localPosition = new Vector3(y * offset, 0, x * offset);
                    go.transform.localRotation = quats[room.rot];
                    room.model = go;
                }
            }
        }
        transform.localPosition = new Vector3(baseX, 0, baseY);
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
        int digital = 0;
        if (inp > 0.1) digital = 1;
        else if (inp < -0.1) digital = -1;
        return digital;
    }
}
