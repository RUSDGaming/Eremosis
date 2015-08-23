using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkLobby : NetworkBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("myNetworkLoby");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	private int playerCount = 0;
	void OnPlayerConnected (NetworkPlayer player)
	{
		Debug.Log ("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);
		playerCount ++;
	}

	void OnConnectedToServer ()
	{
		Debug.Log ("Connected to server");
	}
	
}
