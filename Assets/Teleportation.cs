using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour {

	public GameObject objetTeleporteDepart;
	public GameObject ObjetTeleporteDestination;

	public bool DevientInvisible;


	public GameObject[] Objets_A_Activer;
	public GameObject[] Objet_A_Desactiver;

	public float DelaiAvantTeleporte = 1f;


	void OnCollisionEnter(Collision hit)
	{
		if (hit.collider.name == objetTeleporteDepart.name )
		{


			Invoke("DelaiTeleportation", DelaiAvantTeleporte);

		}
	}
	// Use this for initialization
	void DelaiTeleportation () {
			
		transform.position = ObjetTeleporteDestination.transform.position;
		if (DevientInvisible) objetTeleporteDepart.SetActive(false);

		ObjetTeleporteDestination.SetActive(true);
			
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

				}
			}
			// *********** desactivation des objets ***********
		for (var l = 0; l < Objet_A_Desactiver.Length; l++)
			{
			var objetDesActiver = Objet_A_Desactiver[l];

				if (objetDesActiver != null)
				{
					objetDesActiver.SetActive(false);
				}
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
