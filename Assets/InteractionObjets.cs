using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
Dernière modification : 19 octobre 2017 par Mathieu

Un ou plusieurs objets doivent être trouveés pour pouvoir activer l'objet à activer
   Lorsqu'un objet est trouvé alors :
        il devient invisible et non détectable, 
        s'il possède un son alors le son joue 
        s'il possède des enfants alors ils s'activent, exemple: des particules

   Lorsqu'on touche l'objet à activer, si tous les objets sont trouvés alors:
     s'il possède un son alors le son joue 
     s'il possède une animation alors elle joue
   
   S'il y a une caméra alors elle s'active lorsque tous les objets sont trouvés.
*/
public class InteractionObjets : MonoBehaviour
{


    public GameObject[] ObjetsATrouver;
    public bool ObjetTrouveDevientInvisible;
    public bool Objets_A_ActiverImmediatement = true;
    public GameObject[] Objets_A_Activer;
    public GameObject[] Objets_A_Desactiver;

    public GameObject Camera_A_Activer;
    public int TempsCamera = 5;
    public int TempsCameraAvantActivation = 2;

    int NombreObjetTrouve = 0;


    void Start()
    {   
        if (Camera_A_Activer) Camera_A_Activer.tag = "MainCamera";
    }

    void Update()
  { 
       RaycastHit hit;
       var auSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out hit, .8f);

       //suit le platforme
		if(auSol == true)
        {
        	if(hit.collider.tag.Contains("form") || hit.collider.gameObject.name.Contains("form") )
			{
				transform.parent = hit.collider.gameObject.transform;
			}
        }
        else
        {
            transform.parent = null;
        }
  }

    void OnCollisionEnter(Collision hit)
    {
        //*********** Trouver les objets *********************
        if (ObjetsATrouver.Length > 0)
        {
            for (var i = 0; i < ObjetsATrouver.Length; i++)
            {
                var objet = ObjetsATrouver[i];

                if (objet != null)
                {
                    if (hit.collider.name == objet.name)
                    {

                        ObjetsATrouver[i] = null;
                        if (ObjetTrouveDevientInvisible)   // si objet à rammesser alors rendre invisible
                        {

                            if (objet.GetComponent<Renderer>() != null)
                            {

                                objet.GetComponent<Renderer>().enabled = false;
                                objet.GetComponent<Collider>().enabled = false;
                                //rend les enfants invisibles
                                var rendererDesEnfants = objet.GetComponentsInChildren<Renderer>();
                                foreach (Renderer enfant in rendererDesEnfants)
                                {
                                    enfant.GetComponent<Renderer>().enabled = false;
                                }
                            }
                        }
                        objet.SetActive(true);

                        if (objet.GetComponent<Animator>()) objet.GetComponent<Animator>().enabled = true;
                        if (objet.GetComponent<AudioSource>()) objet.GetComponent<AudioSource>().Play();

                        foreach (Transform enfant in objet.transform)
                        {
                          
                            enfant.gameObject.SetActive(true);
                        }
                           
                        NombreObjetTrouve++;

                        //on peut activer la camera si on a trouvé le dernier objet
                        if (Camera_A_Activer != null && NombreObjetTrouve == ObjetsATrouver.Length)
                        {
                            StartCoroutine(ActiverCamera());
                        }



                    }
                }
            }
            //*********** Quand tous les objets ont été trouvés *********************
            if (NombreObjetTrouve == ObjetsATrouver.Length && Objets_A_Activer.Length != 0)
            {
                if (Objets_A_ActiverImmediatement)
                { // immédiatement
                  // *********** activation des objets ***********
                    for (var j = 0; j < Objets_A_Activer.Length; j++)
                    {
                        var objetActiver = Objets_A_Activer[j];

                        if (objetActiver != null)
                        {
                            objetActiver.SetActive(true);
                          
                            if (objetActiver.GetComponent<Animator>()) objetActiver.GetComponent<Animator>().enabled = true;
                            if (objetActiver.GetComponent<AudioSource>()) objetActiver.GetComponent<AudioSource>().Play();
                            foreach (Transform enfant in objetActiver.transform)
                            {
                                enfant.gameObject.SetActive(true);
                            }
                            NombreObjetTrouve = -1; // pour empêcher d'activer plusieurs fois cet objet
                        }
                    }
                    // *********** desactivation des objets ***********
                    for (var l = 0; l < Objets_A_Desactiver.Length; l++)
                    {
                        var objetDesActiver = Objets_A_Desactiver[l];

                        if (objetDesActiver != null)
                        {
                            objetDesActiver.SetActive(false);
                        }
                    }
                }
                else
                { // sinon on doit le toucher
                    if (hit.collider.name == Objets_A_Activer[0].name)
                    {
                        for (var k = 0; k < Objets_A_Activer.Length; k++)
                        {
                            var objetActiveCible = Objets_A_Activer[k];

                            if (objetActiveCible != null)
                            {
                                //Debug.Log( objetActiveCible.name);

                                objetActiveCible.SetActive(true);
                                if (objetActiveCible.GetComponent<Animator>()) objetActiveCible.GetComponent<Animator>().enabled = true;
                                if (objetActiveCible.GetComponent<AudioSource>()) objetActiveCible.GetComponent<AudioSource>().Play();
                                foreach (Transform enfant in objetActiveCible.transform)
                                {
                                    enfant.gameObject.SetActive(true);
                                }
                                NombreObjetTrouve = -1; // pour empêcher d'activer plusieurs fois cette objet   
                            }
                        }
                            // *********** desactivation des objets ***********
                        for (var m = 0; m < Objets_A_Desactiver.Length; m++)
                        {
                            var objetDesActiverCible = Objets_A_Desactiver[m];

                            if (objetDesActiverCible != null)
                            {
                                objetDesActiverCible.SetActive(false);
                            }
                        }
                    }
                }
            } 
        }
    }

	IEnumerator ActiverCamera()
	{
		yield return new WaitForSeconds(TempsCameraAvantActivation);
		var cameraPersonnage = Camera.main.gameObject;
		Camera.main.gameObject.SetActive(false);
		Camera_A_Activer.SetActive(true);
		yield return new WaitForSeconds(TempsCamera);
		cameraPersonnage.SetActive(true);
		Camera_A_Activer.SetActive(false);
	} 
}
