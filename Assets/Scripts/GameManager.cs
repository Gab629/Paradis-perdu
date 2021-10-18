using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public string NomDuJoueur;
    static public string quelleScene;
    public Text champsTemp;

    public float tempsDejeu = 20f;
    private float tempsDepart;
    static public float tempsFinal = 0f;
    static public string PointageFinal;


    public GameObject motScore;
    public GameObject motPoints;

    



    void Start()
    {
        quelleScene = SceneManager.GetActiveScene().name;

        if(quelleScene == "Jeu" ) 
        {
            tempsDepart = tempsDejeu;
            GameObject.Find("Nom").GetComponent<Text>().text = NomDuJoueur;
        }

    if(quelleScene == "Jeu 2")
        {
            tempsDepart = tempsDejeu;
            GameObject.Find("Nom").GetComponent<Text>().text = NomDuJoueur;
        }

    if(quelleScene == "Jeu 3")
        {
            tempsDejeu = 40f;
            tempsDepart = tempsDejeu;
            GameObject.Find("Nom").GetComponent<Text>().text = NomDuJoueur;
        }

        if (quelleScene == "Fin")
        {
            GameObject.Find("Nom").GetComponent<Text>().text = NomDuJoueur;
            if (GameObject.Find("Nom").GetComponent<Text>().text == "")
            {
               
                GameObject.Find("Nom").GetComponent<Text>().text = "Joueur";
            }

            GameObject.Find("Temps").GetComponent<Text>().text = Mathf.Abs(tempsFinal).ToString(); 
            GameObject.Find("Score").GetComponent<Text>().text = PointageFinal;

            if (PointageFinal == "")
            {

                motScore.SetActive(false);
                motPoints.SetActive(false);
            }
            
        }
    }

   
    void Update()
    {

        

        quelleScene = SceneManager.GetActiveScene().name;

        if (quelleScene == "Jeu" && tempsDejeu > 0f)
        {
            Decompte();
            champsTemp = GameObject.Find("Temps").GetComponent<Text>();
        }

        if (quelleScene == "Jeu 2" && tempsDejeu > 0f)
        {
            Decompte();
            champsTemp = GameObject.Find("Temps").GetComponent<Text>();
            
        }

        if (quelleScene == "Jeu 3" && tempsDejeu > 0f)
        {
            Decompte();
            champsTemp = GameObject.Find("Temps").GetComponent<Text>();     
        }

    }


    public void DebutPartie() 
    {
        SceneManager.LoadScene("Jeu");

        NomDuJoueur = GameObject.Find("NomDuJoueur").GetComponent<InputField>().text;
    }



     public void Decompte()
    {

        if (tempsDejeu <= 40)
        {
            champsTemp.text = "0:" + Mathf.Floor(tempsDejeu);

        } else if (tempsDejeu <= 9)
        {
            champsTemp.text = "0:0" + Mathf.Floor(tempsDejeu);
        }
        tempsDejeu -= 1 * Time.deltaTime;
        if (tempsDejeu <= 0)
        {
            FinDeJeu();
            GameObject.Find("Hero").GetComponent<Mouvement>().anim_hero.Play("mort");
            GameObject.Find("Hero").GetComponent<Mouvement>().Fin = ("Mort");


        }
    }
    




    public void Rejouer() 
    {
        SceneManager.LoadScene("Accueil");
    }

    public void NiveauSuivant1()
    {
        SceneManager.LoadScene("Jeu");

    }
    
    public void NiveauSuivant2()
    {
        SceneManager.LoadScene("Jeu 2");

    }

    public void NiveauSuivant3()
    {
        SceneManager.LoadScene("Jeu 3");
       
    }

    public void RetourAccueil()
    {
        SceneManager.LoadScene("Accueil");
    }


    public void FinDeJeu()
    {
        

        tempsFinal = Mathf.Round(tempsDepart - tempsDejeu);
        
        tempsDejeu = 0;
        Invoke("Victoire", 3);
        PointageFinal = GameObject.Find("Pointage").GetComponent<Text>().text;
    }


    void Victoire()
    {
        
        SceneManager.LoadScene("Fin");


    }



















}





















































