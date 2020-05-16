using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHeadOnWalk : MonoBehaviour
{
    private float time = 0f;
    public float speed = 0.15f;
    public float force = 0.1f;
    public float originPoint = 0f;

    float resetMovment;
    float horizontal;
    float vertical;
    Vector3 savePosition;

    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public int indexSteps = 0;

    public MovePlayer movePlayerScript;

    // Update is called once per frame
    void Update()
    {
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
    }

    void StepsSound()
    {
        if(resetMovment <= -0.95f && !audioSource.isPlaying && movePlayerScript.isOnFloor)
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
}
