using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giraffe : MonoBehaviour
{
    private GameObject[] windows;
    private float timer;
    private float lastThrow;
    public float interval;
    public GameObject spawnPos;
    private Vector3 spawn;
    private bool throwHam;
    public GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        throwHam = false;
        lastThrow = 0;
        timer = 0;
        windows = GameObject.FindGameObjectsWithTag("window");
        if(spawnPos == null)
        {
            spawn = gameObject.transform.position;
        }
        else
        {
            spawn = spawnPos.transform.position;
        }
    }
    public void startThrowing()
    {
        throwHam = true;
    }
    public void stopThrowing()
    {
        throwHam = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (throwHam&&!gm.GetComponent<GameManager>().gameOver)
        {
            throwHammers();
        }
        checkAnimation();
        
    }
    void checkAnimation()
    {
        if (throwHam)
        {
            gameObject.GetComponent<Animator>().SetBool("throw", true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("throw", false);
        }
    }
    public void setWindows(GameObject[] newWindows)
    {
        windows = new GameObject[newWindows.Length];
        windows = newWindows;
    }
    void throwHammers()
    {
        timer += Time.deltaTime;
        if ((Mathf.Round(timer * 10f) / 10f) % interval==0 && ((Mathf.Round(timer * 10f) /10) != lastThrow))
        {
            lastThrow = (Mathf.Round(timer * 10f) / 10f);
            GameObject hammer = Instantiate((GameObject)Resources.Load("prefabs/hammer",typeof(GameObject)),spawn,Quaternion.identity);
            
            int r = Random.Range(0, windows.Length);
            Vector3 loc = windows[r].transform.position;
            hammer.GetComponent<Hammer>().throwMe(loc);
        }
        
                
    }
}
