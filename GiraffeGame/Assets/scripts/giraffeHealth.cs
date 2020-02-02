using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giraffeHealth : MonoBehaviour
{

    int health;
    public GameObject ghealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponent<creatureHealth>().currentHealth;
        string s = "sprites/Giraffe_Health_Spritesheet_" + (4-health);

        ghealth.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(s, typeof(Sprite));
    }
}
