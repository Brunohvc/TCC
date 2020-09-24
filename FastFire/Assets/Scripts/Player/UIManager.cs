using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    MovePlayer scripMovePlayer;
    public Slider stamina;
    public Text hp;
    public Text ammunition;
    public Text gunName;
    public Image typeFire;
    public Sprite[] spriteTypeFire;
    public RectTransform corsshair;

    [SerializeField]
    private Transform deathListContainer;
    [SerializeField]
    private GameObject playerListingDeathPrefab;


    // Start is called before the first frame update
    void Start()
    {
        scripMovePlayer = GetComponentInParent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        stamina.value = scripMovePlayer.stamina;
        hp.text = scripMovePlayer.hp.ToString();
    }

    public void addNewDeath(string playerKillerName, string playerDeathName)
    {
        Debug.Log("Death! Network");
        GameObject tempListing = Instantiate(playerListingDeathPrefab, deathListContainer);
        Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
        tempText.text = playerKillerName + "  Matou  " + playerDeathName;


        Debug.Log("Death string: " + tempText.text);

        StartCoroutine(removeDeathPlayer(tempListing));
    }

    IEnumerator removeDeathPlayer(GameObject item)
    {
        yield return new WaitForSeconds(5);
        Destroy(item);
    }
}
