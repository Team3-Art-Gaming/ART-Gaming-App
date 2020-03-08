using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerBehavior : MonoBehaviour
{
    Vector2 move;
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
    }

    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
        Debug.Log("X: " + m.x + " Y: " + m.y); 
        transform.Translate(m, Space.World);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
