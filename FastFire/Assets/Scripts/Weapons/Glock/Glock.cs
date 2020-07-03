using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Glock : MonoBehaviourPunCallbacks
{
    Animator animator;
    bool shootting = false;
    RaycastHit hit;

    public GameObject spark;
    public GameObject hole;
    public GameObject smoke;
    public GameObject shotEffect;
    public GameObject positionShotEffect;

    public ParticleSystem bulletTrail;
    AudioSource audioGun;
    public AudioClip[] gunSounds;

    public int loader = 3;
    public int ammunition = 17;
    int defaultAmmunition = 0;
    int totalAmmunition = 0;

    UIManager uiScript;
    MoveWeapon moveWeaponScript;

    public bool automatic = false;
    public float sizeCrosshair = 300f;
    float defaultSizeCrosshair = 300f;

    public float bulletForce = 400.0f;
    public Transform bulletPrefab;
    public Transform bulletSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        defaultAmmunition = ammunition;
        totalAmmunition = defaultAmmunition * loader;
        animator = GetComponent<Animator>();
        audioGun = GetComponent<AudioSource>();
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UIManager>();
        moveWeaponScript = GetComponentInParent<MoveWeapon>();
        defaultSizeCrosshair = sizeCrosshair;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        ModifyCorsshair();
        uiScript.gunName.text = "Glock 18";

        if (animator.GetBool("actionOccurs")) return;

        Fire();
        Reload();
        ChangeTypeFire();
        VerifyCrossHair();
    }

    void Fire()
    {
        uiScript.ammunition.text = ammunition.ToString() + " / " + totalAmmunition.ToString();

        if (Input.GetButtonDown("Fire1") || automatic ? Input.GetButton("Fire1") : false)
        {
            if (!shootting && ammunition > 0)
            {
                Debug.Log("Atirou");
                audioGun.clip = gunSounds[0];
                audioGun.Play();
                // bulletTrail.Play();
                shootting = true;
                StartCoroutine(Shoot());
                ammunition--;

                var bullet = (Transform)Instantiate(
                    bulletPrefab,
                    bulletSpawnPoint.transform.position,
                    bulletSpawnPoint.transform.rotation);

                //Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity =
                    bullet.transform.forward * bulletForce;
            }
            else if (!shootting && ammunition == 0 && totalAmmunition > 0)
            {
                Reload(true);
            }
            else if (!shootting && ammunition == 0)
            {
                audioGun.clip = gunSounds[4];
                audioGun.Play();
            }
        }
    }
    void Reload(bool tryFire = false)
    {
        if ((Input.GetKeyDown(KeyCode.R) || tryFire) && totalAmmunition > 0 && ammunition < defaultAmmunition)
        {
            animator.Play("reload");

            totalAmmunition += ammunition;

            if (totalAmmunition - defaultAmmunition >= 0)
            {
                ammunition = defaultAmmunition;
                totalAmmunition -= defaultAmmunition;
            }
            else
            {
                ammunition = totalAmmunition;
                totalAmmunition = 0;
            }
        }
    }
    void ChangeTypeFire()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            audioGun.clip = gunSounds[1];
            audioGun.Play();
            automatic = !automatic;
            if (automatic)
            {
                uiScript.typeFire.sprite = uiScript.spriteTypeFire[1];
            }
            else
            {
                uiScript.typeFire.sprite = uiScript.spriteTypeFire[0];
            }
        }
    }
    IEnumerator Shoot()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        animator.Play("shoot");

        GameObject shootEffectObj = Instantiate(shotEffect, positionShotEffect.transform.position, positionShotEffect.transform.rotation);
        shootEffectObj.transform.parent = positionShotEffect.transform;

        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit))
        {
            InstantiateEffects();

            print(hit.transform.name);
            if (hit.transform.tag == "dragObject" || hit.transform.tag == "catchObject")
            {
                Vector3 shootDirection = ray.direction;

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(shootDirection * 500, hit.point);
                }
            }
        }

        yield return new WaitForSeconds(0.4f);

        shootting = false;
    }
    void InstantiateEffects()
    {
        Instantiate(spark, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(smoke, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject holeObj = Instantiate(hole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        holeObj.transform.parent = hit.transform;
    }
    void MagazineSound1()
    {
        audioGun.clip = gunSounds[1];
        audioGun.Play();
    }
    void MagazineSound2()
    {
        audioGun.clip = gunSounds[2];
        audioGun.Play();
    }
    void BulletInTheNeedle()
    {
        audioGun.clip = gunSounds[3];
        audioGun.Play();
    }
    void VerifyCrossHair()
    {
        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("crosshair", true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
            uiScript.corsshair.gameObject.SetActive(false);
            moveWeaponScript.value = 0.01f;
        }
        else
        {
            animator.SetBool("crosshair", false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
            uiScript.corsshair.gameObject.SetActive(true);
            moveWeaponScript.SetValueToDefault();
        }
    }

    void ModifyCorsshair()
    {
        if (shootting)
        {
            sizeCrosshair = Mathf.Lerp(sizeCrosshair, (defaultSizeCrosshair * 1.5f), Time.deltaTime * 20);
        }
        else
        {
            sizeCrosshair = Mathf.Lerp(sizeCrosshair, defaultSizeCrosshair, Time.deltaTime * 20);
        }
        uiScript.corsshair.sizeDelta = new Vector2(sizeCrosshair, sizeCrosshair);
    }
}
