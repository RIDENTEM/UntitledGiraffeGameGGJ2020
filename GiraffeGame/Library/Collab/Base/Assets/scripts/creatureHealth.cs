using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatureHealth : MonoBehaviour
{
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

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


