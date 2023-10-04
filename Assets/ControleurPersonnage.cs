using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Gestion des déplacements et des rotations du personnage à l'aide des touches en modifiant la velocité 
 * Gestion des animations du prsaonnage
 * @author Vahik Toroussian 
 * @version 2018-03-13
 *@author Modifié par Mathieu Dionne 
* @version 2019-01-01
*/


public class ControleurPersonnage : MonoBehaviour
{
    public float vitesseMarche = 4f;             //Vitesse de déplacement vers l'avant
    public float vitesseCourse  = 8f;                       // vitesse de course du personnage
    public bool tourneAvecLeClavier = true;
    public int vitesseRotation = 2;               //Vitesse de rotation du prsonnage
    public float ForceSaut = 6f;            //Vitesse de saut du prsonnage
    public float ForceGravite = 1;
    private bool auSol;                          //le personnage est au sol ou pas
    private float vDeplacement;                 // le déplacement à appliquer au personnage selon les touches
    
    Rigidbody rigidbodyPerso;                   //Référence au Rigidbody du personnage
    Animator animPerso; 

    //Mettre en mémoire la réference au Rigidbody 
    void Start()
    {
        rigidbodyPerso = GetComponent<Rigidbody>();
        animPerso = GetComponent<Animator>();
        gameObject.tag = "Player";  // la caméra 3e personne, script "Protect from wall" a besoin de cette Tag pour ignorer le joueur comme obstacle 
    }

    //Contrôle les déplacemnts du personnage, sa rotation et ses animaitons 

    void Update()
    {
        if (Camera.main.name == "CameraDistancePivot")
            DeplaceDirectionCamera();
        else
        {
            DeplaceNormale();
        } 

        if (Input.GetKeyDown(KeyCode.Space) && auSol)
        {
            rigidbodyPerso.velocity += new Vector3(0, ForceSaut,0);
        }
        rigidbodyPerso.AddForce(0,-ForceGravite,0);
        AnimerPersonnage();
        
    }

    private Vector3 directionActuelle;
   void DeplaceDirectionCamera()
    { 
        // obtient les valeurs touches horizontales et verticales 
        float hDeplacement = Input.GetAxis("Horizontal");
        float vDeplacement = Input.GetAxis("Vertical");

        //Obtient le transfrom de la caméra active
        Transform camTransform = Camera.main.transform;

        //obtient la vouvelle direction ( (avant/arrièrre)         +  (gouche/droite) )
        Vector3 directionDep = camTransform.forward * vDeplacement + camTransform.right * hDeplacement;


animPerso.SetFloat("directionAnim", 1);  //anime toujours vers l'avant . pas de reculer
        directionDep.y = 0;   //pas de valeur en y , le cas où la camera regarde vers le bas ou vers le haut

        if (directionDep != Vector3.zero)
        {

           directionActuelle = Vector3.Slerp(directionActuelle, directionDep, Time.deltaTime * 10f);

            rigidbodyPerso.velocity = (directionActuelle * vitesseMarche) + new Vector3(0, rigidbodyPerso.velocity.y, 0);
            
            //Oriente dans la direction de la caméra, garde seulement la rotaiton en Y
            transform.forward = directionDep;
            //ou transform.rotation = Quaternion.LookRotation(directionDep);

            //
        }
    }
float directionAvantArrierre;
    void DeplaceNormale()
    {
        // Tourne le personnage à l'aide des touches horizontales (a,d et fléches G et D)
        float rotationPerso = Input.GetAxis("Horizontal");

        if(tourneAvecLeClavier) transform.Rotate(0, rotationPerso*vitesseRotation, 0);

        // Ajout 2019 --> Modification pour ajouter la course
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            vDeplacement = Input.GetAxis("Vertical") * vitesseCourse;
        }
        else
        {
            vDeplacement = Input.GetAxis("Vertical") * vitesseMarche;
        }
        
        

       // Marche avant ou arrière
        if (  Input.GetAxis("Vertical") > 0)  directionAvantArrierre = 1;
        if (  Input.GetAxis("Vertical") < 0)  directionAvantArrierre = -1; 
        
        animPerso.SetFloat("directionAnim", directionAvantArrierre);
        //

        rigidbodyPerso.velocity = (transform.forward * vDeplacement) + new Vector3(0, rigidbodyPerso.velocity.y, 0);
        print(vDeplacement);
    }

void AnimerPersonnage()
    {
        animPerso.SetFloat("vitesseDep", rigidbodyPerso.velocity.magnitude);

        RaycastHit hit;
        auSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out hit, .8f);

        animPerso.SetBool("animeSaut", auSol);

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
} //Physics.SphereCast(transform.position + (Vector3.up * 0.5f), rayonDetectePlancher, Vector3.down, out hitInfo, distanceSol)