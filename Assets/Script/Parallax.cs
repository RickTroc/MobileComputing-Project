using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float depth = 1;

    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

        if(pos.x <= -64.2)
            pos.x = 191.08f;


        transform.position = pos;
        }
    }
}
