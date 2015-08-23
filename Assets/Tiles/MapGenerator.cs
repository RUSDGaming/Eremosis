using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

//[ExecuteInEditMode]
public class MapGenerator : NetworkBehaviour
{

	
	public GameObject[,,] map = new GameObject[20, 100, 100];
	
	public GameObject hexTilePrefab = null;
	public GameObject voidTile = null;
	string filename = "BigFlatMap";
	private GameLogic logic;
	
	
	
	public void BuildMap ()
	{
		
		//StreamReader sr = new StreamReader (Application.dataPath + "/Tiles/" + filename);
		//string text = sr.ReadToEnd ();
		TextAsset textasset = (TextAsset)Resources.Load (filename, typeof(TextAsset));
		string text = textasset.text;
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
						SpawnTile (tile, x, y, z);
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


	public void SpawnTile (string tileString, int x, int y, int z)
	{
		if (tileString.Equals ("g")) {
			GameObject tileObject = (GameObject)Instantiate (hexTilePrefab, Vector3.zero, Quaternion.identity);
			map [y, z, x] = tileObject;
			tileObject.transform.position = new Vector3 (x * 1.7f + (z * .85f), y * .5f, z * 1.5f);
			tileObject.GetComponentInChildren<TileInfo> ().x = x;
			tileObject.GetComponentInChildren<TileInfo> ().z = z;
			tileObject.GetComponentInChildren<TileInfo> ().y = -x - z;
			tileObject.GetComponentInChildren<TileInfo> ().h = y;
			//NetworkServer.Spawn (tileObject);
			
		}

	}

	// Use this for initialization
	void Start ()
	{
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
