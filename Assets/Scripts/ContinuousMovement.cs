using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1f;
    public XRNode inputSource;
    public XRNode secondaryInputSource;
    private Vector2 inputAxis;
    private Vector2 secondaryInputAxis;
    private CharacterController character;
    private XRRig rig;
    public LayerMask groundLayer;

    public float gravity = -9.81f;
    private float fallingSpeed = 0;
    private float additionalHeight = 0.2f;

    public float jumpVelocity = 1f;
    private bool jumpButtonPressed = false;
    private bool jumpReady = true;
    private Vector3 verticalVelocity = Vector3.zero;

    void Start()
    {
        character = GetComponent<CharacterController>();   
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice secondaryDevice = InputDevices.GetDeviceAtXRNode(secondaryInputSource);
        secondaryDevice.TryGetFeatureValue(CommonUsages.primaryButton, out jumpButtonPressed);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);

        // gravity
        if (CheckIfGrounded())
        {
            fallingSpeed = 0;
            verticalVelocity = Vector3.zero;
            jumpReady = true;
        }

        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
            jumpReady = false;
        }

        // Jump
        if (jumpReady && jumpButtonPressed)
        {
            jumpReady = false;
            verticalVelocity = Vector3.up * jumpVelocity;
        }

        verticalVelocity += Vector3.up * fallingSpeed * Time.fixedDeltaTime;
        character.Move(verticalVelocity * Time.fixedDeltaTime);
    }

    private void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
