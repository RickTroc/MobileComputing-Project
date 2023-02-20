using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        int prob = 10; //probabitlita' di spawn dei powerup
    public Obstacle speedBox;
    public Boss boss;

    //uso static perchè il valore di diff e lastspawn non dipende dalla singola istanza do ground
    static float diff = 800; //differenza tra player.distance e last spawn, inizializzato ad 800 solo per far spawnare la prima istanza di boss subito dopo i 1500 metri
    static float lastSpawn = 0;//ultimo spawn del boss (in metri). 

    public bool isTibetano;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        bCollider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;

        isTibetano = false;
       
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

        if (groundRight < -2)
        {
            Destroy(gameObject);
            groundAmount--;
            return;
        }

        if (!didGenerateGround )
        {
            if(groundRight < screenRight && !player.isDead)
            {
                if (player.esistePowerUp("bridge"))
                {
                    PonteTibetano();
                    Debug.Log("PONTE TUBETANO ATTIVO");
                    groundAmount++;
                    didGenerateGround = true;

                }
                else
                { 
                    generateGround();
                    groundAmount++;
                    didGenerateGround = true; 
                }
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
        float maxX = totalTime * player.velocity.x - 2f; //vediamo se funziona
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 2;
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

        if (player.distance >= 750 && Random.Range(0, 5) == 3)
        {
            
            fall = go.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(1f, 5f);

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
            if (player.distance > 666)
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
        int speedBoxnum = Random.Range(0, 2);
        for (int i = 0; i < speedBoxnum; i++)
        {
            if (player.distance > 450)
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
        if (player.distance > 555)
        {
            
            if ((Random.Range(0, prob) == 3))
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
                if (player.distance > 2000) prob = 5;

            } 
        }

        //boss
        if (player.distance>1000)
        {
            if (diff >= 1000) //il boss spawna ogni 1000 metri, la distruzione di boss è nella classe 'boss'
            {
                GameObject bossI = Instantiate(boss.gameObject);
                Vector2 bossPos = new Vector2(screenRight, 10.9f); // ho cambiato da 61.3f a questo perchè 
                                                                        // cosi' e' compatibile con tutti gli schermi
                bossI.transform.position = bossPos;
                lastSpawn = player.distance;
                diff = player.distance - lastSpawn;
            }
            else
                diff = player.distance - lastSpawn;
        }
        else // reinizializzo le variabili statiche
        {
            diff = 1000;
            lastSpawn = 0;
        }
//----------------------------------------------------------------------------------------------------------------------------------
    }

    //genera ground costante per il tempo del powerUp TELEPORT (Successivamente da cambiare)
    void PonteTibetano()
    {

        GameObject go = Instantiate(gameObject);
        isTibetano = true;
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos = new Vector2();
        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2 - 2;
        float maxY = maxJumpHeight * 0.5f; //-5, forse è qui che si annidava il bug...
        maxY += groundHeight;



        float minY = 1;
        float actualY = minY;

        pos.y = actualY - goCollider.size.y / 2;
     

        float minX = screenRight;
        float actualX = minX;

        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        GroundFall fall = go.GetComponent<GroundFall>();
        if (fall != null)
        {
            Destroy(fall);
            fall = null;
        }

        if (player.distance >= 10 && Random.Range(0, 2) == 123)
        {

            fall = go.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(1f, 3f);

        }


        /*  spawn degli oggetti
        -------------------------------------------------------------------------------------------------------------------------------*/

        // box
        int obstacleNum = Random.Range(0, 2);
        for (int i = 0; i < obstacleNum; i++)
        {
            if (player.distance > 100)
            {
                GameObject box = Instantiate(boxTemplate.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);

                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;

              
            }
        }

        //bird
        int obsFlyObjNum = Random.Range(0, 3);
        for (int i = 0; i < obsFlyObjNum; i++)
        {
            if (player.distance > 750)
            {
                GameObject bird = Instantiate(flyingEnemy.gameObject);
                float y = Random.Range(goGround.groundHeight + 3, goGround.groundHeight + maxJumpHeight - 5);
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);

                Vector2 birdPos = new Vector2(x, y);
                bird.transform.position = birdPos;

            }
        }

        //speedbox
        int speedBoxnum = Random.Range(0, 2);
        for (int i = 0; i < obstacleNum; i++)
        {
            if (player.distance > 500)
            {
                GameObject speedbox = Instantiate(speedBox.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float x = Random.Range(left, right);

                Vector2 boxPos = new Vector2(x, y);
                speedbox.transform.position = boxPos;

            }
        }

        //power-up box
        if ((Random.Range(0, 20) == 7 && player.distance > 1000))
        {
            GameObject specialBox = Instantiate(powerUpBox.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            specialBox.transform.position = boxPos;
          
        }

        //boss
        if (player.distance > 1000)
        {
            if (diff >= 1000) //il boss spawna ogni 1000 metri, la distruzione di boss è nella classe 'boss'
            {
                GameObject bossI = Instantiate(boss.gameObject);
                Vector2 bossPos = new Vector2(screenRight, 10.9f); // ho cambiato da 61.3f a questo perchè 
                                                                   // cosi' e' compatibile con tutti gli schermi
                bossI.transform.position = bossPos;
                lastSpawn = player.distance;
                diff = player.distance - lastSpawn;
            }
            else
                diff = player.distance - lastSpawn;
        }
        else // reinizializzo le variabili statiche
        {
            diff = 1000;
            lastSpawn = 0;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
    }


}

