using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatureHealth : MonoBehaviour
{
    enum creatureType { Player, Giraffe }
    [SerializeField] creatureType thisCreature;
    // Max health to be determined per creature
    [SerializeField] public int maxHealth = 0;
    // Current health of the creature
    [SerializeField] public int currentHealth;
    // Creture rigidbody 2d
    Rigidbody2D rb;
    // Creature sprite renderer
    SpriteRenderer sr;
    // Determines whether the creature can take damage
    [SerializeField] bool invuln = false;
    // Timer to determine when the invuln and repeating invokes should end
    // Doesn't have to be serialized, just used for testing purposes
    [SerializeField] float timer = 0;

    // How long the creature should be invulnerable
    [SerializeField] float timerLength = 3.0f;
    // When the timer is started
    bool startTimer = false;

    playerThrow pt;


    [SerializeField] GameObject playerHeart;
    List<GameObject> playerHearts;
    [SerializeField] GameObject playerHurtHeart;
    List<GameObject> playerHurtHearts;
    [SerializeField] Vector2 playerHeartsPos;
    int heartToSwap;


    void Start()
    {
        heartToSwap = 0;
        playerHearts = new List<GameObject>();
        playerHurtHearts = new List<GameObject>();
        pt = new playerThrow();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerHeartsPos = playerHeart.transform.position;

        currentHealth = maxHealth;
        switch (thisCreature)
        {
            case creatureType.Player:
                float pInc = 0;
                for (int i = 0; i < maxHealth; i++)
                {
                    pInc += 1.4f;
                    float furtherX = playerHeartsPos.x + pInc;
                    float y = playerHeartsPos.y;
                    playerHearts.Add(Instantiate(playerHeart, new Vector2(furtherX, y), Quaternion.identity));
                    playerHurtHearts.Add(Instantiate(playerHurtHeart, new Vector2(furtherX,y),Quaternion.identity));
                }
                break;
            case creatureType.Giraffe:
               // int gInc = 0;
               // for (int i = 0; i < maxHealth; i++)
               // {
               //     gInc -= 1;
               //     float furtherX = giraffeHeartsPos.x - gInc;
               //     float y = giraffeHeartsPos.y;
               //     Instantiate(giraffeHeart, new Vector2(furtherX, y), Quaternion.identity);
               // }
               // Abandoning this for now
                break;
        }

    }

    void invokeDamage()
    {
        InvokeRepeating("spriteInvis", 0.01f, 0.25f);
        InvokeRepeating("spriteVis", 0.0f, 0.3f);
        startTimer = true;
    }

    void endDamage()
    {
        CancelInvoke();
        invuln = false;
        timer = 0;
        startTimer = false;
        spriteVis();
    }

    void checkForDeath()
    {
        if (currentHealth <= 0)
        {
            // Do dead stuff
            switch (thisCreature)
            {
                case creatureType.Player:
                    break;
                case creatureType.Giraffe:

                    break;
            }

        }
    }

    void Update()
    {
        if (startTimer)
        {
            timer += 1.0f * Time.deltaTime;
        }

        if (timer > timerLength)
        {
            endDamage();
        }

        bool f = pt.isCatching();

    }

    void takeDamage()
    {

        if (invuln == false)
        {
            invuln = true;
            currentHealth -= 1;
            
            if (thisCreature == creatureType.Player)
            {
                rb.AddForce(new Vector2(-400.0f, 400.0f));
                playerHearts[heartToSwap].SetActive(false);
                playerHurtHearts[heartToSwap].SetActive(true);
                heartToSwap++;
            }

            invokeDamage();
            checkForDeath();
        }
    }

    void spriteInvis()
    {
        sr.enabled = false;
    }

    void spriteVis()
    {
        sr.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "invisGiraffeWall")
        {
            takeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(thisCreature)
        {
            case creatureType.Player:
                if (collision.gameObject.tag == "hammer" && !pt.isCatching())
                {
                    Debug.Log("Collided with hammer");
                    takeDamage();
                }
                break;

            case creatureType.Giraffe:
                if (collision.gameObject.tag == "hammer(player)")
                {
                    takeDamage();
                }
                break;
        }

        
    }

}


