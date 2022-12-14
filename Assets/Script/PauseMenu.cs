using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    Player player;
    
    [SerializeField] GameObject pauseMenu;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Resume();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("Touched the UI");
            }
        }

    }


    public void Pause()
    {
        if (!player.isDead)
        {   
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            
        }
     

    }

    public void Resume()
    { 
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
       

    }



}
