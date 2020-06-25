using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DragAndCatchRayCastScript : MonoBehaviourPunCallbacks
{
    public float targetDistance;
    public GameObject dragObject, catchObject;
    RaycastHit hit;

    public Text buttonText, infoText;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Time.frameCount % 5 == 0)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);

            if(Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5))
            {
                targetDistance = hit.distance;

                if(hit.transform.gameObject.tag == "dragObject")
                {
                    dragObject = hit.transform.gameObject;
                    catchObject = null;

                    buttonText.text = "[E]";
                    infoText.text = "Arrasta/Solta";
                }

                if (hit.transform.gameObject.tag == "catchObject")
                {
                    catchObject = hit.transform.gameObject;
                    dragObject = null;

                    buttonText.text = "[E]";
                    infoText.text = "Pega";
                }
            }
            else
            {
                dragObject = null;
                catchObject = null;
                buttonText.text = "";
                infoText.text = "";
            }
        }
    }
}
