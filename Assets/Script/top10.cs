using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class top10 : MonoBehaviour
{
    private Transform entry;
    private Transform container;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        container = transform.Find("Container");
        entry = container.Find("HighscoreEntry");
        
        entry.gameObject.SetActive(false);
        float templateHeight = 41f;
        for(int i = 0; i < 10; i++)
        {
            Instantiate(entry,container);
            RectTransform entryRectTransform=entry.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entry.gameObject.SetActive(true);
            int rank = i + 1;
            string rankString = rank.ToString();
            entry.Find("PosText").GetComponent<Text>().text = rankString;

            entry.Find("ScoreText").GetComponent<Text>().text = PlayerPrefs.GetInt("pos"+(i+1).ToString()).ToString();

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
