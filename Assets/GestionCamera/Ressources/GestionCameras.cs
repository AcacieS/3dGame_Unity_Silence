using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCameras : MonoBehaviour {

	private GameObject camActive;

	public GameObject camera1 ;   // il faut utiliser le type GameObject et non pas Camera
	public GameObject camera2;
	public GameObject camera3 ;
	public GameObject personnage;

	public enum lesCaméras
	{
		Camera1,
		Camera2,
		Camera3
	}

	public lesCaméras CameraDepart;

	private bool valeurTourneDepart;

	void Start()
	{
		if(personnage)
			{	
				valeurTourneDepart = personnage.GetComponent<ControleurPersonnage>().tourneAvecLeClavier;
			}

		if (camera1 != null) 
		{
			camActive = camera1;
			camera1.SetActive (false);
			camera1.tag = "MainCamera";
		}
		if (camera2 != null) 
		{
			camera2.SetActive (false);
			camera2.tag = "MainCamera";
		}
		if (camera3 != null) 
		{
			camera3.SetActive (false);
			camera3.tag = "MainCamera";
		}

		switch(CameraDepart)
		{
		case lesCaméras.Camera1:
			ActiverCamera (camera1);
				break;

			case lesCaméras.Camera2:
				ActiverCamera(camera2); 
				break;

			case lesCaméras.Camera3:
				ActiverCamera(camera3); 
				break;
		}

		
			
			 
	}

	void Update () 
	{
		
		if(Input.GetKeyDown("1") ) ActiverCamera(camera1);  
		if(Input.GetKeyDown("2") ) ActiverCamera(camera2); 
		if(Input.GetKeyDown("3") ) ActiverCamera(camera3); 


	}


	void ActiverCamera(GameObject cameraChoisie)
	{
		if (cameraChoisie != null)
		{
			camActive.SetActive (false);
			cameraChoisie.SetActive (true);
			camActive = cameraChoisie;

			if(personnage.GetComponent<ControleurPersonnage>())
			{
			
				if(personnage && cameraChoisie.GetComponent<ScriptCameraFPS>())
				{	
					personnage.GetComponent<ControleurPersonnage>().tourneAvecLeClavier=false;
					cameraChoisie.transform.forward = personnage.transform.forward;
				}
				else
				{
					personnage.GetComponent<ControleurPersonnage>().tourneAvecLeClavier=valeurTourneDepart;
				}
			}

			//var personnage = GameObject.FindWithTag("Player");
			//if (cameraChoisie.name.Contains("3") || cameraChoisie.GetComponent<ScriptCameraFPS>() )
				//personnage.GetComponent<ControlerPersonnage>().m_controlMode = ControlerPersonnage.ControlMode.Direct;
			//else
				//personnage.GetComponent<ControlerPersonnage>().m_controlMode = ControlerPersonnage.ControlMode.Tank;

		}

	}

}
