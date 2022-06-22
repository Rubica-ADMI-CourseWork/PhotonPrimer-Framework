using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Receives Movement direction input from the Joystick Controls
/// </summary>
public class PlayerMovement : MonoBehaviourPun
{
    CamController controller;
    JoystickController joystickController;
    [field:SerializeField]public Vector2 MovementInput { get;private set; }

    private void OnEnable()
    {
        controller = FindObjectOfType<CamController>();
        joystickController = FindObjectOfType<JoystickController>();
    }
    private void Start()
    {
        controller.InitCamera(transform, transform);
        joystickController.OnMove += SetInput;
    }

    private void Update()
    {
        float horizontalInput = MovementInput.x;
        float upDownInput = MovementInput.y;

        
        transform.Rotate(Vector3.up * horizontalInput * 10f * Time.deltaTime);

        transform.Translate(Vector3.forward * upDownInput * 10f * Time.deltaTime);
    }
    private void SetInput(Vector2 movement)
    {
        MovementInput = movement;
    }
}
