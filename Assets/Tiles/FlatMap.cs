using UnityEngine;
using System.Collections;

public class FlatMap : MonoBehaviour
{
	public int width = 10;
	public int height = 10;

	public float HexSideLenght = 1f;

	public GameObject hexTilePrefab = null;

	public void GenerateGrid ()
	{
		// 1.7 is the x delta
		// 1.5 is the z delta

		if (hexTilePrefab != null) {
			for (int x = 0; x < width; x ++) {
				for (int z = 0; z < height; z ++) {
					GameObject tile = (GameObject)Instantiate (hexTilePrefab, Vector3.zero, Quaternion.identity);
					if (z % 2 == 0) {
						tile.transform.position = new Vector3 (x * 1.7f, 0, z * 1.5f);
					} else {
						tile.transform.position = new Vector3 (x * 1.7f + .85f, 0, z * 1.5f);
					}

				}
			}

			//GameObject tile2 = (GameObject)Instantiate (hexTilePrefab, Vector3.zero, Quaternion.identity);
			//tile2.transform.position = new Vector3 (.85f, tile2.transform.position.y, 1.5f);

			//GameObject tile3 = (GameObject)Instantiate (hexTilePrefab, Vector3.zero, Quaternion.identity);
			//tile3.transform.position = new Vector3 (-.85f, tile3.transform.position.y, 1.5f);

		}

	}

	// Use this for initialization
	void Start ()
	{
		GenerateGrid ();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
