using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VictoryConditions : NetworkBehaviour
{


	public GameObject VictoryMessagPanel;

	public Text text;
	private PlayerIdentity id;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void CheckVictoryConditions ()
	{
		GameObject[] heroes = GameObject.FindGameObjectsWithTag ("Hero");
		int[] heroTally = new int[2];
		
		foreach (GameObject go in heroes) {
			HeroInfo heroInfo = go.GetComponent<HeroInfo> ();
			if (heroInfo == null) {
				continue;
			}
			heroTally [heroInfo.player - 1] ++;
		}
		if (heroTally [0] <= 0) {
			Debug.Log ("player 1 lost");
		}
		if (heroTally [1] <= 0) {
			Debug.Log ("player 2 lost");
		}
		
	}
	[ClientRpc]
	public void RpcSetVictoryMessage (int winner)
	{
		if (id == null) {
			LoadId ();
		}

		if (id.PlayerNumber == winner) {
			text.text = "Victory!";
		} else {
			text.text = "Defeat!";
		}

		VictoryMessagPanel.SetActive (true);
	}

	public void LoadId ()
	{
		if (isClient) {
			GameObject go = NetworkManager.singleton.client.connection.playerControllers [0].gameObject;
			id = go.GetComponent<PlayerIdentity> () as PlayerIdentity;
			
		}
	}

}
