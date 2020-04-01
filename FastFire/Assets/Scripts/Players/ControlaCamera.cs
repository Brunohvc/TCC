using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlaCamera : MonoBehaviourPunCallbacks
{

    public GameObject Jogador;
    Vector3 distanciaCompensar;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        distanciaCompensar = transform.position - Jogador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        transform.position = Jogador.transform.position + distanciaCompensar;
    }
}
