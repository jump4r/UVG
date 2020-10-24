﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    public Transform headTransform;
    public Transform rigTransform;

    private BoxCollider platform;

    void Start()
    {
       platform = GetComponent<BoxCollider>(); 
    }

    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);
        if (Vector3.Distance(rightHandTransform.position, leftHandTransform.position) < 0.2f && !platform.enabled)
        {
            EnablePlatform();
        }

        else
        {
            DisablePlatform();
        }

        if (platform.enabled)
        {
            Vector3 averageHandPosition = new Vector3(
                (rightHandTransform.position.x + leftHandTransform.position.x) / 2f,
                (rightHandTransform.position.y + leftHandTransform.position.y) / 2f,
                (rightHandTransform.position.z + leftHandTransform.position.z) / 2f
            );

            Vector3 angleComparePoint = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );

            float platformAngle = Vector3.Angle((angleComparePoint - transform.position), (averageHandPosition - transform.position));
            transform.rotation = Quaternion.Euler(platformAngle, rigTransform.rotation.eulerAngles.y, transform.rotation.z);
        }
    }

    public void EnablePlatform()
    {
        platform.enabled = true;
    }

    public void DisablePlatform()
    {
        platform.enabled = false;
    }
}