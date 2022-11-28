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

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        

        results.SetActive(false);

    }


    void Start()
    {
        for (int i = 0; i < top10.Length; i++)
        {
            top10[i] = PlayerPrefs.GetInt("pos" + (i + 1).ToString(), 0);
        }
        highscoreText.text = "Highscore: \n" + top10[0].ToString();
    }


    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead)
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

    public void quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void retry()
    {
        SceneManager.LoadScene("Game");
    }


}
