using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float depth = 1;
    public float screenPos;
    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        screenPos = Camera.main.transform.position.x;
    }
   

    // Update is called once per frame
    void Update()
    {
        if(!player.isDead)
        {

        
        float realVelocity = player.velocity.x / depth;
            if(PauseMenu.isPaused || player.isDead)
                realVelocity = 0;
            else
                realVelocity = player.velocity.x / depth;

        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

            Debug.Log("centro schermo: " + screenPos + "\ndespawn: " + screenPos + "\nspawn: " + screenPos*3);
        if(pos.x <= -31)
            pos.x = 92.5f;

            /*
            if(pos.x <= -64.2)
                pos.x = 191.08f;*/
            transform.position = pos;
        }
    }
}
