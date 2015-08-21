using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TileInfo : NetworkBehaviour
{
	public enum TileType
	{
		GRASS,
		VOID,
	}

	public TileType type;

	[SyncVar]
	public bool
		occupied = false;
	[SyncVar]
	public int
		x;
	[SyncVar]
	public int
		y;
	[SyncVar]
	public int
		z;
	[SyncVar]
	public int
		h;

	public GameObject UnitOnTile = null;

	Renderer theRenderer;

	private GameLogic logic;
	public Color startColor;

	// Use this for initialization
	void Start ()
	{
		theRenderer = gameObject.GetComponent<Renderer> ();
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnMouseEnter ()
	{
		startColor = theRenderer.material.color;

		if (GameLogicUtils.canHeroMoveToTile (logic.getSelectedObject (), this)) {
			theRenderer.material.color = Color.green;
		} else {
			theRenderer.material.color = Color.red;
		}

	}
	void OnMouseExit ()
	{
		//theRenderer.material.color = startColor;
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
