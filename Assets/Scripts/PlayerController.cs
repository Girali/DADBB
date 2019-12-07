using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{

    bool canMove = true;
    float speed = 5f;
    float currentLookSensitivity = 2f;
    float currentCameraRotationX = 0f;
    float cameraRotationLimit = 85f;

    PhotonView pv;
    Rigidbody rb;
    [SerializeField]
    Transform view = null;

    void Mouvement(float x, float z)
    {
        if (canMove)
        {
            Vector3 horizontalMove = Vector3.right * x;
            Vector3 verticalMove = Vector3.forward * z;

            Vector3 move = (horizontalMove + verticalMove).normalized * speed;
            move.y = rb.velocity.y;
            transform.Translate(move * Time.deltaTime);
        }
    }

    void CameraMouvement(float y, float x)
    {
        if (canMove)
        {
            float yRot = y;
            Vector3 rotation = new Vector3(0f, yRot, 0f) * currentLookSensitivity;

            float xRot = x;
            float cameraRotationX = xRot * currentLookSensitivity;

            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            view.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        { 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameObject.tag = "LocalPlayer";

            if (!PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < view.childCount; i++)
                {
                    view.GetChild(i).GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                }
            }
            else
            {
                Destroy(GetComponent<MeshRenderer>());
                Destroy(GetComponent<MeshFilter>());
                transform.localScale = new Vector3(1, 50, 1);
                speed = 25f;
            }
        }
        else
        {
            Destroy(view.gameObject);
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {

            CameraMouvement(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {

            Mouvement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}
