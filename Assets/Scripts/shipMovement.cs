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
            Quaternion headsetRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);

            float zRotation = headsetRotation.eulerAngles.z;
            float xRotation = headsetRotation.eulerAngles.x;

            if (zRotation > 180f)
            {
                zRotation -= 360f;
            }
            if (xRotation > 180f)
            {
                xRotation -= 360f;
            }

            float clampedZRotation = Mathf.Clamp(-zRotation, -maxHeadsetRotation, maxHeadsetRotation);
            float clampedXRotation = Mathf.Clamp(-xRotation, -maxHeadsetRotation, maxHeadsetRotation);

            float targetX = Mathf.Lerp(-maxMovementX, maxMovementX, (clampedZRotation + maxHeadsetRotation) / (2 * maxHeadsetRotation));
            float targetY = Mathf.Lerp(-maxMovementY, maxMovementY, (clampedXRotation + maxHeadsetRotation) / (2 * maxHeadsetRotation));

            Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z + curSpeed);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            float clampedYaw = Mathf.Clamp(headsetRotation.eulerAngles.y, -maxHeadsetRotation, maxHeadsetRotation);
            targetRotation = Quaternion.Euler(-clampedXRotation / rotationDivision, 0, -clampedZRotation / rotationDivision);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime);
        }
    }
}
