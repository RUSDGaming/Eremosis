using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamSelection : MonoBehaviour
{


	//public GameObject rangeHero;
	//public GameObject meleHero;
	public List<GameObject> heroes = new List<GameObject> ();
	public float currentarmyCost;
	public float maxArmyCost;

	public int[] armies;
	public GameObject[] panels; 

	public GameObject Panel;


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


	void modUnit (int i)
	{
		HeroInfo heroInfo = heroes [i].GetComponent<HeroInfo> ();
		currentarmyCost += heroInfo.cost * i;
		armies [i] += i;
	}




}
