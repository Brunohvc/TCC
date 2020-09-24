using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

public class GameSetupController : MonoBehaviour
{
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        System.Random randNum = new System.Random();

        var x = randNum.Next(-80, 100);
        var y = 35;
        var z = randNum.Next(-90, 80);

        var posicao = new Vector3(x, y, z);

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), posicao, Quaternion.identity);
    }
}
