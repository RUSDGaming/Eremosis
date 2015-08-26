using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;

public class NetworkMaster : NetworkBehaviour
{

	static GameLogic logic;
	public static int numConnected;

	// Use this for initialization
	void Start ()
	{
		//logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void findLogic ()
	{
		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}
	}




	
	[Command]
	public void CmdMoveUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		Debug.Log ("cmdMove Unit");
		Debug.Log ("connection to client: " + connectionToClient.connectionId);
		GameObject go = connectionToClient.playerControllers [0].gameObject;
		PlayerIdentity pi = go.GetComponent<PlayerIdentity> ();

		Debug.Log ("player id:" + pi.PlayerNumber);
		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}

		HeroInfo heroInfo = logic.getUnitOnTile (aX, aZ, aH).GetComponent<HeroInfo> () as HeroInfo;
		TileInfo tileInfo = logic.getTile (bX, bZ, bH).GetComponentInChildren<TileInfo> () as TileInfo;

		if (heroInfo == null || tileInfo == null) {
			Debug.Log ("Something was null");
			return;
		}

		if (!logic.isPlayersTurn (getPlayerIdByConnection (connectionToClient))) {
			return;
		}

		if (heroInfo.player != getPlayerIdByConnection (connectionToClient)) {
			Debug.Log ("You don't own that player you fool");
			return;
		}

		if (GameLogicUtils.canHeroMoveToTile (heroInfo, tileInfo)) {
			// get the server to do this
			if (isServer && !isClient) {
				MoveUnit (aX, aZ, aH, bX, bZ, bH);
			}
			// get the clients to do this
			RpcMoveUnit (aX, aZ, aH, bX, bZ, bH);

		}
		
	}

	[ClientRpc]
	public void RpcMoveUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		MoveUnit (aX, aZ, aH, bX, bZ, bH);
	}

	public void MoveUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}


		
		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}
		GameObject hero = logic.getUnitOnTile (aX, aZ, aH);
		if (hero == null) {
			Debug.LogError ("hero was null in rpcMoveUnit");
			return;
		}
		
		
		GameObject tile = logic.getTile (bX, bZ, bH);
		if (tile == null) {
			Debug.LogError ("tile was null in rpcMoveUnit");
			return;
		}
		
		HeroInfo heroInfo = hero.GetComponent<HeroInfo> ();
		TileInfo tileInfo = tile.GetComponentInChildren<TileInfo> ();

		if (logic.playerMoved) {
			Debug.Log ("player Already Moved");
			return;
		}

	
		
		Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
		heroInfo.tile.occupied = false;
		heroInfo.tile.UnitOnTile = null;
		heroInfo.tile = tileInfo;
		heroInfo.transform.position = movePos;
		tileInfo.UnitOnTile = heroInfo.gameObject;

		logic.MoveFinished ();
	}






	[Command]
	public void CmdAttackUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{

		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}
		if (!logic.isPlayersTurn (getPlayerIdByConnection (connectionToClient))) {
			return;
		}

		GameObject heroAttacker = logic.getUnitOnTile (aX, aZ, aH);
		if (heroAttacker == null) {
			Debug.LogError ("hero was null in rpcMoveUnit");
			return;
		}
		GameObject heroDeffender = logic.getUnitOnTile (bX, bZ, bH);
		if (heroDeffender == null) {
			Debug.LogError ("hero was null in rpcMoveUnit");
			return;
		}

		HeroInfo heroInfoA = heroAttacker.GetComponent<HeroInfo> ();
		HeroInfo heroInfoB = heroDeffender.GetComponent<HeroInfo> ();

		if (heroInfoA == null) {
			Debug.LogError ("Hero Attacker was null");
			return;
		}

		if (heroInfoB == null) {
			Debug.LogError ("Hero Deffender was null");
			return;
		}

		if (!logic.isPlayersTurn (getPlayerIdByConnection (connectionToClient))) {
			return;
		}
		
		if (heroInfoA.player != getPlayerIdByConnection (connectionToClient)) {
			Debug.Log ("You don't own that player you fool");
			return;
		}

		if (GameLogicUtils.canHeroAttackUnit (heroInfoA, heroInfoB)) {
			// get the server to do this
			if (isServer && !isClient) {
				AttackUnit (aX, aZ, aH, bX, bZ, bH);
			}			
			// get the client to do this..
			RpcAttackUnit (aX, aZ, aH, bX, bZ, bH);
			logic.AttackFinished ();
		}

	}

	public void AttackUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		if (logic == null) {
			logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		}
		
		GameObject heroAttacker = logic.getUnitOnTile (aX, aZ, aH);
		if (heroAttacker == null) {
			Debug.LogError ("hero was null in rpcMoveUnit");
			return;
		}
		GameObject heroDeffender = logic.getUnitOnTile (bX, bZ, bH);
		if (heroDeffender == null) {
			Debug.LogError ("hero was null in rpcMoveUnit");
			return;
		}
		
		HeroInfo heroInfoA = heroAttacker.GetComponent<HeroInfo> ();
		HeroInfo heroInfoB = heroDeffender.GetComponent<HeroInfo> ();
		
		if (heroInfoA == null) {
			Debug.LogError ("Hero Attacker was null");
			return;
		}
		
		if (heroInfoB == null) {
			Debug.LogError ("Hero Deffender was null");
			return;
		}
		
		float damage = heroInfoA.damage - heroInfoB.deffense;
		if (damage > 0) {
			heroInfoB.health -= damage;
		}
		if (heroInfoB.health < 0) {
			GameObject.Destroy (heroInfoB.gameObject);
		}
	}
	
	
	[ClientRpc]
	public void RpcAttackUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		AttackUnit (aX, aZ, aH, bX, bZ, bH);
	}

	[Command]
	public void CmdImReady ()
	{
		GameObject go = connectionToClient.playerControllers [0].gameObject;
		PlayerIdentity pi = go.GetComponent<PlayerIdentity> ();
		pi.ConnectionId = connectionToClient.connectionId;
		//numConnected ++;
		pi.PlayerNumber = Interlocked.Increment (ref numConnected);

	}

	public int getPlayerIdByConnection (NetworkConnection connection)
	{
		GameObject go = connection.playerControllers [0].gameObject;
		PlayerIdentity pi = go.GetComponent<PlayerIdentity> ();
		return  pi.PlayerNumber;
	}

	public int getNumConnected ()
	{
		numConnected ++;
		return numConnected;
	}

	[Command]
	public void CmdDoneWithTurn ()
	{

		if (logic.isPlayersTurn (getPlayerIdByConnection (connectionToClient))) {
			if (isServer && !isClient) {
				logic.TurnFinished ();
			}			
			// get the client to do this..
			RpcDoneWithTurn ();
		}

	}
	[ClientRpc]
	public void RpcDoneWithTurn ()
	{
		logic.TurnFinished ();
	}




}
