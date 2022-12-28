using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{


    public float gravity;
    public Vector2 velocity;
    public float maxAcceleration = 7.5f;
    public float maxXvelocity = 100;
    public float acceleration = 7.5f;
    public float distance = 0;

    public float jumpVelocity = 20;
    public float groundHight = 10;
    public Boolean isGrounded = false;

    public bool isHoldingJump= false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;


    public float jumpThreshold = 1;

    public bool isDead = false;

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;

    GroundFall fall;

    public string[] powerUp = new String[3];
    public bool doubleJumped=false;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHight);

        if ((isGrounded || groundDistance<=jumpThreshold) && !isDead )
        {
            if (PauseMenu.isPaused == false && Input.GetKeyDown(KeyCode.Mouse0))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;

                if(fall != null)
                {
                    fall.player = null;
                    fall = null;
                }
                
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isHoldingJump = false;
        }

        // codice per il doppio-salto
        if (!isGrounded && !isHoldingJump)
        {
             if (esistePowerUp("jump") && !doubleJumped && PauseMenu.isPaused == false && Input.GetKeyDown(KeyCode.Mouse0))
            {
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0.0f;
                doubleJumped = true;
            }
        }
        // codice per il teletrasporto
        if (esistePowerUp("teleport"))
        {
            if (Input.GetKeyDown(KeyCode.A))
                teleport();
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (isDead)
        {
            Time.timeScale = 0f;
            return;
        }

        if(pos.y < -20)
        {
            isDead = true;
            Destroy(gameObject);    
        }

        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                    isHoldingJump = false;
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;

            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y); //trigger del personaggio per la collisione
            Vector2 rayDirection = Vector2.up; //vettore di posizione
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            //il raycast prende come parametri il trigger, il vettore di posizione, la distanza dal terreno e la maskera del layer
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if(hit2D.collider != null)
            {
                //prendiamo la componente ground...
                Ground ground = hit2D.collider.GetComponent<Ground>();  
                if(ground != null)
                {
                    //se il personaggio si trova al di sopra del terreno può atterrare
                    if(pos.y >= ground.groundHeight) { 
                        groundHight = ground.groundHeight;
                        pos.y = groundHight; //l'altitudine viene reinizializzata al terreno
                        velocity.y = 0;//la velocità di caduta viene ripristinata
                        isGrounded = true;//è atterrato
                        doubleJumped = false; //resetta l'eventuale doppio salto
                        }

                    fall = ground.GetComponent<GroundFall>();
                    if(fall != null)
                    {
                        fall.player = this;
                    }

                }
                
                
            }

            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            /*FUNZIONE PER SBATTERE SUI MURI*/
            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if(wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    //Se il player si trova al di sotto del terreno calpestabile...
                    if(pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;//...si ferma e muore
                    }
                }
            }
            
            
           
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)//il player è a terra e continua a giocare.
        {
            float velocityRatio = velocity.x / maxXvelocity; //rapporto tra velocità attuale e massima
            acceleration = (maxAcceleration * (1 - velocityRatio))*0.6f; 
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if(velocity.x >= maxXvelocity)
                velocity.x = maxXvelocity;
            

            //La posizione del trigger è leggermente spostata a sinistra perchè 
            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;

            //Perchè muoio quando caso su una piattaforma che cade?
            //Perchè non avevo scritto questo.
            if (fall != null)
            {
                //Per queste tre linee di codice sono stato scomunicato dalla chiesa cattolica...
                rayDistance = -fall.fallSpeed * Time.fixedDeltaTime;
            }
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }


        Vector2 obsOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obsHitX = Physics2D.Raycast(obsOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if(obsHitX.collider != null)
        {
            hitObstacle(obsHitX);
        }


        RaycastHit2D obsHitY = Physics2D.Raycast(obsOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obsHitY.collider != null)
        {
            hitObstacle(obsHitY);
        }

        transform.position = pos;
    }

    void hitObstacle(RaycastHit2D obsHit)
    {
        // Collisione avvenuta con oggetto di tipo Obstacle
        Obstacle obstacle = obsHit.collider.GetComponent<Obstacle>();
        if(obstacle!=null)
        { 
            //box
            if(obstacle.obsCode == 1)
            {
                Destroy(obstacle.gameObject);
                velocity.x *= 0.6f;

            }
            //bird
            if (obstacle.obsCode == 2)
            {
                Destroy(obstacle.gameObject);
                if (!esistePowerUp("shield"))   // se esiste "shield" player non muore  
                { 
                velocity.x = 0;
                Destroy(gameObject);   
                isDead = true;
                }
                eliminaPowerUp("shield"); //se player è sopravvissuto aveva shield e glielo levo
            }
           

            //speedBox
            if (obstacle.obsCode == 3)
            {
                Destroy(obstacle.gameObject);
                velocity.x += 10f;

            }

            //power-up
            if (obstacle.obsCode == 4)
            {
                Destroy(obstacle.gameObject);
                int cod;
                cod = Random.Range(0, 3);
                if (cod == 0)
                    aggiungiPowerUp("shield");
                if (cod == 1)
                    aggiungiPowerUp("jump");
                if (cod == 2)
                    aggiungiPowerUp("teleport");
            }
        }

        //collisione avvenuta con Bullet
        Bullet bullet = obsHit.collider.GetComponent<Bullet>();
        if(bullet != null)
        {
            Destroy(bullet.gameObject);
            if (!esistePowerUp("shield"))  // se esiste "shield" player non muore 
            { 
            velocity.x = 0;
            Destroy(gameObject);
            isDead = true;
            }
            eliminaPowerUp("shield");   //se player è sopravvissuto aveva shield e glielo levo
        }

    }
    /*funzioni utili per la gestione dei power-up
------------------------------------------------------------------------------------------------------------------*/
    public void activateShield()
    {
        int i = 0;
        bool done = false;
        do
        {
            if (powerUp[i] == "")
            {
                powerUp[i] = "shield";
                done = true;
            }
            i++;
        }
        while (i < powerUp.Length && done == false);
    }
    public void teleport()
    {
        Vector2 position = transform.position;
        RaycastHit2D obsHitY = Physics2D.Raycast(position, Vector2.down);
        if (obsHitY.collider != null)
        {
            Ground ground = obsHitY.collider.gameObject.GetComponent<Ground>();
            if (ground != null)
            {
                velocity.y = 0;
                gravity = 0;
                groundHight = ground.groundHeight;
                position.y = groundHight + 1;
                transform.position = position;
                gravity = -500;
            }
        }
    }
    

    public bool esistePowerUp(string nomePowerUp)
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            if (powerUp[i] == nomePowerUp)
            {
                return true;
            }
        }
        return false;
    }
    void eliminaPowerUp(string nomePowerUp)
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            if (powerUp[i] == nomePowerUp)
            {
                powerUp[i] = "";
            }
        }
    }
    async void aggiungiPowerUp(string nomePowerUp)
    {
        if (!esistePowerUp(nomePowerUp))
        {
            for (int i = 0; i < powerUp.Length; i++)
            {
                if (powerUp[i] == "")
                {
                    powerUp[i] = nomePowerUp;
                    i = powerUp.Length;
                }
            }
            await Task.Delay(10000);
            eliminaPowerUp(nomePowerUp);    
        }
    }
//----------------------------------------------------------------------------------------------------------------------

}


/*
 * 
 * 
 * TODO LIST
 * 
Leaderboard [FATTA]

terreni che cadono [FATTO]

bilancire l'accelerazione [FATTO]

Bosses

Bonus/Malus [Work in progress, lato Anzio]

Shop(per Skin e Musiche?)

Quali e quanti ostacoli.

Diario del Cuore


 */