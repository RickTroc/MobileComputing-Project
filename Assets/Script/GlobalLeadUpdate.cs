using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEditor.Compilation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalLeadUpdate : MonoBehaviour
{
    int leaderboardID = 10727;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        Debug.Log("routine started successfully");
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("score uploaded successfully");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error); 
                done = true;
            }
        });
        yield return new WaitWhile(()=>done == false);
    }
}
