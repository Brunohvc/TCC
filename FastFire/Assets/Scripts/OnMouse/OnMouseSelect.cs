using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseSelect : MonoBehaviour
{
    public Material selected, notSelected;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        rend.material = selected;
    }

    private void OnMouseExit()
    {
        rend.material = notSelected;
    }
}
