/* Ce script permet de causer la mort du personnage lorsqu’il touche un objet “mortel”. 
Lorsqu’un objet mortel est touché alors :
    •il devient invisible;
    •s'il possède un son alors le son joue;
    •s'il possède des enfants alors ils s'activent (exemple: des particules);
    •Le personnage ne peut plus bouger et il devient invisible;
    •s’il possède un son ou une animation alors ils jouent
    •s'il y a une caméra alors elle s'active temporairement. 
    •La scène recommence.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Objets_Mortels : MonoBehaviour 
{
   
    public GameObject[] ObjetsMortels;
    public float distanceMortels = 1.2f;
    public bool DevientInvisible;
    public GameObject[] Objets_A_Activer;
    public GameObject[] Objet_A_Desactiver;
    public bool AnimationDeMort;
    public GameObject Camera_A_Activer;
    public int TempsCamera = 5;
    public bool RecommencerAuDebut = true;
    public float DelaiAvantRecommencer = 5;
	



    void Start()
    {
        if(Camera_A_Activer) Camera_A_Activer.tag = "MainCamera";
    }

    void OnCollisionEnter(Collision hit)
    {
        if (ObjetsMortels.Length > 0)
        {
            // Vérifie si ontouvhe les objets mortels
            foreach (GameObject objet in ObjetsMortels)
                if (objet != null)
                {
                    if (hit.collider.name == objet.name || Vector3.Distance(gameObject.transform.position, objet.transform.position) < distanceMortels)
                    {
                        if (DelaiAvantRecommencer <= 0 && RecommencerAuDebut)
                        {
							SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            break;
                        }
                        

                        if (DevientInvisible)
                        {
                            objet.GetComponent<Renderer>().enabled = false;   //l'objetMortel devient invisible
                                                                              //rend les enfants invisibles,

                            Renderer[] renderDesEnfants = objet.GetComponentsInChildren<Renderer>();
                            foreach (Renderer enfant in renderDesEnfants)
                            {
                                enfant.GetComponent<Renderer>().enabled = false;
                            }
							objet.GetComponent<Collider>().enabled = false;
                        }


                        objet.SetActive(true);
                        foreach (Transform enfant in objet.transform)
                        {

                            enfant.gameObject.SetActive(true);
                        }

                        if (objet.GetComponent<AudioSource>()) objet.GetComponent<AudioSource>().Play();
                        if (GetComponent<AudioSource>()) GetComponent<AudioSource>().Play();

                        if (GetComponent<ControleurPersonnage>()) GetComponent<ControleurPersonnage>().enabled = false;


                        if (AnimationDeMort)
                        {

                            GetComponent<Animator>().SetTrigger("mort");
                        }

                        foreach (Transform enfant in gameObject.transform)
                        {

                            enfant.gameObject.SetActive(true);
                        }

                        foreach (var quelObjet in Objets_A_Activer)
                        {
                            if (quelObjet != null)
                            {
                                quelObjet.SetActive(true);
                                foreach (Transform enfant in quelObjet.transform)
                                {

                                    enfant.gameObject.SetActive(true);
                                }
                                if (quelObjet.GetComponent<Animator>()) quelObjet.GetComponent<Animator>().enabled = true;
                                if (quelObjet.GetComponent<AudioSource>()) quelObjet.GetComponent<AudioSource>().Play();
                            }
                        }





                        foreach (var quelObjet in Objet_A_Desactiver)
                        {
                            if (quelObjet != null) quelObjet.SetActive(false);
                        }

                        if (RecommencerAuDebut) StartCoroutine(ActiveDecompte());
                      

                        if (Camera_A_Activer != null)
                        {
                            StartCoroutine(ActiverCamera());
                        }
                        return;
                    }
                }
                else print("il y a des objets vides à trouver dans les paramètres du inspecteur!!!!");
        }
    }



    IEnumerator ActiveDecompte()
    {
        yield return new WaitForSeconds(DelaiAvantRecommencer);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    IEnumerator ActiverCamera()
    {
        var cameraPersonnage = Camera.main.gameObject;
        Camera.main.gameObject.SetActive(false);
        Camera_A_Activer.SetActive(true);
        yield return new WaitForSeconds(TempsCamera);
        cameraPersonnage.SetActive(true);
        Camera_A_Activer.SetActive(false);
    }
}
