using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZoneBouton : MonoBehaviour {

	

    public KeyCode RaccourciClavier;
    public bool RelanceLeJeu = false;
    public GameObject[] Objet_A_Activer;
    public GameObject[] Objet_A_DesActiver;

    Color CouleurDepart;


	void Start () 
    {
       
	}
	
	
	void Update () 
    {

        if (RaccourciClavier != KeyCode.None)
        {
            if (Input.GetKeyUp(RaccourciClavier))
            {
                if (RelanceLeJeu)
                {
                    SceneManager.LoadScene(0);
                    return;
                }
                ActiveObjets();
            }
        }
	}

    void ActiveObjets()
    {
        foreach(GameObject objet in Objet_A_Activer)
        {
            if (objet != null)
            {
                objet.SetActive(true);
            }
        }
       
        foreach(GameObject objet in Objet_A_DesActiver)
        {
            if (objet)
            {
                objet.SetActive(false);
            }
        }

    }

  



    public void cliquer()
    {
       
         if (RelanceLeJeu)
         {
                SceneManager.LoadScene(0);
                return;
            }


            ActiveObjets();
        
    }


}
