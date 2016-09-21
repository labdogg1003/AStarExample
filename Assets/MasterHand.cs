using UnityEngine;
using System.Collections;

public class MasterHand : MonoBehaviour 
{
	public CameraSwitcher switcher;
	GameObject controlledObject;
	public float moveSpeed = 2;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = switcher.ActiveCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast (ray, out hit))
			{
				if(hit.transform.CompareTag("moveable"))
				{
					controlledObject = hit.transform.gameObject;
				}
			}
		}

		if(controlledObject != null)
		{
			controlledObject.transform.Translate(-moveSpeed * Input.GetAxis("Horizontal")*Time.deltaTime,0f,moveSpeed * -Input.GetAxis("Vertical")*Time.deltaTime);
		}
	}


}
