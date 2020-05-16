using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Photon.Pun;


public class PlayerS : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public InputStr Input;
    public struct InputStr
    {
        public float LookX;
        public float LookZ;
        public float RunX;
        public float RunZ;
        public bool Jump;

        public bool Shoot;
        public Vector3 ShootTarget;
    }

    public float Speed = 10f;
    public const float JumpForce = 7f;

    protected Rigidbody Rigidbody;
    protected Quaternion LookRotation;

    public AudioClip AudioClipAK;
    public AudioClip AudioClipShot;

    protected Animator Animator;
    protected float Cooldown;
    protected AudioSource AudioSourcePlayer;
    protected ParticleSystem ParticleSystem;

    public int HP;
    public bool IsDead { get { return HP <= 0; } }
    [HideInInspector]
    public bool Debug = false;
    public const int AllButIgnoreLayer = 0b11111011;

    private void Awake()
    {
        if (!photonView.IsMine) return;

        HP = 100;
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        AudioSourcePlayer = GetComponent<AudioSource>();
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }


    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (IsDead)
            return;

        var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

        Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

        //rotation to go target
        if (inputLook.magnitude > 0.01f)
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        transform.rotation = LookRotation;
    }

    void Update()
    {
        /*
        if (!photonView.IsMine) return;
        if (IsDead)
            return;

        Cooldown -= Time.deltaTime;

        if (Input.Shoot || Debug)
        {
            if (Cooldown <= 0 || Debug)
            {
                var shootVariation = UnityEngine.Random.insideUnitSphere;

                Animator.SetTrigger("Shoot");
                if (Animator.GetBool("AK") == true)
                {
                    AudioSourcePlayer.PlayOneShot(AudioClipAK);
                    Cooldown = 0.2f;
                    shootVariation *= 0.02f;
                }
                else
                {
                    AudioSourcePlayer.PlayOneShot(AudioClipShot);
                    Cooldown = 1f;
                    shootVariation *= 0.01f;
                }

                var shootOrigin = transform.position + Vector3.up * 1.5f;
                var shootDirection = (Input.ShootTarget - shootOrigin).normalized;
                var shootRay = new Ray(shootOrigin, shootDirection + shootVariation);


                //do we hit anybody?
                var hitInfo = new RaycastHit();
                gameObject.layer = Physics.IgnoreRaycastLayer;
                if (Physics.SphereCast(shootRay, 0.1f, out hitInfo, PlayerS.AllButIgnoreLayer))
                {
                    UnityEngine.Debug.DrawLine(shootRay.origin, hitInfo.point, Color.red);

                    var player = hitInfo.collider.GetComponent<PlayerS>();
                    if (player != null && !Debug && player != this)
                    {
                        player.OnHit();
                    }
                }
                gameObject.layer = 0;


            }
        }


        var charVelo = Quaternion.Inverse(transform.rotation) * Rigidbody.velocity;
        Animator.SetFloat("SpeedForward", charVelo.z);
        Animator.SetFloat("SpeedSideward", charVelo.x * Mathf.Sign(charVelo.z + 0.1f));*/
    }

    private void OnHit()
    {
        /*
        if (!photonView.IsMine) return;
        if (IsDead)
            return;

        HP = HP - 10;
        ParticleSystem.Stop();
        ParticleSystem.Play();
        if (IsDead)
            Die();
        //Activate Particles*/
    }

    private void Die()
    {
        // Activate Ragdoll Mode
        // GetComponent<PlayerRagdoll>().RadollSetActive(true);
    }
}
