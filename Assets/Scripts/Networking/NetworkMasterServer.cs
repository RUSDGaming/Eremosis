using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class NetworkMasterServer : MonoBehaviour
{

	public int MasterServerPort = 7777;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void StartServer (bool client)
	{
		if (client) {

		}
	}



	public void InitializeServer ()
	{
		if (NetworkServer.active) {
			Debug.LogError ("Server already started");
			return;
		}
		NetworkServer.Listen (MasterServerPort);




	}
}
