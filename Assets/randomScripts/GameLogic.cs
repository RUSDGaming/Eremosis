using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{

	public enum player
	{
		player1,
		player2
	}


	public List<List<GameObject>> armies = new List<List<GameObject>> ();
	private HeroInfo selectedObject;
	public bool mapLoaded = false;
	private GameLogic logic ;
	public GameObject Hero;

	private GameObject[,,] map;

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

		MapFromText mapfromtext = (MapFromText)FindObjectOfType (typeof(MapFromText));
		map = mapfromtext.map;

		int heroCount = 8;
		for (int i =0; i <armyCount; i++) {
			armies.Add (new List<GameObject> ());
			for (int h = 0; h < heroCount; h++) {

				GameObject hero = (GameObject)Instantiate (Hero, Vector3.zero, Quaternion.identity);
				armies [i].Add (hero);
				HeroInfo heroInfo = hero.GetComponent<HeroInfo> ();
				GameObject tile = map [0, 0, h];
				TileInfo tileInfo = tile.GetComponentInChildren<TileInfo> ();
				heroInfo.tile = tileInfo;
				
				Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
				tileInfo.UnitOnTile = hero;
				hero.transform.position = movePos;



			}

		}

	}


	public void SetSelectedObject (HeroInfo gameObject)
	{
		this.selectedObject = gameObject;
	}

	public void MoveSelected (TileInfo tileInfo)
	{
		if (mapLoaded)
		if (this.selectedObject != null) {
			Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
			//tileInfo.GetComponent<HeroInfo> ().tile = tileInfo;
			selectedObject.tile = tileInfo;
			selectedObject.transform.position = movePos;
			tileInfo.UnitOnTile = selectedObject.gameObject;

		}
	}
}
