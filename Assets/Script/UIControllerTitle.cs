using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIControllerTitle : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject leaderboardMenu;
    [SerializeField] GameObject tutorialMenu;


    // Start is called before the first frame update
    private void Awake()
    {
        leaderboardMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void play() => SceneManager.LoadScene("Game");
/*
    public void tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
*/
    public void tutorial()
    {
        tutorialMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void MainMenu()
    {
        tutorialMenu.SetActive(false);
        leaderboardMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LeaderBoard()
    {
        leaderboardMenu.SetActive(true);
        mainMenu.SetActive(false);  
    }


}
