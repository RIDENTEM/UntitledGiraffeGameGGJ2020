using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private GameObject bezier;
    private BezierCurves curve;
    private Vector3 cachedPosition;
    private Rigidbody2D rb;
    public int spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void throwMe(Vector3 loc){
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(spin());
        bezier = Instantiate((GameObject)Resources.Load("Prefabs/Giraffe Curve", typeof(GameObject)));
       
        curve = bezier.GetComponent<BezierCurves>();
        curve.controlPoints[0].transform.position = gameObject.transform.position;
        curve.controlPoints[2].transform.position = loc;
        float y = Mathf.Max(curve.controlPoints[0].transform.position.y, curve.controlPoints[2].transform.position.y)+5;
        float x = (curve.controlPoints[0].transform.position.x + curve.controlPoints[2].transform.position.x)/2;
        curve.controlPoints[1].transform.position = new Vector3(x,y,0);
        curve.lockedObject = gameObject;
        curve.animateIt(2);
        StartCoroutine(finishThrow());

    }
    IEnumerator spin()
    {
        while (true)
        {
            gameObject.transform.eulerAngles = new Vector3(0,0, gameObject.transform.eulerAngles.z+spinSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator finishThrow()
    {
        while (curve.movingObject)
        {
            yield return null;


        }
        rb.gravityScale = 1;

        rb.AddForce((curve.controlPoints[2].transform.position - curve.controlPoints[1].transform.position) *10*3);
        yield return new WaitForSeconds(3);
        Destroy(bezier);
        Destroy(gameObject);
        
    }
}
