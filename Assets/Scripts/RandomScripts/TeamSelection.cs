using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TeamSelection : MonoBehaviour
{


	//public GameObject rangeHero;
	//public GameObject meleHero;
	public List<GameObject> heroes = new List<GameObject> ();
	public float currentarmyCost;
	public float maxArmyCost;

	public int[] armies;
	public GameObject[] panels; 

	public GameObject melePanel;
	public GameObject rangePanel;
	public Text meleCount;
	public Text rangeCount;
	public Text total;


	// Use this for initialization
	void Start ()
	{
		armies = new int[heroes.Count];
		panels = new GameObject[heroes.Count];
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void addUnit (int i)
	{
		HeroInfo heroInfo = heroes [i].GetComponent<HeroInfo> ();
		currentarmyCost += heroInfo.cost;
		armies [i] += 1;

		switch (i) {
		case 0:
			{
				float count = float.Parse (meleCount.text);				
				meleCount.text = (count + heroInfo.cost) + "";
				break;
			}
		case 1:
			{
				float count = float.Parse (rangeCount.text);	
				rangeCount.text = (count + heroInfo.cost) + "";
				break;
			}
		}
		updateTotalText ();

	}
	public void removeUnit (int i)
	{
		HeroInfo heroInfo = heroes [i].GetComponent<HeroInfo> ();
		currentarmyCost += heroInfo.cost;
		armies [i] -= 1;

		switch (i) {
		case 0:
			{
				float count = float.Parse (meleCount.text);
				meleCount.text = (count - heroInfo.cost) + "";
				break;
			}
		case 1:
			{
				float count = float.Parse (rangeCount.text);
				rangeCount.text = (count - heroInfo.cost) + "";
				break;
			}
		}
		updateTotalText ();
	}

	void updateTotalText ()
	{
		float totalValue = 0f;
		totalValue += float.Parse (meleCount.text);
		totalValue += float.Parse (rangeCount.text);
		total.text = totalValue + "\\" + maxArmyCost;
	}






}
