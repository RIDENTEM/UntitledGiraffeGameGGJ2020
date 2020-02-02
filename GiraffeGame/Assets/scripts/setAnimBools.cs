using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class setAnimBools : MonoBehaviour
{
    public Animator a;
    GameObject win;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        
        
    }
    private void FixedUpdate()
    {
        checkFix();
    }
    void checkFix()
    {
        if (win != null)
        {
            if (win.GetComponent<window>().isFixing)
            {
                
                setTrue("startRepairing");
            }
            else
            {
                setFalse("startRepairing");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "window")
        {
            
            win = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    public void setAllFalse()
    {
        a.SetBool("startRunning",false);
        a.SetBool("startJumping", false);
        a.SetBool("startRepairing", false);
        a.SetBool("startThrowing", false);
    }
    public void setTrue(string s) {
        a.SetBool(s,true);
    }
    public void setFalse(string s)
    {
        a.SetBool(s, false);
    }
    public void setTrigger(string s)
    {
        a.SetTrigger(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
