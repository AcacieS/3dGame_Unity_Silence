using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

public class ScriptCameraFPS : MonoBehaviour {

	public GameObject cible;
	public float vitesseRotHor = 15F;
	public float vitesseRotVer = 15F;

	public float angleMinVerticale = -40F;
	public float angleMaxVerticale = 60F;

	float rotationY = 0F;

	public float hauteur= 2.5f;
    float vit ;//de deplacement
    float inputH;

	void Update ()
	{
        
		if(cible.GetComponent<ControleurPersonnage>())
		{
			inputH = Input.GetAxis("Horizontal");
			cible.GetComponent<ControleurPersonnage>().tourneAvecLeClavier=false;
			vit = cible.GetComponent<ControleurPersonnage>().vitesseMarche;
			cible.transform.Translate(new Vector3(inputH * vit*Time.deltaTime,0f,0f));
		}
		
        //cible.GetComponent<ControlerPersonnage>().m_controlMode = ControlerPersonnage.ControlMode.Direct;

        transform.position = new Vector3 (cible.transform.position.x, cible.transform.position.y + hauteur, cible.transform.position.z);
		
		//rotation personage
		cible.transform.Rotate(0, Input.GetAxis("Mouse X") * vitesseRotHor, 0);

		//rotation camera
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * vitesseRotHor;
		rotationY += Input.GetAxis("Mouse Y") * vitesseRotVer;
		rotationY = Mathf.Clamp (rotationY, angleMinVerticale, angleMaxVerticale);
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
      
        //cible.transform.position = new Vector3(Input.GetAxis("Horizontal")*vit, transform.position.y, transform.position.z);
      

	}
	
	void Start ()
	{
		gameObject.tag = "MainCamera";
		transform.forward = cible.transform.forward;	
       
    }

}