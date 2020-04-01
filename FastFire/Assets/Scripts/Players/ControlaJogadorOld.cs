using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlaJogadorOld : MonoBehaviourPunCallbacks
{
    public float Velocidade = 10;
    Vector3 direcao;

    public GameObject camera;
    Vector3 distanciaCompensar;

    void Start()
    {
        camera.SetActive(photonView.IsMine);
        if (!photonView.IsMine) return;

        distanciaCompensar = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

        // transform.Translate(direcao * Velocidade * Time.deltaTime);

        if (direcao != Vector3.zero)
        {
            // GetComponent<Animator>().SetBool("Movendo", true);
        }
        else
        {
            // GetComponent<Animator>().SetBool("Movendo", false);
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        GetComponent<Rigidbody>().MovePosition
            (GetComponent<Rigidbody>().position +
            (direcao * Velocidade * Time.deltaTime));

        camera.transform.position = transform.position + distanciaCompensar;
    }
}
