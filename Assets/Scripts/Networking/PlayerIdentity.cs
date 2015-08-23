using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerIdentity : NetworkBehaviour
{

	[SyncVar]
	public int
		PlayerNumber;

	[SyncVar]
	public int 
		ConnectionId;
}
