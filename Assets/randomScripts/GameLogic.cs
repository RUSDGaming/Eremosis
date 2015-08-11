using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{

	public enum player
	{
		player1,
		player2}
	;

	private HeroInfo selectedObject;



	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void SetSelectedObject (HeroInfo gameObject)
	{
		this.selectedObject = gameObject;
	}

	public void MoveSelected (TileInfo tileInfo)
	{
		if (this.selectedObject != null) {
			Vector3 movePos = new Vector3 (tileInfo.transform.position.x, tileInfo.transform.position.y + 1.5f, tileInfo.transform.position.z);
			selectedObject.transform.position = movePos;
		}
	}
}
