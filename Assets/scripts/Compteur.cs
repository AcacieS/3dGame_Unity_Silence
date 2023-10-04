using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compteur : MonoBehaviour {

    public int duree = 180;
    public bool decroissant = true;
    public GameObject[] Objet_A_Activer;
    public GameObject[] Objet_A_DesActiver;

    bool SecondePassee = true;
    int TempsFin;
    int TempsDebut;
    int UniteTemps;
/*enum choixVertical{Haut=0,Centre=1,Bas=2}
var AlignementVertical : choixVertical;

enum choixHorizontal{Gauche,Centre,Droite}
var AlignementHorizontal : choixHorizontal;*/




	void Start () 
    {
        if (decroissant)
        {
            TempsDebut = duree;
            TempsFin = 0;
            UniteTemps = -1;   
        }
        else
        {
       
            gameObject.GetComponent<Text>().text="0";
            TempsDebut = 0;
            TempsFin = duree;
            UniteTemps = 1;
        }
        gameObject.GetComponent<Text>().text=TempsDebut.ToString(); 
	}
	
	
	void Update () {
        if (SecondePassee) StartCoroutine(Decompte());
	}

    IEnumerator Decompte()
    {
        SecondePassee = false;
        yield return new  WaitForSeconds(1f);
        TempsDebut = TempsDebut + UniteTemps;
        SecondePassee = true;
        gameObject.GetComponent<Text>().text = TempsDebut.ToString();

        if (TempsDebut == 0 || TempsDebut == duree)
        {
            SecondePassee = false;
            ActiveObjets();
        }
    }

    void ActiveObjets()
    {

        foreach (GameObject objet in Objet_A_Activer)
        {
            if (objet != null)
            {
                objet.SetActive(true);
                if (objet.GetComponent<Animator>()) objet.GetComponent<Animator>().enabled = true;
                if (objet.GetComponent<AudioSource>()) objet.GetComponent<AudioSource>().Play();
                foreach (Transform enfant in objet.transform)
                {
                    enfant.gameObject.SetActive(true);
                }
            }
        }

          
        foreach (GameObject objet in Objet_A_DesActiver)
        {
            if (objet != null)
            {
                objet.SetActive(false);
                foreach (Transform enfant in objet.transform)
                {
                    enfant.gameObject.SetActive(false);
                }
            }
        }
    }
}
