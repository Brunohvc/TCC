using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlaJogador : MonoBehaviourPunCallbacks
{
    /*
    public float Velocidade = 10;
    Vector3 direcao;

    public Animator animator;
    Vector3 distanciaCompensar;

    Actions actions;
    */

    public GameObject camera;
    PlayerController controller;
    public GameObject character;

    //Input
    protected PlayerS Player;

    //Parameters
    protected const float RotationSpeed = 100;

    //Camera Controll
    public Vector3 CameraPivot;
    public float CameraDistance;
    protected float InputRotationX;
    protected float InputRotationY;

    protected Vector3 CharacterPivot;
    protected Vector3 LookDirection;

    void Start()
    {
        camera.SetActive(photonView.IsMine);
        if (!photonView.IsMine) return;
        controller = character.GetComponent<PlayerController>();
        /*
        camera.SetActive(photonView.IsMine);
        if (!photonView.IsMine) return;

        actions = character.GetComponent<Actions>();

        distanciaCompensar = camera.transform.position - transform.position;
        */
        controller.SetArsenal("Rifle");
        

        Player = FindObjectOfType<PlayerS>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!photonView.IsMine) return;
        /*
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

        // transform.Translate(direcao * Velocidade * Time.deltaTime);

        if (direcao != Vector3.zero)
        {
            // GetComponent<Animator>().SetBool("Movendo", true);
            //animator.SetFloat("Blend", 1);
            actions.SendMessage("Run", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            // GetComponent<Animator>().SetBool("Movendo", false);
            //animator.SetFloat("Blend", 0);
            actions.SendMessage("Stay", SendMessageOptions.DontRequireReceiver);
        }
        */

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Cursor.lockState = CursorLockMode.Locked;

        //input
        InputRotationX = InputRotationX + mouseInput.x * RotationSpeed * Time.deltaTime % 360f;
        InputRotationY = Mathf.Clamp(InputRotationY - mouseInput.y * RotationSpeed * Time.deltaTime, -88f, 88f);

        //left and forward
        var characterForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        //look and run direction
        var runDirection = characterForward * (Input.GetAxisRaw("Vertical")) + characterLeft * (Input.GetAxisRaw("Horizontal"));
        LookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterForward;


        //set player values
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = LookDirection.x;
        Player.Input.LookZ = LookDirection.z;

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Player.Input.SwitchToAK = true;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Player.Input.SwitchToPistol = true;
        */

        Player.Input.Shoot = Input.GetMouseButton(0);

        RaycastHit hitInfo = new RaycastHit();
        gameObject.layer = Physics.IgnoreRaycastLayer;
        int j = Physics.DefaultRaycastLayers;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, PlayerS.AllButIgnoreLayer))
            Player.Input.ShootTarget = hitInfo.point;
        else
            Player.Input.ShootTarget = Vector3.up * 1000f;
        gameObject.layer = 0;

        // Debug.DrawLine(transform.position + Vector3.up * 1.5f, Player.Input.ShootTarget, Color.magenta);

        CharacterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * CameraPivot;
    }

    void FixedUpdate()
    {
        /*
        // if (!photonView.IsMine) return;

        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (direcao * Velocidade * Time.deltaTime));

        camera.transform.position = transform.position + distanciaCompensar;
        */
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;

        //set camera values
        camera.transform.position = (transform.position + CharacterPivot) - LookDirection * CameraDistance;
        camera.transform.rotation = Quaternion.LookRotation(LookDirection, Vector3.up);
    }
}
