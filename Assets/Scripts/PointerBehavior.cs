using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerBehavior : MonoBehaviour
{
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => Grow();
    }

    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
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
