using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIControllerTitle : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject leaderboardMenu;
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] GameObject globalLeaderboardMenu;
    [SerializeField] GameObject LoginMenu;
    [SerializeField] GameObject SignUpMenu;

    // Start is called before the first frame update
    private void Start()
    {
        mainMenu.SetActive(true);  
    }
    private void Awake()
    {
        globalLeaderboardMenu.SetActive(false);
        leaderboardMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        LoginMenu.SetActive(false);
        SignUpMenu.SetActive(false);
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
        globalLeaderboardMenu.SetActive(false); 
        SignUpMenu.SetActive(false);
        LoginMenu.SetActive(false);    
        mainMenu.SetActive(true);
    }

    public void LeaderBoard()
    {
        leaderboardMenu.SetActive(true);
        mainMenu.SetActive(false);
        globalLeaderboardMenu.SetActive(false);
    }

    public void GlobalLeaderboard()
    {
        globalLeaderboardMenu.SetActive(true);
        leaderboardMenu.SetActive(false);
    }
    public void SignUp()
    {
        SignUpMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Login()
    {
        LoginMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
}
