using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveHeadOnWalk : MonoBehaviourPunCallbacks
{
    private float time = 0f;
    public float speed = 0.15f;
    float defaultSpeed = 0.15f;
    public float force = 0.1f;
    float defaultForce = 0.1f;
    public float originPoint = 0f;

    float resetMovment;
    float horizontal;
    float vertical;
    Vector3 savePosition;

    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public int indexSteps = 0;

    MovePlayer scripMovePlayer;

    private void Start()
    {
        if (!photonView.IsMine) return;
        scripMovePlayer = GetComponentInParent<MovePlayer>();
        defaultSpeed = speed;
        defaultForce = force;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        resetMovment = 0;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        savePosition = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            time = 0f;
        }
        else
        {
            resetMovment = Mathf.Sin(time);
            time += speed;

            if (time > Mathf.PI * 2)
            {
                time -= Mathf.PI * 2;
            }
        }

        if (resetMovment != 0)
        {
            float changeMovement = resetMovment * force;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
            changeMovement *= totalAxes;
            savePosition.y = originPoint + changeMovement;
        }
        else
        {
            savePosition.y = originPoint;
        }

        transform.localPosition = savePosition;

        StepsSound();
        updateHead();
    }

    void StepsSound()
    {
        if(resetMovment <= -0.95f && !audioSource.isPlaying && scripMovePlayer.isOnFloor)
        {
            audioSource.clip = audioClip[indexSteps];
            audioSource.Play();

            indexSteps++;

            if(indexSteps >= 4)
            {
                indexSteps = 0;
            }
        } 
    }

    void updateHead()
    {
        if (scripMovePlayer.isRunning)
        {
            speed = defaultSpeed * 1.5f;
            force = defaultForce * 1.15f; 
        }
        else if (scripMovePlayer.isLowered)
        {
            speed = defaultSpeed * 0.75f;
            force = defaultForce * 0.70f;
        }
        else
        {
            speed = defaultSpeed;
            force = defaultForce;
        }
    }
}
