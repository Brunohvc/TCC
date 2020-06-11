using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
