using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{


	public Transform cameraTransform;
	public float speed = 5f;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		float xAxisValue = Input.GetAxis ("Horizontal");
		float zAxisValue = Input.GetAxis ("Vertical");

		cameraTransform.Translate (new Vector3 (xAxisValue * Time.deltaTime * speed, 0.0f, zAxisValue * Time.deltaTime * speed));
		
	
	}



}
