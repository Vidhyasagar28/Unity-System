using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController character;

    [Header("Movement Configs")]
    [SerializeField] Vector3 movement;
    [SerializeField] float CurrentSpeed;
    [SerializeField] float WalkSpeed;
    [SerializeField] float SprintSpeed;
    [SerializeField] bool isSprinting;
    bool isCrouch;
    float smoothenAngle;
    

    [Header("Touch Grass?")]
    [SerializeField] bool IsPlayerGrounded;
    [SerializeField] Transform feet;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float gravity = -9.8f;
    

    [Header("Jump")]
    [SerializeField] bool IsJumping = false;
    [SerializeField] float jumpHeight;
    [SerializeField] float applyGravity = 0;

    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        InputManager._instance.OnCrouching += SetCrouch;
    }

    // Update is called once per frame
    void Update()
    {
        #region Get Inputs
        Vector3 horizontal = InputManager._instance.GetMovementInput() * CurrentSpeed;
        horizontal = horizontal.z * cam.forward.normalized + horizontal.x * cam.right.normalized;
        IsJumping = InputManager._instance.isJumping;
        isSprinting = InputManager._instance.isSprinting;
        #endregion

        #region Ground & Gravity 

        IsGrounded();
        if (IsPlayerGrounded && applyGravity < 0f)
        {
            applyGravity = -2f;
        }
        if (IsPlayerGrounded && IsJumping)
        {
            applyGravity = Mathf.Sqrt(2f * gravity * jumpHeight);
        }
        applyGravity -= gravity * Time.deltaTime;
        #endregion

        Vector3 vertical = new Vector3(0f, applyGravity, 0f);

        movement = horizontal + vertical;

        SetMovementSpeed();
        
        if (horizontal.sqrMagnitude > 0.01f) {
            float rotateAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float smoothAngleTurn = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref smoothenAngle, 0.1f);
            transform.rotation =  Quaternion.Euler(0f, smoothAngleTurn, 0f);      
        }
        character.Move(movement * Time.deltaTime);
    }

    void IsGrounded()
    {
        IsPlayerGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, groundLayer);
    }
    void SetMovementSpeed()
    {
        if (isSprinting)
            CurrentSpeed = SprintSpeed;
        else
            CurrentSpeed = WalkSpeed;
    }
    void SetCrouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            character.height /= 2;
        }
        else
        {
            character.height = 1.7f;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feet.position, groundCheckRadius);
    }

}
