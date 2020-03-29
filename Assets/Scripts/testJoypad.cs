using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testJoypad : MonoBehaviour
{
    //private Text readout;
    // Start is called before the first frame update
    void Start()
    {
        //readout = GetComponentInChildren<Text>();
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Axis 1");
        float vertical = Input.GetAxis("Axis 2");
        float speed=5.0f;
        if (Mathf.Abs(horizontal)>0.01)
        {
            //move in the direction of the camera
            //transform.position = transform.position + Camera.main.transform.right * -horizontal * speed * Time.deltaTime;
            transform.localPosition += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
            //readout.text = vertical.ToString();
        }
        if (Mathf.Abs(vertical)>0.01)
        {
            //strafe sideways
            transform.localPosition += new Vector3(0,0, -vertical * speed * Time.deltaTime);
            //transform.position = transform.position + Camera.main.transform.forward * -vertical * speed * Time.deltaTime;
        }         
    }
    

    /*
    void FixedUpdate()
    {
        //reading the input:
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
         
        //assuming we only using the single camera:
        var camera = Camera.main;
 
        //camera forward and right vectors:
        var forward = camera.transform.forward;
        var right = camera.transform.right;
 
        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
 
        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
 
        //now we can apply the movement:
        transform.Translate(desiredMoveDirection * Time.deltaTime);
    }
    */
}
