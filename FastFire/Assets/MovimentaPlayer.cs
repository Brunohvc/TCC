using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovimentaPlayer : NetworkBehaviour
{

    public float Velocidade;
    public float Sensibilidade;
    public GameObject CameraObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            CameraObj.SetActive(false);
            return;
        }

        var x = Input.GetAxis("Horizontal") * Velocidade * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * Velocidade * Time.deltaTime;
        transform.Translate(x, 0, z);

        float mouseY = Input.GetAxis("Mouse X") * Sensibilidade;
        transform.Rotate(0, mouseY, 0);
    }
}
