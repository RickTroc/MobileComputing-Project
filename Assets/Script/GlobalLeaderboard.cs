using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalLeaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    int leaderboardID = 10727;

    private Transform entry;
    private Transform container;
    private LootLockerLeaderboardMember[] members;


    private void Awake()
    {
        container = transform.Find("Container");
        entry = container.Find("HighscoreEntry");
        entry.gameObject.SetActive(false);
        float templateHeight = 41f;
        LootLockerSDKManager.GetScoreListMain(leaderboardID, 10, 0, response =>
        {
            if (response.success)
            {
                members = response.items;
                for (int i = 0; i < members.Length && i<5 ; i++)
                {
                    Instantiate(entry, container);
                    RectTransform entryRectTransform = entry.GetComponent<RectTransform>();
                    entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                    entry.gameObject.SetActive(true);
                    entry.Find("ScoreText").GetComponent<Text>().text = members[i].score.ToString();
                    if (members[i].player.name != "")
                        entry.Find("PosText").GetComponent<Text>().text = members[i].player.name;
                    else
                        entry.Find("PosText").GetComponent<Text>().text = members[i].player.id.ToString();
                    Debug.Log(PlayerPrefs.GetString("PlayerID"));
                }
            }
            else
            {

            }
        });
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    }

