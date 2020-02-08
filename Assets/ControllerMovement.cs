using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllerMovement : MonoBehaviour
{
    public float speed = 0.3F;
    public float rotateSpeed = 0.3F;

    public CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Rotate around y - axis
       // transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Move
        Vector3 offset = new Vector3(speed * Input.GetAxis("Horizontal"), 0, speed * Input.GetAxis("Vertical"));
        transform.position += offset;
    }
}
