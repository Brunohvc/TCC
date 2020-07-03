using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RotationCamera : MonoBehaviourPunCallbacks
{
    public float sensitivity = 100f;
    public float angleMin = -45, angleMax = 45;

    public Transform transformPlayer;
    public float rotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Camera>().gameObject.SetActive(photonView.IsMine);
        if (!photonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, angleMin, angleMax);

        transform.localRotation = Quaternion.Euler(rotation, 0, 0);

        transformPlayer.Rotate(Vector3.up * mouseX);
    }
}
