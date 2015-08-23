using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HeroInfo : NetworkBehaviour
{

	public float health;
	public float deffense;
	public float damage;
	public float move;
	public float height;
	public float range;
	public float cost;
	[SyncVar]
	public int
		player;
	private GameLogic logic;
	public TileInfo tile;
	Renderer theRenderer;
	public Material player1Mat;
	public Material player2Mat;

	Color startColor;

	// Use this for initialization
	void Start ()
	{	
		Debug.Log ("hero info was started");
		logic = (GameLogic)FindObjectOfType (typeof(GameLogic));
		theRenderer = gameObject.GetComponent<Renderer> ();

		switch (player) {
		case 1:
			{
				
				theRenderer.material = player1Mat;
				break;
			}

		case 2:
			{
				theRenderer.material = player2Mat;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

	
	}

	void OnMouseEnter ()
	{
		startColor = theRenderer.material.color;
		theRenderer.material.color = Color.magenta;
	}
	void OnMouseExit ()
	{
		theRenderer.material.color = startColor;
	}

	void OnMouseOver ()
	{

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("unit was clicked");
			logic.SetSelectedObject (this);
		}
		if (Input.GetMouseButtonDown (1)) {
			logic.Attack (this);
		}
	}


}
