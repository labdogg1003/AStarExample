using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour 
{
	public Camera ActiveCamera;
	public Camera cam1;
	public Camera cam2;

	public void switchView()
	{
		if(cam1.enabled)
		{
			cam1.enabled = false;
			cam2.enabled = true;
			ActiveCamera = cam2;
		}
		else
		{
			cam2.enabled = false;
			cam1.enabled = true;
			ActiveCamera = cam1;
		}
	}

}
