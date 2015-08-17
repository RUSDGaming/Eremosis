
using UnityEngine;
using System.Collections;
using System.IO;


public class MapFromText : MonoBehaviour
{

	
	public GameObject[,,] map = new GameObject[20, 100, 100];

	public GameObject hexTilePrefab = null;
	public GameObject voidTile = null;
	string filename = "BigFlatMap.txt";
	private GameLogic logic;



	IEnumerator BuildMap ()
	{

		StreamReader sr = new StreamReader (Application.dataPath + "/Tiles/" + filename);
		string text = sr.ReadToEnd ();
		string[] levels = text.Split ('&');
		//Debug.Log ("There are this many levels: " + levels.Length);

		int y = 0;
		if (hexTilePrefab != null) {
			foreach (string level in levels) {
				//Debug.Log (level);
				string[] rows = level.Split (new string[]{"\r\n","\n"}, System.StringSplitOptions.None);
				//Debug.Log ("rows in level : " + rows.Length);
				int z = 0;
				foreach (string row in rows) {
					string[] tiles = row.Split (new string[]{","}, System.StringSplitOptions.None);
					//Debug.Log ("tiles in row: " + tiles.Length);
					int x = 0;
					foreach (string tile in tiles) {
						//Debug.Log ("tile: " + tile);
						if (tile.Equals ("g")) {
							GameObject tileObject = (GameObject)Instantiate (hexTilePrefab, Vector3.zero, Quaternion.identity);
							tileObject.GetComponentInChildren<TileInfo> ().x = x;
							tileObject.GetComponentInChildren<TileInfo> ().y = y;
							tileObject.GetComponentInChildren<TileInfo> ().z = z;
							map [y, z, x] = tileObject;
							if (z % 2 == 0) {
								tileObject.transform.position = new Vector3 (x * 1.7f, y * .5f, z * 1.5f);
							} else {
								tileObject.transform.position = new Vector3 (x * 1.7f + .85f, y * .5f, z * 1.5f);
							}
							//yield return new WaitForSeconds (0.000f);
						}
						if (tile.Equals ("v")) {
							GameObject tileObject = (GameObject)Instantiate (voidTile, Vector3.zero, Quaternion.identity);
							tileObject.GetComponentInChildren<TileInfo> ().x = x;
							tileObject.GetComponentInChildren<TileInfo> ().y = y;
							tileObject.GetComponentInChildren<TileInfo> ().z = z;
							map [y, z, x] = tileObject;
							if (z % 2 == 0) {
								tileObject.transform.position = new Vector3 (x * 1.7f, y * .5f, z * 1.5f);
							} else {
								tileObject.transform.position = new Vector3 (x * 1.7f + .85f, y * .5f, z * 1.5f);
							}
							yield return new WaitForSeconds (0.00f);
						}
						x += 1;

					}
					if (! string.IsNullOrEmpty (row)) {
						z += 1;
					}
				}
				y += 1;
				//Debug.Log (text);
			}
			
		}

		logic.mapLoaded = true;
	}

	// Use this for initialization
	void Start ()
	{
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		StartCoroutine (BuildMap ());
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	
	}
}
