using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviourPunCallbacks
{
    [Header("Player Config")]
    public CharacterController controller;
    public float speed = 6f;
    float defaultSpeed = 6f;
    public float jumpHeight = 3f;
    public float gravity = -20f;
    public bool isRunning = false;
    public AudioClip[] jumpSound;
    public AudioSource jumpAudio;
    public bool upInTheAir = false;
    public GameObject character;

    [Header("Verify Floor")]
    public Transform checkFloor;
    public float SphereRadius = 0.4f;
    public LayerMask floorMask;
    public bool isOnFloor;
    Vector3 fallSpeed;

    [Header("Verify is Lowered")]
    public Transform cameraTransform;
    public bool isLowered = false;
    public bool riseBlocked;
    public float liftingUpHeight = 2, loweringHeight = 1.5f;
    public float cameraPositionStanding = 1, cameraPositionDown = 0.5f;
    float currentVelocity = 1f;
    RaycastHit hit;

    [Header("Player Status")]
    public float hp = 100;
    public float stamina = 100;
    public int kiils = 0;
    bool tiredOut = false;
    public Breath breathScript;
    public GameObject canvasPlayer;
    public GameObject options;
    DateTime startTime;

    UIManager uiScript;

    // Start is called before the first frame update
    void Start()
    {
        canvasPlayer.SetActive(photonView.IsMine);
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UIManager>();
        startTime = DateTime.Now;
        if (!photonView.IsMine) return;
        character.SetActive(false);
        defaultSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        VerifyOptions();
        if (options.activeInHierarchy) return;
        isOnFloor = Physics.CheckSphere(checkFloor.position, SphereRadius, floorMask);
        VerifyIsRunning();
        Walk();
        Jump();
        VerifyTurnDown();
        VerifyTiredOut();
        SoundJump();
    }

    void SoundJump()
    {
        if (!isOnFloor)
        {
            upInTheAir = true;
        }
        if (isOnFloor && upInTheAir)
        {
            upInTheAir = false;
            jumpAudio.clip = jumpSound[1];
            jumpAudio.Play();
        }
    }

    void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    void VerifyIsRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isOnFloor && !isLowered && !tiredOut)
        {
            speed = defaultSpeed * 1.5f;
            isRunning = true;
            stamina -= 0.3f;
        }
        else
        {
            speed = defaultSpeed;
            isRunning = false;
            stamina += 0.1f;
        }

        stamina = Mathf.Clamp(stamina, 0, 100);
    }

    void VerifyTiredOut()
    {
        if (stamina == 0)
        {
            tiredOut = true;
            breathScript.breathForce = 5;
        }

        if (stamina > 20)
        {
            tiredOut = false;
        }
    }

    void Jump()
    {
        if (isOnFloor && fallSpeed.y < 0)
        {
            fallSpeed.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isOnFloor)
        {
            fallSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpAudio.clip = jumpSound[0];
            jumpAudio.Play();
        }

        fallSpeed.y += gravity * Time.deltaTime;

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

        TurnDownMovment();
    }

    void TurnDown()
    {
        isLowered = !isLowered;
    }

    void TurnDownMovment()
    {
        controller.center = Vector3.down * (liftingUpHeight - controller.height) / 2f;
        if (isLowered)
        {
            controller.height = Mathf.Lerp(controller.height, loweringHeight, Time.deltaTime * 3);
            float newCameraPositionDown = Mathf.SmoothDamp(cameraTransform.localPosition.y, cameraPositionDown, ref currentVelocity, Time.deltaTime * 3);
            cameraTransform.localPosition = new Vector3(0, newCameraPositionDown, 0);
            speed = defaultSpeed / 2;
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, liftingUpHeight, Time.deltaTime * 3);
            float newcameraPositionStanding = Mathf.SmoothDamp(cameraTransform.localPosition.y, cameraPositionStanding, ref currentVelocity, Time.deltaTime * 3);
            cameraTransform.localPosition = new Vector3(0, newcameraPositionStanding, 0);
            speed = defaultSpeed;
        }
    }

    void checkBlockLowered()
    {
        if (Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
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
    public void VerifyOptions()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.SetActive(!options.activeInHierarchy);

            if (options.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void LeaveRoom()
    {
        if (photonView.IsMine)
        {
            /*
            if (photonView.Owner.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
            }
            */

            TimeSpan duration = DateTime.Now.Subtract(startTime);

            EndGame endGame = new EndGame();
            endGame.changeValue(kiils, photonView.Owner.NickName, duration.ToString());

            PhotonNetwork.LeaveRoom();
            Debug.Log("Leave Room: " + photonView.Owner.NickName + " - " + photonView.IsMine);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(3);
        }
    }

    public void TakeDamage(string playerID, string playerName)
    {
        photonView.RPC("TakeDamageNetwork", RpcTarget.AllViaServer, playerID, playerName);
    }

    [PunRPC]
    public void TakeDamageNetwork(string playerID, string playerName)
    {
        int damage = (int)UnityEngine.Random.Range(25f, 100f);
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log(photonView.Owner.NickName + " => " + photonView.IsMine);

            if (photonView.IsMine)
            {
                Debug.Log("TakeDamageNetwork: " + photonView.Owner.NickName + " - " + playerName);

                photonView.RPC("DeathPlayer", RpcTarget.AllViaServer, playerName, photonView.Owner.NickName, playerID);
                LeaveRoom();
            }

        }
    }

    [PunRPC]
    public void DeathPlayer(string playerKillerName, string playerDeathName, string playerID)
    {

        var listPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach(var player in listPlayer)
        {
            var playerView = player.GetComponent<PhotonView>();
            if (playerView.IsMine)
            {
                var playerScript = player.GetComponent<MovePlayer>();
                if (playerScript.uiScript != null)
                {
                    playerScript.uiScript.addNewDeath(playerKillerName, playerDeathName);
                }

                if (playerView.Owner.UserId.Equals(playerID))
                {
                    playerScript.addKill();
                    player.GetComponentInChildren<Glock>().addAmmunition();
                }
                break;
            }
        }
    }

    public void addKill()
    {
        kiils++;
    }
}
