using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
public class window : MonoBehaviour
{
    private Input controller; 
    public bool broken;
    public Sprite brokenWindowSprite;
    public Sprite windowSprite;
    public GameObject timerEmpty;
    public GameObject timerFull;
    public bool atWindow;
    private float hold;
    public float fixTime;
    public GameObject brokenText;
    public bool unbreakable;
    private GameManager gm;
    private setAnimBools setAB;
    public bool isFixing;
    // Start is called before the first frame update
    void Start()
    {
        setAB = GameObject.Find("player").GetComponent<setAnimBools>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        hideTimers();
        broken = false;
    }
    void showTimers()
    {
        timerEmpty.SetActive(true);
        timerFull.SetActive(true);
    }
    void hideTimers()
    {
        timerEmpty.SetActive(false);
        timerFull.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
       
        checkWindowFix();
        checkBroken();
    }
    
    void checkWindowFix()
    {
        if (atWindow && Input.GetButton("Fix")&& broken)
        {
            isFixing = true;
            showTimers();
            hold += Time.deltaTime;
            float sx = Mathf.Min(1, hold / fixTime);
            timerFull.transform.localScale = new Vector3(sx, .2f, 1);

        }
        else
        {
            isFixing = false;
            hideTimers();
            hold = 0;
        }
        if (hold > fixTime)
        {
            isFixing = false;
            setAB.setFalse("startRepairing");
            gm.score += 125*gm.stage;
            broken = false;
        }
    }
    void checkBroken()
    {
        if (broken)
        {
            brokenText.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().sprite = brokenWindowSprite;
        }
        else
        {
            brokenText.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().sprite = windowSprite;
        }
    }
    public void setBroken()
    {
        if (!unbreakable)
        {
            gameObject.GetComponent<AudioSource>().Play();
            broken = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "hammer")
        {
            setBroken();
        }
        else if (collision.tag == "Player")
        {

            atWindow = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            atWindow = false;
            unbreakable = false;
        }
    }
}
