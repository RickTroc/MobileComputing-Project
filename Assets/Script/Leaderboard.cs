using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{

    private Transform entry;
    private Transform container;




    private void Awake()
    {
        container = transform.Find("Panel");
        entry = container.Find("HighscoreEntry");

        entry.gameObject.SetActive(false);
        float templateHeight = 41f;
        for(int i= 0; i<5; i++)
        {
            Instantiate(entry, container);
            RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entry.gameObject.SetActive(true);
            int rank = 1 + i;
            string rankString = rank.ToString();
            entry.Find("PosText").GetComponent<Text>().text = rankString;

            entry.Find("ScoreText").GetComponent<Text>().text = PlayerPrefs.GetInt("pos" + (1+i).ToString()).ToString();

        }

    }


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
