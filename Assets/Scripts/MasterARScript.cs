using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterARScript : MonoBehaviour
{
    Quaternion[] quats = {Quaternion.LookRotation(Vector3.right,Vector3.up),
                          Quaternion.LookRotation(Vector3.forward,Vector3.up),
                          Quaternion.LookRotation(Vector3.left,Vector3.up),
                          Quaternion.LookRotation(Vector3.back,Vector3.up)};
    GameObject[] meshes;
    float offset = 0.315f;
    int mapSize = 128;
    private const int descriptorSize = 8;

    // Start is called before the first frame update
    void Start()
    {
        loadMeshes();
    }

    private void loadMeshes()
    {
        meshes = Resources.LoadAll<GameObject>("Sets/Dungeon") as GameObject[];
        foreach(GameObject mesh in meshes)
        {
            Debug.Log(mesh.name);
        }
        Debug.Log("Found " + meshes.Length + " objs");
        string level = "";
        if(PlayerPrefs.HasKey("TempLevel"))
        {
            level = PlayerPrefs.GetString("TempLevel");
            Debug.Log(level);
            int stringMarker = 0;
            for(int i = 0; i < mapSize*mapSize; ++i)
            {
                char c = level[stringMarker];
                if(c == 'N')
                {
                    stringMarker++;
                }
                else
                {
                    int x = i/mapSize;
                    int y = i%mapSize;
                    string descriptor = level.Substring(stringMarker,descriptorSize);
                    stringMarker+=descriptorSize;
                    //Debug.Log(descriptor);
                    string selHex = descriptor.Substring(3,3);
                    int selInt = int.Parse(selHex, System.Globalization.NumberStyles.HexNumber);
                    int rot = int.Parse(descriptor.Substring(6,1));
                    //rot+=1;
                    Debug.Log("Hex: " + selHex + " Int: " + selInt + " Rot: " + rot);
                    GameObject go = Instantiate(meshes[selInt]);
                    go.transform.localScale = new Vector3(0.002f,0.002f,0.002f);
                    go.transform.position = new Vector3(x*offset,0,y*offset);  
                    //go.transform.rotation = Quaternion.Euler(0,rot*90,0);
                    go.transform.rotation = quats[rot];
                    go.transform.parent = this.transform;
                }
            }
            //GameObject go = Instantiate(meshes[2],this.transform);
            //go.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        }
    }

    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Axis 1");
        float vertical = Input.GetAxis("Axis 2");

        if (Input.GetKey(KeyCode.JoystickButton0) == true) Debug.Log("C");
        if (Input.GetKey(KeyCode.JoystickButton1) == true) Debug.Log("A");
        if (Input.GetKey(KeyCode.JoystickButton3) == true) Debug.Log("B");
        if (Input.GetKey(KeyCode.JoystickButton4) == true) Debug.Log("D");
        if (Input.GetKey(KeyCode.JoystickButton6) == true) Debug.Log("T2");
        if (Input.GetKey(KeyCode.JoystickButton7) == true) Debug.Log("T1");

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
}
