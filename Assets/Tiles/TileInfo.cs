using UnityEngine;
using System.Collections;

public class TileInfo : MonoBehaviour
{
	public enum TileType
	{
		GRASS,
		VOID,
	}

	public TileType type;
	public bool occupied = false;
	public int x;
	public int y;
	public int z;
	public GameObject UnitOnTile = null;

	private GameLogic logic;

	// Use this for initialization
	void Start ()
	{
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnMouseOver ()
	{
		//Debug.Log ("mouse clicked");
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Right Mouse Clicked");
			logic.MoveSelected (this);
		}
	}
}
