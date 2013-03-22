using UnityEngine;
using System.Collections;

public class ShipDetection : MonoBehaviour
{
	RaycastHit hit = new RaycastHit();
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void OnGUI ()
	{
		Ray ray = new Ray(this.transform.position,-Vector3.up);
		
        if (Physics.Raycast(ray, out hit, 200.0f))
		{
			if(hit.collider.name == "Ship")
			{
				GUI.Box(new Rect(450,50, 400, 150), "You're above a ship!\nRescue mode will be added here");
			}
		}
	}
}
