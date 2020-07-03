using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffects : MonoBehaviour
{
    public float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, time);
    }
}
