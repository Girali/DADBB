using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClipCamera : MonoBehaviour
{
    float speed = 5f;
    float currentLookSensitivity = 2f;
    float currentCameraRotationX = 0f;
    float currentCameraRotationY = 0f;
    float cameraRotationLimit = 85f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CameraMouvement(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Mouvement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void CameraMouvement(float y, float x)
    {
        float yRot = y;
        float cameraRotationY = yRot * currentLookSensitivity;

        currentCameraRotationY += cameraRotationY;

        float xRot = x;
        float cameraRotationX = xRot * currentLookSensitivity;


        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        transform.eulerAngles = new Vector3(currentCameraRotationX, currentCameraRotationY, 0f);
    }

    void Mouvement(float x, float z)
    {
        Vector3 horizontalMove = Vector3.right * x;
        Vector3 verticalMove = Vector3.forward * z;

        Vector3 move = (horizontalMove + verticalMove).normalized * speed;
        transform.Translate(move * Time.deltaTime);
    }
}
