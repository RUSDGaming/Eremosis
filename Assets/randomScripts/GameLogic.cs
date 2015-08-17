using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
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

	private GameObject[,,] map;

	private int playerTurn = 1;

	private TurnState turnState = TurnState.MOVE;


	// Use this for initialization
	void Start ()
	{
		StartCoroutine (GenerateArmies (2));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	private IEnumerator GenerateArmies (int armyCount)
	{
		while (!mapLoaded) {
			yield return new WaitForSeconds (.2f);
		}

		//MapFromText mapfromtext = (MapFromText)FindObjectOfType (typeof(MapFromText));
		//map = mapfromtext.map;

		MapGenerator mapGenerator = (MapGenerator)FindObjectOfType (typeof(MapGenerator));
		map = mapGenerator.map;

		int heroCount = 8;
		for (int i =0; i <armyCount; i++) {
			armies.Add (new List<GameObject> ());
			for (int h = 0; h < heroCount; h++) {

				GameObject hero = (GameObject)Instantiate (Hero, Vector3.zero, Quaternion.identity);
				armies [i].Add (hero);
				HeroInfo heroInfo = hero.GetComponent<HeroInfo> ();
				GameObject tile = map [0, i * 30, h];
				Debug.Log (tile);
				TileInfo tileInfo = tile.GetComponentInChildren<TileInfo> ();
				heroInfo.tile = tileInfo;
				heroInfo.player = i + 1;
				
				Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
				tileInfo.UnitOnTile = hero;
				hero.transform.position = movePos;



			}

		}

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

			if (canHeroMoveToTile (selectedObject, tileInfo)) {
				Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
				//tileInfo.GetComponent<HeroInfo> ().tile = tileInfo;
				selectedObject.tile = tileInfo;
				selectedObject.transform.position = movePos;
				tileInfo.UnitOnTile = selectedObject.gameObject;
			}

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
		float damage = selectedObject.damage - deffendant.deffense;
		if (damage > 0) {
			deffendant.health -= damage;
		}
		if (deffendant.health < 0) {
			GameObject.Destroy (deffendant.gameObject);
		}

	}


	//http://www.redblobgames.com/grids/hexagons/#range
	public bool canHeroMoveToTile (HeroInfo heroInfo, TileInfo tileInfo)
	{
		if (!mapLoaded) {
			Debug.Log ("map isnt loaded yet");
			return false;
		}

		if (heroInfo == null || tileInfo == null) {
			Debug.Log ("something was null");
			return false;
		}
		TileInfo heroTile = heroInfo.tile;

		int dX = tileInfo.x - heroTile.x;
		int dY = tileInfo.y - heroTile.y;
		int dZ = tileInfo.z - heroTile.z;

//		float d = Mathf.Pow (dX, 2) + Mathf.Pow (dY, 2) + Mathf.Pow (dZ, 2);
//		Debug.Log ("dist is " + d);
//		if (d <= Mathf.Pow (heroInfo.move, 2)) {
//			return true;
//		}

		float d = Mathf.Abs (dX) + Mathf.Abs (dY) + Mathf.Abs (dZ);
		Debug.Log (d);
		d = d / 2;
		Debug.Log ("distance: " + d);
		Debug.Log ("hero move: " + heroInfo.move);

		if (d <= heroInfo.move) {
			return true;
		}

		return false;
	}




}
