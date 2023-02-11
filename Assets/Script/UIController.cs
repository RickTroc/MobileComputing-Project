using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    Text highscoreText;

    bool updated = false;
    int[] top10 = new int[10];
    GameObject results;
    Text finalDistanceText;
    GameObject shield;
    GameObject teleport;
    GameObject jump;

    int distance;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        shield = GameObject.Find("ShieldText");
        teleport = GameObject.Find("TeleportText");
        jump = GameObject.Find("JumpText");


        results.SetActive(false);

    }


    void Start()
    {
        for (int i = 0; i < top10.Length; i++)
        {
            top10[i] = PlayerPrefs.GetInt("pos" + (i + 1).ToString(), 0);
        }
      
    }


    void Update()
    {
        distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        // se il player ha dei power up mostro il rispettivo template
        if (player.esistePowerUp("jump"))
            jump.SetActive(true);
        if (player.esistePowerUp("teleport"))
            teleport.SetActive(true);
        if (player.esistePowerUp("shield"))
            shield.SetActive(true);

        //se non li ha più lo nascondo
        if (!player.esistePowerUp("shield"))
            shield.SetActive(false);
        if (!player.esistePowerUp("teleport"))
            teleport.SetActive(false);
        if (!player.esistePowerUp("jump"))
            jump.SetActive(false);
    }

    public void quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void endMenu()
    { 
        
        results.SetActive(true);
        finalDistanceText.text = distance + " m";

        //modifico la top 10 se ho fatto un record
        if (distance > PlayerPrefs.GetInt("pos10") && !updated)
        {
            for (int i = 0; i < 10; i++)
            {
                if (top10[i] < distance)
                {
                    for (int j = 9; j > i; j--)
                    {
                        top10[j] = top10[j - 1];
                    }
                    top10[i] = distance;
                    i = 10;
                    updated = true;
                }
            }
            //aggiorno l'highscore

        }

        //aggiorno i prefs
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("pos" + (i + 1).ToString(), top10[i]);
        }
    }
}
