using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkMasterClient : NetworkBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	[Command]
	public void CmdMoveUnit ()
	{
		if (!Network.isServer) {
			return;
		}


	}

	[Command]
	public void CmdAttackUnit ()
	{
		if (!Network.isServer) {
			return;
		}
	}

	[ClientRpc]
	public void RpcMoveUnit ()
	{

	}

	[ClientRpc]
	public void RpcAttackUnit ()
	{

	}


}
