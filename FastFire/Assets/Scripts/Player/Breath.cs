using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Breath : MonoBehaviourPunCallbacks
{
    public bool isInspiring = true;
    public float heightMin = -0.35f, heightMax = 0.35f;

    [Range(0f, 5f)]
    public float breathForce = 1f;

    float movement;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (isInspiring)
        {
            movement = Mathf.Lerp(movement, heightMax, Time.deltaTime * 1 * breathForce);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);

            if(movement >= heightMax - 0.01f)
            {
                isInspiring = !isInspiring;
            }
        }
        else
        {
            movement = Mathf.Lerp(movement, heightMin, Time.deltaTime * 1 * breathForce);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);

            if (movement <= heightMin + 0.01f)
            {
                isInspiring = !isInspiring;
            }
        }

        if(breathForce != 0)
        {
            breathForce = Mathf.Lerp(breathForce, 1f, Time.deltaTime * 0.2f);
        }
    }
}
