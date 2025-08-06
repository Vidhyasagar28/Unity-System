
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{

    public static InputManager _instance { get; private set; }
    PlayerControls playerControls;
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseInput { get; private set; }

    public bool isJumping = false;
    public bool isInteracting = false;
    public event Action OnShooting;

    private void Awake()
    {
        if (_instance == null) { _instance = this;}
        else { Destroy(this.gameObject);}
           
        playerControls = new PlayerControls();

        
        playerControls.Locomotion.Shoot.performed += OnShootPerformed;

        playerControls.Enable();
    }
    private void Update()
    {
        MoveInput = playerControls.Locomotion.Movement.ReadValue<Vector2>();
        MouseInput = playerControls.Locomotion.MouseMovement.ReadValue<Vector2>();
        isJumping = playerControls.Locomotion.Jump.WasPerformedThisFrame();
        isInteracting = playerControls.UI.Interact.triggered;
    }
    void OnShootPerformed(InputAction.CallbackContext ctx) { OnShooting?.Invoke(); }

    private void OnDisable()
    {
        
        playerControls.Locomotion.Shoot.performed -= OnShootPerformed;

        playerControls.Disable();
    }

}
