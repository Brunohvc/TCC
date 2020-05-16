using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWeapon : MonoBehaviour
{
    public float value;
    public float softensValue;
    public float maxValue;
    Vector3 startinfPosition;


    // Start is called before the first frame update
    void Start()
    {
        startinfPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * value;
        float movementY = -Input.GetAxis("Mouse Y") * value;

        movementX = Mathf.Clamp(movementX, -maxValue, maxValue);
        movementY = Mathf.Clamp(movementY, -maxValue, maxValue);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + startinfPosition, Time.deltaTime * softensValue);
    }
}
