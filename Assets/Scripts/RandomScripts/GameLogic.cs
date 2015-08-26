using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameLogic : NetworkBehaviour
{

	public enum TurnState
	{
		MOVE,
		ATTACK
	}

	public Text TitleText;
	public Text AttackText;
	public Text DeffenseText;
	public Text RangeText;
	public Text MoveText;
	public Text HealthText;



	public List<List<GameObject>> armies = new List<List<GameObject>> ();
	private HeroInfo selectedObject;

	public bool mapLoaded = false;
	public GameObject Hero;

	public GameObject[,,] map;

	
	public MapGenerator mapGen;
	

	private int playerTurn = 1;
	public bool playerMoved = false;
	public bool playerAttacked = false;

	//private TurnState turnState = TurnState.MOVE;

	NetworkMaster networkMaster;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Generating Map");

		mapGen.BuildMap ();
		map = mapGen.map;
		StartCoroutine (GenerateArmies (2));
		
		loadNetworkObject ();

		if (isClient) {
			Debug.Log ("client gamelogic started");
			networkMaster.CmdImReady ();
		}

		if (isServer) {
			Debug.Log ("Server gamelogic started");
			//SetPlayerID ();
		}
	}


	public void loadNetworkObject ()
	{
		if (isClient) {
			GameObject go = NetworkManager.singleton.client.connection.playerControllers [0].gameObject;
			networkMaster = go.GetComponent<NetworkMaster> () as NetworkMaster;
			//networkMaster = (NetworkMaster)FindObjectOfType (typeof(NetworkMaster));
		}
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
	private IEnumerator GenerateArmies (int armyCount)
	{
		while (!mapLoaded) {
			yield return new WaitForSeconds (0);
		}

		//MapFromText mapfromtext = (MapFromText)FindObjectOfType (typeof(MapFromText));
		//map = mapfromtext.map;

		MapGenerator mapGenerator = (MapGenerator)FindObjectOfType (typeof(MapGenerator));
		map = mapGenerator.map;

		int heroCount = 8;
		for (int i =0; i <armyCount; i++) {
			armies.Add (new List<GameObject> ());
			for (int h = 0; h < heroCount; h++) {
				CreateHero (i, h);
			}

		}

	}
	public void CreateHero (int i, int h)
	{
		GameObject hero = (GameObject)Instantiate (Hero, Vector3.zero, Quaternion.identity);
		armies [i].Add (hero);
		//HeroInfo heroInfo = hero.GetComponent<HeroInfo> ();
		GameObject tile = map [0, i * 30, h];
		//Debug.Log (tile);
		TileInfo tileInfo = tile.GetComponentInChildren<TileInfo> ();
		hero.GetComponent<HeroInfo> ().tile = tileInfo;
		//heroInfo.tile = tileInfo;
		hero.GetComponent<HeroInfo> ().player = i + 1;
		//heroInfo.player = i + 1;
		
		Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
		tileInfo.UnitOnTile = hero;
		hero.transform.position = movePos;

		//NetworkServer.Spawn (hero);
	}


	public void SetSelectedObject (HeroInfo gameObject)
	{
		if (gameObject == this.selectedObject) {
			this.selectedObject = null;
			TitleText.text = "Unit Info (?)"; 
			AttackText.text = "Attack (?)"; 
			DeffenseText.text = "Deffense (?)"; 
			MoveText.text = "Move (?)"; 
			RangeText.text = "Range (?)"; 
			HealthText.text = "Health (?)"; 
			return;
		}
		this.selectedObject = gameObject;
		HeroInfo heroInfo = selectedObject.GetComponent<HeroInfo> ();

		if (heroInfo != null) {
			TitleText.text = "Unit Info (" + heroInfo.player + ")"; 
			AttackText.text = "Attack (" + heroInfo.damage + ")"; 
			DeffenseText.text = "Deffense (" + heroInfo.deffense + ")"; 
			MoveText.text = "Move (" + heroInfo.move + ")"; 
			RangeText.text = "Range (" + heroInfo.range + ")"; 
			HealthText.text = "Health (" + heroInfo.health + ")"; 
		}
	}
	public HeroInfo getSelectedObject ()
	{
		return this.selectedObject;
	}

	public void addHeroToTile (GameObject hero, GameObject tile)
	{
		HeroInfo heroInfo = hero.GetComponentInChildren<HeroInfo> ();
		if (heroInfo == null) {
			Debug.Log ("unit passed was not a hero");
			return;
		}

		TileInfo tileInfo = tile.GetComponent<TileInfo> ();
		if (tileInfo == null) {
			Debug.Log ("tile was not a tile");
			return;
		}

		heroInfo.tile = tileInfo;


		Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
		tileInfo.UnitOnTile = hero;
		hero.transform.position = movePos;

	}

	public void MoveSelected (TileInfo tileInfo)
	{

		Debug.Log ("Network Player is: " + Network.player.ToString ());
		if (mapLoaded)
		if (this.selectedObject != null) {
			networkMaster.CmdMoveUnit (selectedObject.tile.x, selectedObject.tile.h, selectedObject.tile.z, tileInfo.x, tileInfo.h, tileInfo.z);
		}
	}

	public void Attack (HeroInfo deffendant)
	{
		if (!mapLoaded) {
			return;
		}

		if (this.selectedObject != null) {
			networkMaster.CmdAttackUnit (selectedObject.tile.x, selectedObject.tile.h, selectedObject.tile.z, deffendant.tile.x, deffendant.tile.h, deffendant.tile.z);
		}



	}



	public GameObject getUnitOnTile (int x, int y, int z)
	{
		GameObject tileObject = getTile (x, y, z);
		if (tileObject == null) {
			Debug.LogError ("the tile you tried to get is not there");
			return null;
		}
		GameObject hero = tileObject.GetComponentInChildren<TileInfo> ().UnitOnTile;
		if (hero == null) {
			Debug.LogError ("There was no Hero on the tile");
			return null;
		}
		return hero;
	}


	public GameObject getTile (int x, int h, int z)
	{
		//Debug.Log ("getting tile, x:" + x + " z:" + z + " y/h:" + h);
		GameObject tileObject = map [h, z, x];
		if (tileObject == null) {
			Debug.LogError ("the tile you tried to get is not there");
			return null;
		}
		return tileObject;
	}

//	[Server]
//	void SetPlayerID ()
//	{
//
//		//GameObject go = NetworkManager.singleton.client.connection.playerControllers [0].gameObject;
//		List<NetworkConnection> connections = NetworkServer.connections;
//		int cnum = 0;
//		foreach (NetworkConnection c in connections) {
//			// forwhatever reason there are null connections in that list..... wtf
//			if (c != null) {
//				if (c.playerControllers != null) {
//					foreach (PlayerController pc in c.playerControllers) {
//
//						PlayerIdentity pi = pc.gameObject.GetComponent<PlayerIdentity> ();
//						cnum ++;
//						Debug.Log ("thhis many connectections:" + cnum);
//						Debug.Log ("c.connectionid: " + c.connectionId);
//						pi.PlayerNumber = cnum;
//						pi.ConnectionId = c.connectionId;
//
//					}
//				}
//			}
//		}
//	}


	public bool isPlayersTurn (int playerNum)
	{
		if (playerNum == playerTurn) {
			return true;
		}
		Debug.Log ("Its not :" + playerNum + "'s turn, it's :" + playerTurn + "'s turn");
		return false;
	}

	public void AttackFinished ()
	{
		playerAttacked = true;
		TurnFinished ();
	}

	public void MoveFinished ()
	{
		playerMoved = true;
		//TurnFinished ();
	}

	public void TurnFinished ()
	{
		if (playerTurn == 1) {
			playerTurn = 2;
		} else {
			playerTurn = 1;
		}

		playerAttacked = false;
		playerMoved = false;
	}
	public void DoneWithTurn ()
	{
		networkMaster.CmdDoneWithTurn ();
		Debug.Log ("Done with turn");
	}




}
