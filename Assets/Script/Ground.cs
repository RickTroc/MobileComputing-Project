using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ground : MonoBehaviour
{
    Player player;
   public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D bCollider;
    
    private int groundAmount = 0;
    
    bool didGenerateGround = false;


    public Obstacle boxTemplate;
    public Obstacle flyingEnemy;
    public Obstacle powerUpBox;
    public Obstacle speedBox;
    public Boss boss;

    //uso static perchè il valore di diff e lastspawn non dipende dalla singola istanza do ground
    static float diff = 800; //differenza tra player.distance e last spawn, inizializzato ad 800 solo per far spawnare la prima istanza di boss subito dopo i 1500 metri
    static float lastSpawn = 0;//ultimo spawn del boss (in metri). 


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        bCollider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        groundHeight = transform.position.y + (bCollider.size.y / 2);
    }

    private void FixedUpdate()
    {

       
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;


        groundRight = transform.position.x + (bCollider.size.x / 2);

        groundAmount++;

        if (groundRight < 0)
        {
            Destroy(gameObject);
            groundAmount--;
            return;
        }

        if (!didGenerateGround )
        {
            if(groundRight < screenRight && !player.isDead)
            {
                generateGround();
                groundAmount++;
                didGenerateGround = true;
            }
        }

        transform.position = pos;
    }

    void generateGround()
    { 

        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos = new Vector2();

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f*(player.gravity * (t*t)));        
        float maxJumpHeight = h1 + h2 -2;
        float maxY = maxJumpHeight * 0.5f ; //-5, forse è qui che si annidava il bug...
        maxY += groundHeight;
        
       
        float minY = 1;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        // se spawna troppo in alto setta a 2 l'altezza
        if(pos.y > 2.7f)
        
            pos.y = 2.7f;
        


        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);
        

        
        pos.x = actualX + goCollider.size.x /2;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        GroundFall fall = go.GetComponent<GroundFall>();
        if (fall != null)
        {
            Destroy(fall);
            fall = null;
        }

        if (player.distance >= 10 && Random.Range(0, 2) == 0)
        {
            
            fall = go.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(1f, 3f);

        }

/*  spawn degli oggetti
-------------------------------------------------------------------------------------------------------------------------------*/        
        
        // box
        int obstacleNum = Random.Range(0, 2);
        for(int i = 0; i<obstacleNum; i++)
        {
            if(player.distance > 100) { 
            GameObject box = Instantiate(boxTemplate.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);

            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;

            if(fall != null)
                {
                    Obstacle o = box.GetComponent<Obstacle>();
                    fall.obstacles.Add(o);
                }
            }
        }

        //bird
        int obsFlyObjNum = Random.Range(0, 3);
        for(int i = 0; i<obsFlyObjNum; i++)
        {
            if (player.distance > 1000)
            {
                GameObject bird = Instantiate(flyingEnemy.gameObject);
                float y = Random.Range(goGround.groundHeight + 3, goGround.groundHeight + maxJumpHeight - 5);
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);

                Vector2 birdPos = new Vector2(x, y);
                bird.transform.position = birdPos;
                if (fall != null)
                {
                    Obstacle o = bird.GetComponent<Obstacle>();
                    fall.obstacles.Add(o);
                }
            }
        }

        //speedbox
        int speedBoxnum = Random.Range(0, 10);
        for (int i = 0; i < obstacleNum; i++)
        {
            if (player.distance > 700)
            {
                GameObject speedbox = Instantiate(speedBox.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);

                Vector2 boxPos = new Vector2(x, y);
                speedbox.transform.position = boxPos;

                if (fall != null)
                {
                    Obstacle o = speedbox.GetComponent<Obstacle>();
                    fall.obstacles.Add(o);
                }
            }
        }
        
        //power-up box
        if ((Random.Range(0, 10) == 1 && player.distance>500))
        {
            GameObject specialBox = Instantiate(powerUpBox.gameObject);
            float y = goGround.groundHeight; 
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            specialBox.transform.position = boxPos;
            if (fall != null)
            {
                Obstacle o = specialBox.GetComponent<Obstacle>();
                fall.obstacles.Add(o);
            }
        }

        //boss
        if (player.distance>500)
        {
            if (diff >= 800) //il boss spawna ogni 800 metri, la distruzione di boss è nella classe 'boss'
            {
                GameObject bossI = Instantiate(boss.gameObject);
                Vector2 bossPos = new Vector2(61.3f, 10.9f);
                bossI.transform.position = bossPos;
                lastSpawn = player.distance;
                diff = player.distance - lastSpawn;
            }
            else
                diff = player.distance - lastSpawn;
        }
        else // reinizializzo le variabili statiche
        {
            diff = 800;
            lastSpawn = 0;
        }
//----------------------------------------------------------------------------------------------------------------------------------
    }

    
}

