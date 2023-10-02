using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformVectorUtils
{
    public static Quaternion PlatformAngle(Transform leftHandTransform, Transform rightHandTransform, Transform rigTransform, Transform playerTransform)
    {
        // X Position & Angle
        Vector3 averageHandPosition = new Vector3(
            (rightHandTransform.position.x + leftHandTransform.position.x) / 2f,
            (rightHandTransform.position.y + leftHandTransform.position.y) / 2f,
            (rightHandTransform.position.z + leftHandTransform.position.z) / 2f
        );
        Vector3 angleComparePointX = new Vector3(
            averageHandPosition.x, playerTransform.position.y, averageHandPosition.z
        );

        float platformXAngle = Vector3.SignedAngle((angleComparePointX - playerTransform.position), (averageHandPosition - playerTransform.position), rigTransform.rotation * Vector3.right);

        // Y Position & Angle
        Vector3 angleComparePointY = new Vector3(
            averageHandPosition.x, playerTransform.position.y, averageHandPosition.z
        );

        Vector3 yDirection = (angleComparePointY - playerTransform.position).normalized;
        float platformYAngle = Vector3.SignedAngle(Vector3.forward, yDirection, Vector3.up);

        // Z Position & Angle
        Vector3 rightHandForwardVector = rightHandTransform.position + (rightHandTransform.rotation * Vector3.down);
        Vector3 rightHandRightVector = (rightHandTransform.rotation * Vector3.right).normalized;
        Vector3 rightHandPlanarVector = VectorUtils.YPlanarVector(rightHandRightVector).normalized;
        float rightHandAngle = Vector3.SignedAngle(rightHandRightVector, rightHandPlanarVector, rightHandForwardVector);
        // Debug.DrawLine(rightHandTransform.position, rightHandForwardVector);
        // Debug.DrawLine(rightHandTransform.position, rightHandTransform.position + rightHandRightVector, Color.cyan);
        // Debug.DrawLine(rightHandTransform.position, rightHandTransform.position + rightHandPlanarVector, Color.green);


        Vector3 leftHandForwardVector = leftHandTransform.position + (leftHandTransform.rotation * Vector3.down);
        Vector3 leftHandRightVector = (leftHandTransform.rotation * Vector3.right).normalized;
        Vector3 leftHandPlanarVector = new Vector3(leftHandRightVector.x, 0, leftHandRightVector.z).normalized;
        float leftHandAngle = Vector3.SignedAngle(leftHandRightVector, leftHandPlanarVector, leftHandForwardVector);

        int flipPlatformAngle = Vector3.SignedAngle(Vector3.up, rightHandRightVector, rightHandForwardVector) > 0 ? 1 : -1;

        float platformZAngle = rightHandAngle * flipPlatformAngle;


        return Quaternion.Euler(platformXAngle, platformYAngle, platformZAngle);
    }

    public static float CalculatePlaformMovementHitMultiplier(Vector3 leftHandVelocity, Vector3 rightHandVelocity)
    {
        Vector3 averageHandVelocity = (leftHandVelocity + rightHandVelocity) / 2f;
        float platformMovementAngle = Vector3.Angle(Vector3.up, averageHandVelocity);
        float platformSpeedClamp = Mathf.Max(1f, Mathf.Log(averageHandVelocity.magnitude, 2f));
        float platformAdjustment = platformMovementAngle < 90f ? platformSpeedClamp : 1f / platformSpeedClamp;

        return platformAdjustment;
    }
}
