using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    CharacterController character;

    [Header("Movement Configs")]
    [SerializeField] Vector3 movement;
    [SerializeField] float WalkSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Get Inputs
        Vector2 moveInput = InputManager._instance.MoveInput;
        IsJumping = InputManager._instance.isJumping;

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

        #endregion

        
        
        applyGravity -= gravity * Time.deltaTime;

        Vector3 horizontal = (transform.right * moveInput.x + transform.forward * moveInput.y) * WalkSpeed;
        Vector3 vertical = new Vector3(0f, applyGravity, 0f);

        movement = horizontal + vertical;        

        character.Move(movement * Time.deltaTime);
    }

    void IsGrounded()
    {
        IsPlayerGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(feet.position, groundCheckRadius);
    }

}
