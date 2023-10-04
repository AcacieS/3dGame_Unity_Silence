using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraSuitMoi : MonoBehaviour {
	public float hauteurFocus  = 1f;
	// The target we are following
	public Transform cible ;
	// The distance in the x-z plane to the target
	public float distance = 10f;
	// the height we want the camera to be above the target
	public float hauteur = 5f;
	// How much we 
	public float amortissementHauteur = 2f;
	public float amortissementRotation = 3f;

	void Start()
	{
		//cible.gameObject.GetComponent<ControlerPersonnage>().m_controlMode = ControlerPersonnage.ControlMode.Tank;
		gameObject.tag = "MainCamera";

	}
	void LateUpdate () {
		// Early out if we don't have a target
		if (!cible)
			return;

		//cible.GetComponent<ControlerPersonnage>().m_controlMode = ControlerPersonnage.ControlMode.Tank;
		// Calculate the current rotation angles
		var wantedRotationAngle = cible.eulerAngles.y;
		var wantedHeight = cible.position.y + hauteur;

		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, amortissementRotation * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, amortissementHauteur * Time.deltaTime);

		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = cible.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		var posActu = transform.position;
		posActu.y = currentHeight;
		transform.position = posActu;
		// Always look at the target

		//transform.LookAt(tagetFocus);

		// ajout par Vahik pour pouvoir modifier la hauteur du Focus pour ne pas regarder les pieds	
		var relativePos = cible.position - transform.position;
		relativePos.y += hauteurFocus;
		var rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
	}
}
