using UnityEngine;
using System.Collections;

public class HeroInfo : MonoBehaviour
{

	public float health;
	public float deffense;
	public float move;
	public float height;

	private GameLogic logic;

	public TileInfo tile;


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

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("unit was clicked");
			logic.SetSelectedObject (this);
		}
	}


}
