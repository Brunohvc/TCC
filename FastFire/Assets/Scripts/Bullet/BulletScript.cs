using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour, IPunInstantiateMagicCallback
{

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;
	public string playerID = "";
	public string playerName = "";

	public float bulletSpeed = 400f;
	public Rigidbody rb;

	[Header("Impact Effect Prefabs")]
	public Transform[] metalImpactPrefabs;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = gameObject.transform.forward * bulletSpeed;
		//Start destroy timer
		StartCoroutine(DestroyAfter());
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		object[] instantiationData = info.photonView.InstantiationData;
		playerID = instantiationData[0].ToString();
		playerName = instantiationData[1].ToString();
	}

	//If the bullet collides with anything
	private void OnCollisionEnter(Collision collision)
	{
		//If destroy on impact is false, start 
		//coroutine with random destroy timer
		if (!destroyOnImpact)
		{
			StartCoroutine(DestroyTimer());
		}

		if (collision.transform.tag == "Player" 
			&& collision.gameObject.GetComponent<PhotonView>().IsMine)
		{

			collision.transform.gameObject.GetComponent
				<MovePlayer>().TakeDamage(playerID, playerName);
			this.GetComponent<PhotonView>().RPC("BulletDestroy", RpcTarget.AllViaServer);

		}

		Destroy(gameObject);
	}

	[PunRPC]
	void BulletDestroy()
    {
		Destroy(gameObject);
	}

	private IEnumerator DestroyTimer()
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		this.GetComponent<PhotonView>().RPC("BulletDestroy", RpcTarget.AllViaServer);
	}

	private IEnumerator DestroyAfter()
	{
		//Wait for set amount of time
		yield return new WaitForSeconds(destroyAfter);
		//Destroy bullet object
		this.GetComponent<PhotonView>().RPC("BulletDestroy", RpcTarget.AllViaServer);
	}
}
