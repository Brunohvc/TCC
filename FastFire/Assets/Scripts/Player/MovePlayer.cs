using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovePlayer : MonoBehaviourPunCallbacks
{

    public CharacterController controller;
    public float speed = 6f;
    public float jumpHeight = 3f;
    public float gravity = -20f;

    public Transform checkFloor;
    public float SphereRadius = 0.4f;
    public LayerMask floorMask;
    public bool isOnFloor;

    Vector3 fallSpeed;

    public Transform cameraTransform;
    public bool isLowered = false;
    public bool riseBlocked;
    public float liftingUpHeight = 2, loweringHeight = 1.5f;
    public float cameraPositionStanding = 1, cameraPositionDown = 0.5f;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isOnFloor = Physics.CheckSphere(checkFloor.position, SphereRadius, floorMask);
        Walk();
        Jump();
        VerifyTurnDown();
    }


    void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    void Jump()
    {
        if(isOnFloor && fallSpeed.y < 0)
        {
            fallSpeed.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isOnFloor)
        {
            Debug.Log("Jump");
            fallSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        fallSpeed.y += gravity * Time.deltaTime;

        Debug.Log(fallSpeed * Time.deltaTime);

        controller.Move(fallSpeed * Time.deltaTime);
    }

    void VerifyTurnDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isLowered)
            {
                checkBlockLowered();
            }

            if (!riseBlocked && isOnFloor)
            {
                TurnDown();
            }
        }
    }

    void TurnDown()
    {
        isLowered = !isLowered;
        if (isLowered)
        {
            controller.height = loweringHeight;
            cameraTransform.localPosition = new Vector3(0, cameraPositionDown, 0);
        }
        else
        {
            controller.height = liftingUpHeight;
            cameraTransform.localPosition = new Vector3(0, cameraPositionStanding, 0);
        }
    }

    void checkBlockLowered()
    {
        if(Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
        {
            riseBlocked = true;
        }
        else
        {
            riseBlocked = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(checkFloor.position, SphereRadius);
    }
}
