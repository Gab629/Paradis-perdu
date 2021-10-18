using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mouvement : MonoBehaviour
{

    private float vitesseDeDeplacement = 3f;
    private float vitesseDeSaut = 7f;
    static public int score = 0;

    //Créer une variable pour l'état de la partie et une autre pour le temps avant le chargement de la scène de fin.
    public string Fin = "EnCours";

    static public string quelleScene;

    public GameObject Diamant;
    public GameObject Diamant1;
    public GameObject Diamant2;
    public float AlphaMax = 1f;
    public float AlphaMin = 0f;
    //créer des variables pour atteindre les diamants du jeu et aller modifier leur opacité


    [SerializeField] private AudioClip audioSaute = null;
    [SerializeField] private AudioClip audioCollectObjets = null;
    [SerializeField] private AudioClip audioMort = null;
    [SerializeField] private AudioClip audioVictoire = null;
    //Créer des variables pour les différents sons
    
    

    public Animator anim_hero;
    private Rigidbody2D rb_hero;
    private Collider2D collider_hero;
    private AudioSource perso_AudioSource;



    void Start()
    {
        anim_hero = GetComponent<Animator>();
        rb_hero = GetComponent<Rigidbody2D>();
        collider_hero = GetComponent<Collider2D>();
        

        perso_AudioSource = GetComponent<AudioSource>();
        quelleScene = SceneManager.GetActiveScene().name;

        if (quelleScene == "Jeu")
        {
            Fin = "En Cours";
            score = 0;
            //quand la scène jeu débute le score et l'état de la partie sont reinitialisé
        }

        if (quelleScene == "Jeu 2")
        {
            Fin = "En Cours";
            score = 0;
            //quand la scène jeu 2 débute le score et l'état de la partie sont reinitialisé
        }

        if (quelleScene == "Jeu 3")
        {
            Fin = "En Cours";
            score = 0;
            //quand la scène jeu 3 débute le score et l'état de la partie sont reinitialisé
        }


    }


    void Update()
    {
        Deplacement();
        Saute();

        if (quelleScene == "Jeu") 
        {
            FlipHeroJeu1();
          
            

            //Le scale du hero est différent dans ce niveau, donc j'ai du modifier le flip hero pour qu'il soit appellé uniquement dans ce niveau
        }
        
        if(quelleScene == "Jeu 2") 
        {
            FlipHeroJeu2();
       
          
            //Le scale du hero est différent dans ce niveau, donc j'ai du modifier le flip hero pour qu'il soit appellé uniquement dans ce niveau
        }

        if (quelleScene == "Jeu 3")
        {
            FlipHeroJeu1();
      
           
                //Le scale du hero est différent dans ce niveau, donc j'ai du modifier le flip hero pour qu'il soit appellé uniquement dans ce niveau
        }

    }






    void Deplacement() 
    {
        float mouvementHorizontal = Input.GetAxis("Horizontal");
        rb_hero.velocity = new Vector2(mouvementHorizontal * vitesseDeDeplacement, rb_hero.velocity.y);

        bool heroBouge = rb_hero.velocity.x != 0;
        anim_hero.SetBool("PeutMarcher", heroBouge);

    }

    void Saute() 
    {
        int quelNiveau = LayerMask.GetMask("Sol");

        if (!collider_hero.IsTouchingLayers(quelNiveau)) 
        {
            return;
        }

        if (Input.GetButtonDown("Jump")) 
        {
            rb_hero.velocity = new Vector2(0, vitesseDeSaut);
            anim_hero.SetTrigger("PeutSauter");
            perso_AudioSource.PlayOneShot(audioSaute);
        }
    }

    void FlipHeroJeu1() 
    {
        bool heroBouge = rb_hero.velocity.x != 0;

        if (heroBouge) 
        {
            transform.localScale = new Vector2(Mathf.Sign(rb_hero.velocity.x) * 0.7758265f, 0.7758265f);
        }
    }

    void FlipHeroJeu2()
    {
        bool heroBouge = rb_hero.velocity.x != 0;

        if (heroBouge)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb_hero.velocity.x) * 0.5908539f, 0.5908539f);
        }
    }

    void OnCollisionEnter2D(Collision2D laCollision)
    {
        if(laCollision.transform.tag == "PourPFMouvement")
        {
            transform.parent = laCollision.transform;
            //Si le hero entre en collision avec la plateforme, il deviendra enfant de la plateforme et ne glissera pas dessus.
        }

        if(laCollision.transform.tag == "Mort") 
        {
            rb_hero.bodyType = RigidbodyType2D.Static;
            //Cette ligne met le RigidBody du héro en mode Static, donc il ne peut plus bouger.
            anim_hero.Play("mort");
            perso_AudioSource.PlayOneShot(audioMort);
           
            Fin = "Mort";
            //On met l'état de la partie à mort et on met le chargement de la scene fin à 3 secondes

            GameObject.Find("GestionnaireDeJeu").GetComponent<GameManager>().FinDeJeu();
        }



    }







    private void OnTriggerEnter2D(Collider2D laCollision)
    {
        if(laCollision.transform.tag == "ObjetAPrendre")
        {
            Destroy(laCollision.gameObject);
            perso_AudioSource.PlayOneShot(audioCollectObjets);
            score++;
            //Lorsque le joueur récupère une pièce son score augmente.
            GameObject.Find("Pointage").GetComponent<Text>().text = score.ToString();

            if (score == 5)
            {
                vitesseDeDeplacement = 0f;
                anim_hero.Play("victoire");
                perso_AudioSource.PlayOneShot(audioVictoire);

                Fin = "Termine";
                GameObject.Find("GestionnaireDeJeu").GetComponent<GameManager>().FinDeJeu();

            }
            //Si le score du joueur est égal à 5, le joueur gagne la partie.

        }

        if(laCollision.transform.tag == "Ennemie" ) 
        {
            vitesseDeDeplacement = 0f;
            vitesseDeSaut = 0f;
            anim_hero.Play("mort");
            perso_AudioSource.PlayOneShot(audioMort);
            Fin = "Mort";
            //On met l'état de la partie à mort et on met le chargement de la scene fin à 3 secondes

        }
        //Si le hero touche un monstre, il meurt.

        if (laCollision.transform.tag == "Buche")
        {
            vitesseDeDeplacement = 0f;
            vitesseDeSaut = 0f;
            anim_hero.Play("mort");
            perso_AudioSource.PlayOneShot(audioMort);
            Fin = "Mort";
            GameObject.Find("GestionnaireDeJeu").GetComponent<GameManager>().FinDeJeu();
        }
        //Si le hero touche une bûche, il meurt.

        if (laCollision.transform.tag == "PieceRouge") 
        {
            Destroy(laCollision.gameObject);          
            vitesseDeDeplacement = 0f;
            anim_hero.Play("victoire");
            perso_AudioSource.PlayOneShot(audioVictoire);
            Fin = "Termine";
            GameObject.Find("GestionnaireDeJeu").GetComponent<GameManager>().FinDeJeu();
        }
        //Si vous touchez la piece rouge, celle-ci se détruit et vous gagnez.

        Diamant = GameObject.Find("Diamant");
        Diamant1 = GameObject.Find("Diamant1");
        Diamant2 = GameObject.Find("Diamant2");

        if (laCollision.transform.tag == "Diamant1")
        {          
            score += 1;
            perso_AudioSource.PlayOneShot(audioCollectObjets);
            GameObject.Find("Pointage").GetComponent<Text>().text = score.ToString();
            Destroy(Diamant1);
            Diamant2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, AlphaMax);
            
        }
        //Quand on touche le diamant 1, celui-ci disparait et le diamant 2 apparrait. Le score augmente de 1.

        if (laCollision.transform.tag == "Socle" && Diamant2.GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, AlphaMax))
        {
            Diamant.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, AlphaMax);
            Diamant2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, AlphaMin);
            vitesseDeDeplacement = 0f;
            anim_hero.Play("victoire");
            perso_AudioSource.PlayOneShot(audioVictoire);
            GameObject.Find("GestionnaireDeJeu").GetComponent<GameManager>().FinDeJeu();
            Fin = "Termine";
        }
        //Quand le hero touche le socle vous gagnez la partie et le diamant apparrait dans le socle et le diamant 2 disparait.

    }




        void OnCollisionExit2D(Collision2D laCollision)
    {
        if(laCollision.transform.tag == "PourPFMouvement")
        {
            transform.parent = null;
        }
    }
    //Lorsque l'on cesse de toucher la plateforme, nous cessons d'être son enfant.



}
