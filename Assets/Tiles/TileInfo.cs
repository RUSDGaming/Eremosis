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
	public int h;
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

		if (logic.canHeroMoveToTile (logic.getSelectedObject (), this)) {
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
