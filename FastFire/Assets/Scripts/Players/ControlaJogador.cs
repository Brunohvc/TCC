using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControlaJogador : MonoBehaviourPunCallbacks
{
    public float Velocidade = 10;
    Vector3 direcao;

    public GameObject camera;
    public GameObject character;
    public Animator animator;
    Vector3 distanciaCompensar;

    Actions actions;
    PlayerController controller;

    void Start()
    {
        // camera.SetActive(photonView.IsMine);
        // if (!photonView.IsMine) return;

        actions = character.GetComponent<Actions>();
        controller = character.GetComponent<PlayerController>();

        distanciaCompensar = camera.transform.position - transform.position;

        controller.SetArsenal("Rifle");
    }

    // Update is called once per frame
    void Update()
    {
        // if (!photonView.IsMine) return;
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

        // transform.Translate(direcao * Velocidade * Time.deltaTime);

        if (direcao != Vector3.zero)
        {
            // GetComponent<Animator>().SetBool("Movendo", true);
            //animator.SetFloat("Blend", 1);
            actions.SendMessage("Run", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            // GetComponent<Animator>().SetBool("Movendo", false);
            //animator.SetFloat("Blend", 0);
            actions.SendMessage("Stay", SendMessageOptions.DontRequireReceiver);
        }
    }

    void FixedUpdate()
    {
        // if (!photonView.IsMine) return;

        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (direcao * Velocidade * Time.deltaTime));

        camera.transform.position = transform.position + distanciaCompensar;
    }
}
