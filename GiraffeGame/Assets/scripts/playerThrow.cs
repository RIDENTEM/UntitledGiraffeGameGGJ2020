using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerThrow : MonoBehaviour
{

    public bool hasHammer;
    static bool canCatch;
    public GameObject giraffe;
    float catchTimer;
    public GameObject timerEmpty;
    public GameObject timerFull;
    bool touchingHammer;
    public GameObject hammerSymbol;
    public GameObject hammerToCatch;
    private window window;
    private GameManager gm;
    private setAnimBools setAB;
    // Start is called before the first frame update
    void Start()
    {
        setAB = gameObject.GetComponent<setAnimBools>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        catchTimer = 0;
        canCatch = false;
        giraffe = GameObject.FindGameObjectWithTag("giraffe");
        hasHammer = false;
        hideTimers();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GetComponent<playerMovement>().getLock() && !gm.gameOver)
        {
            checkThrow();
            catchHammer();
            checkHammer();
        }
    }
    public bool isCatching()
    {
        return canCatch;
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
    void checkHammer()
    {
        if (hasHammer)
        {
            hammerSymbol.SetActive(true);
        }
        else
        {
            hammerSymbol.SetActive(false);
        }
    }
    void checkThrow()
    {
        if (Input.GetButtonDown("throw") && hasHammer)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            GameObject hammer = Instantiate((GameObject)Resources.Load("prefabs/hammer(player)", typeof(GameObject)), transform.position, Quaternion.identity);
            setAB.setTrigger("startThrowing");
            
            Vector3 loc = giraffe.transform.position;
            hammer.GetComponent<Hammer>().throwMe(loc);
            hasHammer = false;
        }
    }

    void catchHammer()
    {
        if (Input.GetButton("catch") && catchTimer < 1)
        {
            canCatch = true;
            catchTimer += Time.deltaTime;
            float sx = Mathf.Min(1, catchTimer / 1);
            showTimers();
            timerFull.transform.localScale = new Vector3(sx, .2f, 1);
            if (window != null)
            {
                window.unbreakable = true;
            }
        }
        else if (Input.GetButton("catch"))
        {
            canCatch = false;
            hideTimers();
            if (window != null)
            {
                window.unbreakable = true;
            }
        }
        else
        {
            canCatch = false;
            catchTimer = 0;
            hideTimers();
            if (window != null)
            {
                window.unbreakable = true;
            }
        }
        if (touchingHammer && canCatch)
        {
            hasHammer = true;
            gm.score += 175 * gm.stage;
            Destroy(hammerToCatch);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "hammer")
        {
            hammerToCatch = collision.gameObject;
            touchingHammer = true;
        }
        else if (collision.tag == "window")
        {
            window = collision.gameObject.GetComponent<window>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "hammer")
        {
            touchingHammer = false;
        }
        else if (collision.tag == "window")
        {
            window = null;
        }
    }
}
