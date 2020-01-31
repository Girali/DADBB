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
    int life = 80;
    public bool offline = false;

    PhotonView pv;
    Rigidbody rb;
    PlayerView playerView;
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

    void Death()
    {
        PhotonNetwork.RemoveRPCs(pv);
        PhotonNetwork.Destroy(pv);
    }

    public void AddLife(int i)
    {
        life += i;

        if (life < 0)
            life = 0;


        if (life > 80)
            life = 80;

        playerView.UpdateLife(life);

        pv.RPC("RPC_setLife", RpcTarget.Others, life);

        if (life == 0)
            Death();
    }

    [PunRPC]
    public void RPC_setLife(int life)
    {
        playerView.UpdateLife(life);
        this.life = life;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        playerView = GetComponent<PlayerView>();

        if(offline)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameObject.tag = "LocalPlayer";
            for (int i = 0; i < view.childCount; i++)
            {
                if (view.GetChild(i).GetComponent<Camera>())
                    view.GetChild(i).GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            }
        }
        else
        {
            if (!pv.Owner.IsMasterClient)
            {
                for (int i = 0; i < view.childCount; i++)
                {
                    if (view.GetChild(i).GetComponent<Camera>())
                        view.GetChild(i).GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                }
            }
            else
            {
                Destroy(GetComponent<MeshRenderer>());
                Destroy(GetComponent<MeshFilter>());
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<CapsuleCollider>().enabled = false;
                gameObject.layer = 8;
                transform.localScale = new Vector3(50, 50, 50);
                speed = 25f;
            }

            if (pv.IsMine)
            { 
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gameObject.tag = "LocalPlayer";
            }
            else
            {
                Destroy(view.gameObject);
            }
        }
    }

    private void Update()
    {
        if (pv.IsMine || offline)
        {
            if (Input.GetKeyDown(KeyCode.O))
                AddLife(10);

            if (Input.GetKeyDown(KeyCode.P))
                AddLife(-10);

            CameraMouvement(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
    }

    private void FixedUpdate()
    {
        if (pv.IsMine|| offline)
        {
            Mouvement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}
