using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameLogic : NetworkBehaviour
{

	public enum TurnState
	{
		MOVE,
		ATTACK
	}


	public List<List<GameObject>> armies = new List<List<GameObject>> ();
	private HeroInfo selectedObject;

	public bool mapLoaded = false;
	public GameObject Hero;

	public GameObject[,,] map;


	public MapGenerator mapGen;
	

	//private int playerTurn = 1;

	//private TurnState turnState = TurnState.MOVE;

	NetworkMaster networkMaster;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Generating Map");
		mapGen.BuildMap ();
		map = mapGen.map;
		StartCoroutine (GenerateArmies (2));

		networkMaster = (NetworkMaster)FindObjectOfType (typeof(NetworkMaster));
		if (isClient) {
			Debug.Log ("client gamelogic started");
		}

		if (isServer) {
			Debug.Log ("Server gamelogic started");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	private IEnumerator GenerateArmies (int armyCount)
	{
		while (!mapLoaded) {
			yield return new WaitForSeconds (.01f);
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
		HeroInfo heroInfo = hero.GetComponent<HeroInfo> ();
		GameObject tile = map [0, i * 30, h];
		Debug.Log (tile);
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
			return;
		}
		this.selectedObject = gameObject;
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
		if (mapLoaded)
		if (this.selectedObject != null) {

			//NetworkMaster.CmdMoveUnit (selectedObject.gameObject, tileInfo.gameObject);
			networkMaster.CmdMoveUnit (selectedObject.tile.x, selectedObject.tile.h, selectedObject.tile.z, tileInfo.x, tileInfo.h, tileInfo.z);

		}
	}

	public void Attack (HeroInfo deffendant)
	{
		if (!mapLoaded) {
			return;
		}

		if (this.selectedObject == null) {
			return;
		}
		if (!GameLogicUtils.canHeroAttackUnit (selectedObject, deffendant)) {
			Debug.Log ("hero was to far away");
			return;
		}

		float damage = selectedObject.damage - deffendant.deffense;
		if (damage > 0) {
			deffendant.health -= damage;
		}
		if (deffendant.health < 0) {
			GameObject.Destroy (deffendant.gameObject);
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




}
