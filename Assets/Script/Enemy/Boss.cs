using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Player player;
    public Bullet bullet;
    public float velocity;
    

    private void Awake()
    {
        eliminaboss();
    }
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
       if(gameObject.activeSelf)    
            StartCoroutine(shoot());
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        //seguo il player
        if(!player.isDead)
        {
            if (player.transform.position.y < pos.y-5)
                velocity = -5;
            if (player.transform.position.y > pos.y+5)
                velocity = 5;
            pos.y += velocity * Time.fixedDeltaTime;
            transform.position = pos;
        }
    }

    IEnumerator shoot()
    {
        while(!player.isDead)
        {
            GameObject shoot = Instantiate(bullet.gameObject);
            float y = transform.position.y;
            float x = transform.position.x;
            Vector2 shooterPos = new Vector2(x, y);
            shoot.transform.position = shooterPos;
            yield return new WaitForSeconds(2);
        }
    }



    async Task eliminaboss()          //despawn del boss
    {
        await Task.Delay(7000);
        Destroy(gameObject);
    }
}
