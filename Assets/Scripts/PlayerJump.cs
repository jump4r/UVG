using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty jumpAction;
    [SerializeField]
    private LayerMask groundLayer;
    private CharacterController character;
    private ActionBasedContinuousMoveProvider playerMove;
    private float additionalHeight = 0.2f;
    private float gravity = -9.81f;
    private float fallingSpeed = 0;
    private bool jumpReady = true;

    private float jumpPressed;

    [SerializeField]
    private float jumpForce = 2f;
    private Vector3 verticalVelocity = Vector3.zero;

    void Start()
    {
        character = GetComponent<CharacterController>();
        playerMove = GetComponent<ActionBasedContinuousMoveProvider>();
    }

    void Update()
    {
        jumpPressed = jumpAction.action.ReadValue<float>();
    }

    void FixedUpdate()
    {
        if (CheckIfGrounded())
        {
            fallingSpeed = 0;
            verticalVelocity = Vector3.zero;
            jumpReady = true;
            playerMove.moveSpeed = PlayerConstants.MOVE_SPEED;
        }

        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
            jumpReady = false;
            playerMove.moveSpeed = PlayerConstants.AIR_SPEED;
        }

        if (jumpReady && jumpPressed == 1f) 
        {
            jumpReady = false;
            verticalVelocity = Vector3.up * jumpForce;
        }

        verticalVelocity += Vector3.up * fallingSpeed * (Time.fixedDeltaTime);
        character.Move(verticalVelocity * Time.fixedDeltaTime);
    }
    
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);

        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
