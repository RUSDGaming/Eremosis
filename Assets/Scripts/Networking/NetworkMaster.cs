using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkMaster : NetworkBehaviour
{

	static GameLogic logic;

	// Use this for initialization
	void Start ()
	{
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}





	
	[Command]
	public void CmdMoveUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{
		Debug.Log ("cmdMove Unit");
		//if (!Network.isServer) {
		//	return;
		//}

		HeroInfo heroInfo = logic.getUnitOnTile (aX, aZ, aH).GetComponent<HeroInfo> () as HeroInfo;
		TileInfo tileInfo = logic.getTile (bX, bZ, bH).GetComponentInChildren<TileInfo> () as TileInfo;

		if (heroInfo == null || tileInfo == null) {
			Debug.Log ("Something was null");
			return;
		}

		if (GameLogicUtils.canHeroMoveToTile (heroInfo, tileInfo)) {
			RpcMoveUnit (aX, aZ, aH, bX, bZ, bH);
		}
		
		
	}

	//[ClientRpc]
	public void RpcMoveUnit (HeroInfo heroInfo, TileInfo tileInfo)
	{
		Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
		heroInfo.tile.occupied = false;
		heroInfo.tile.UnitOnTile = null;
		heroInfo.tile = tileInfo;
		heroInfo.transform.position = movePos;
		tileInfo.UnitOnTile = heroInfo.gameObject;
	}

	[ClientRpc]
	public  void RpcMoveUnit (int aX, int aZ, int aH, int bX, int bZ, int bH)
	{

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

		Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
		heroInfo.tile.occupied = false;
		heroInfo.tile.UnitOnTile = null;
		heroInfo.tile = tileInfo;
		heroInfo.transform.position = movePos;
		tileInfo.UnitOnTile = heroInfo.gameObject;

	}





	[Command]
	public void CmdAttackUnit ()
	{
		if (!Network.isServer) {
			return;
		}
	}
	
	
	[ClientRpc]
	public void RpcAttackUnit ()
	{
		
	}
}
