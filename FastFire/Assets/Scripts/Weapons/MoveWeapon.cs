using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveWeapon : MonoBehaviourPunCallbacks
{
    public float value;
    float defaultvalue;
    public float softensValue;
    public float maxValue;
    Vector3 startinfPosition;


    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        startinfPosition = transform.localPosition;
        defaultvalue = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        float movementX = -Input.GetAxis("Mouse X") * value;
        float movementY = -Input.GetAxis("Mouse Y") * value;

        movementX = Mathf.Clamp(movementX, -maxValue, maxValue);
        movementY = Mathf.Clamp(movementY, -maxValue, maxValue);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + startinfPosition, Time.deltaTime * softensValue);
    }

    public void SetValueToDefault()
    {
        value = defaultvalue;
    }
}
