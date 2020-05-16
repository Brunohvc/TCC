using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragAndCatchRayCastScript))]
public class RayCastActions : MonoBehaviour
{
    DragAndCatchRayCastScript rayCastScript;
    public bool took;
    float distance;
    public GameObject saveObject;

    bool? oldgravityObject, oldKinematicObject;

    // Start is called before the first frame update
    void Start()
    {
        rayCastScript = GetComponent<DragAndCatchRayCastScript>();
        took = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = rayCastScript.targetDistance;

        if(distance <= 3)
        {
            if (Input.GetKeyDown(KeyCode.E) && rayCastScript.catchObject != null)
            {
                Catch();
            }

            if (Input.GetKeyDown(KeyCode.E) && rayCastScript.dragObject != null)
            {
                if (!took)
                {
                    took = true;
                    Drag();
                }
                else
                {
                    took = false;
                    Drop();
                }
            }
        }
    }

    void Catch()
    {
        Destroy(rayCastScript.catchObject);
    }

    void Drag()
    {
        oldKinematicObject = rayCastScript.dragObject.GetComponent<Rigidbody>().isKinematic;
        oldgravityObject = rayCastScript.dragObject.GetComponent<Rigidbody>().useGravity;

        rayCastScript.dragObject.GetComponent<Rigidbody>().isKinematic = true;
        rayCastScript.dragObject.GetComponent<Rigidbody>().useGravity = false;
        rayCastScript.dragObject.transform.SetParent(transform);
        rayCastScript.dragObject.transform.localPosition = new Vector3(0, 0, 3);
        rayCastScript.dragObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Drop()
    {
        var gravity = oldgravityObject.GetValueOrDefault(true);
        var kinematic = oldKinematicObject.GetValueOrDefault(false);
        rayCastScript.dragObject.GetComponent<Rigidbody>().isKinematic = kinematic;
        rayCastScript.dragObject.GetComponent<Rigidbody>().useGravity = gravity;
        rayCastScript.dragObject.transform.localPosition = new Vector3(0, 0, 3);
        rayCastScript.dragObject.transform.SetParent(null);

        oldKinematicObject = null;
        oldgravityObject = null;
    }
}
