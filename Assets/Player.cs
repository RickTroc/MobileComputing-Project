using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public float gravity;
    public Vector2 velocity;
    public float maxAcceleration = 10;
    public float maxXvelocity = 100;
    public float acceleration = 10;
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

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHight);

        if (isGrounded || groundDistance<=jumpThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0.0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (isDead)
        {
            return;
        }

        if(pos.y < -20)
        {
            isDead = true;
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

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if(hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();  
                if(ground != null)
                {
                    if(pos.y >= ground.groundHeight) { 
                        groundHight = ground.groundHeight;
                        pos.y = groundHight;
                        velocity.y = 0f;
                        isGrounded = true;
                        }
                }

            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);



            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime);
            if(wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    if(pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }

        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {

            float velocityRatio = velocity.x / maxXvelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if(velocity.x >= maxXvelocity)
            {
                velocity.x = maxXvelocity;
            }


            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }


        Vector2 obsOrigin = new Vector2(pos.x-1, pos.y);
        RaycastHit2D obsHitX = Physics2D.Raycast(obsOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime);
        if(obsHitX.collider != null)
        {
            Obstacle obstacle = obsHitX.collider.GetComponent<Obstacle>();
            if(obstacle != null)
            {
                hitObstacle(obstacle);
            }
            //Debug.DrawRay(obsOrigin, new Vector2(1, 0), Color.magenta);
        }
        


        RaycastHit2D obsHitY = Physics2D.Raycast(obsOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime);
        if (obsHitY.collider != null)
        {
            Obstacle obstacle = obsHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }


        transform.position = pos;
    }

    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        velocity.x = 0.7f;
    }

}
