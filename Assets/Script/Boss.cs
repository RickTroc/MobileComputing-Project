using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Player player;

    GameObject boss;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        boss.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 pos = transform.position;


        
        if (player.distance == 100)
        {
            boss.SetActive(false);
        }
        if (player.distance == 800)
            boss.SetActive(false);

        transform.position = pos;
    

    }


    void shoot()
    {

    }
}
