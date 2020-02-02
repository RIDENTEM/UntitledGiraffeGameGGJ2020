using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatureHealth : MonoBehaviour
{
    enum creatureType { Player, Giraffe }
    [SerializeField] creatureType thisCreature;
    // Max health to be determined per creature
    [SerializeField] private int maxHealth = 0;
    // Current health of the creature
    [SerializeField] private int currentHealth;
    // Creture rigidbody 2d
    Rigidbody2D rb;
    // Creature sprite renderer
    SpriteRenderer sr;
    // Determines whether the creature can take damage
    [SerializeField] bool invuln = false;
    // Timer to determine when the invuln and repeating invokes should end
    // Doesn't have to be serialized, just used for testing purposes
    [SerializeField] float timer = 0;
    // When the timer is started
    bool startTimer = false;

    [SerializeField] GameObject playerHeart;
    [SerializeField] GameObject giraffeHeart;

    [SerializeField] Transform playerHeartsPos;
    [SerializeField] Transform giraffeHeartsPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        switch (thisCreature)
        {
            case creatureType.Player:
                int pInc = 0;
                for (int i = 0; i < maxHealth; i++)
                {
                    pInc += 1;
                    float furtherX = playerHeartsPos.transform.position.x + pInc;
                    float y = playerHeartsPos.transform.position.y;
                    Instantiate(playerHeart, new Vector2(furtherX, y), Quaternion.identity);
                }
                break;
            case creatureType.Giraffe:
                int gInc = 0;
                for (int i = 0; i < maxHealth; i++)
                {
                    gInc -= 1;
                    float furtherX = giraffeHeartsPos.position.x - gInc;
                    float y = giraffeHeartsPos.position.y;
                    Instantiate(giraffeHeart, new Vector2(furtherX, y), Quaternion.identity);
                }
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

        if (timer > 3.0f)
        {
            endDamage();
        }

    }

    void takeDamage()
    {

        if (invuln == false)
        {
            invuln = true;
            currentHealth -= 1;
            // Not ready yet
            rb.AddForce(new Vector2(-400.0f, 400.0f));
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "hammer")
        {
            Debug.Log("Collided with hammer");
            takeDamage();
        }
    }

}


