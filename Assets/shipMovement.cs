using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class shipMovement : MonoBehaviour
{

    [SerializeField] float maxMovementX = 10f;
    [SerializeField] float maxMovementY = 10f;
    [SerializeField] float smoothTime = 1f;
    [SerializeField] float maxHeadsetRotation = 45f; // Maximum rotation in degrees for mapping
    [SerializeField] float rotationDivision = 2f;
    [SerializeField] float moveSpeed = 10f;

    [SerializeField] public static bool canMove = true;


    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation;
    private Quaternion currentRotationVelocity = Quaternion.identity;

    void Update()
    {
        float curSpeed = moveSpeed * Time.deltaTime;

        if (canMove)
        {
            // Get the headset rotation
            Quaternion headsetRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);


            // Extract z-axis (roll) and x-axis (pitch) rotation
            float zRotation = headsetRotation.eulerAngles.z;
            float xRotation = headsetRotation.eulerAngles.x;

            // Convert rotations to a range of -180 to 180 degrees for clamping
            if (zRotation > 180f)
                zRotation -= 360f;
            if (xRotation > 180f)
                xRotation -= 360f;

            // Clamp the rotations to -maxHeadsetRotation to maxHeadsetRotation
            float clampedZRotation = Mathf.Clamp(zRotation, -maxHeadsetRotation, maxHeadsetRotation);
            float clampedXRotation = Mathf.Clamp(-xRotation, -maxHeadsetRotation, maxHeadsetRotation);

            //Debug.Log(clampedXRotation + " " + clampedZRotation);
            // Map the clamped rotations to movement ranges
            float targetX = Mathf.Lerp(-maxMovementX, maxMovementX, (clampedZRotation + maxHeadsetRotation) / (2 * maxHeadsetRotation));
            float targetY = Mathf.Lerp(-maxMovementY, maxMovementY, (clampedXRotation + maxHeadsetRotation) / (2 * maxHeadsetRotation));

            // Smoothly move the object to the target position
            Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z + curSpeed);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            float clampedYaw = Mathf.Clamp(headsetRotation.eulerAngles.y, -maxHeadsetRotation, maxHeadsetRotation);
            targetRotation = Quaternion.Euler(-clampedXRotation / rotationDivision, 0, -clampedZRotation / rotationDivision);

            // Smoothly interpolate rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime);
        }
    }
}
