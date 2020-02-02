using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezierCurves : MonoBehaviour
{
    public BezierControlPoints[] controlPoints;
    private Vector3[] cachedControlPoints;
    public bool dofrenet;
    private Vector3[][] allCp;
    public Text text;
    public float theBigT;
    
    public GameObject lockedObject;
    public bool movingObject;
    // Start is called before the first frame update
    void Start()
    {
        cachedControlPoints = new Vector3[controlPoints.Length];
        
        CacheControlPoints();
        //StartCoroutine(animateFrenet());
    }
    private void CacheControlPoints()
    {

        for (int i = 0; i < controlPoints.Length; i++)
        {
            cachedControlPoints[i] = controlPoints[i].cachedPosition;
        }

    }

    void controlThePoints()
    {
        for (int i = 0; i < controlPoints.Length; i++)
        {
            if (cachedControlPoints[i] != controlPoints[i].cachedPosition && i % 3 != 0)
            {
                if (i % 3 == 1)
                {
                    float d = Vector3.Distance(cachedControlPoints[i - 1], cachedControlPoints[i - 2]);

                }
            }
        }
        
    }
    private Vector3 bezierAtT(float t, Vector3[] cp, int n){
        
        if(n == 2){
            Vector3 end = (cp[0]*(1-t))+(cp[1]*(t));
            
            return end;
        }
        

        for(int i=0;i<n-1;i++){
            
            cp[i] = (cp[i]*(1-t))+(cp[i+1]*(t));
        }
        cp[n-1] = Vector3.zero;
        
        return bezierAtT(t,cp,n-1);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void animateIt(int speed)
    {
        StartCoroutine(animateObject(speed));
    }
    IEnumerator animateObject(int speed)
    {
        cachedControlPoints = new Vector3[controlPoints.Length];
        CacheControlPoints();
        movingObject = true;
        WaitForSeconds s = new WaitForSeconds(.05f/speed);
        Vector3 start = cachedControlPoints[0];
        Vector3 end;
        Vector3 rot = lockedObject.transform.eulerAngles;
        Vector3 v;
        for (float i = 0f; i <= 1.01f; i += 0.02f)
        {
            cachedControlPoints = new Vector3[controlPoints.Length];
            CacheControlPoints();
            end = bezierAtT(i, cachedControlPoints, cachedControlPoints.Length);
            v = end - start;
            rot = lockedObject.transform.eulerAngles;
            if (i > .02)
            {
                //Debug.Log("y: " + v.y+ "  /  x: " + v.x);

                lockedObject.transform.position = start;
                //lockedObject.transform.eulerAngles = new Vector3(0,0, Mathf.Rad2Deg * Mathf.Atan(v.y/v.x));
            }
            start = end;
            cachedControlPoints = new Vector3[controlPoints.Length];
            CacheControlPoints();
            yield return s;
        }
        movingObject = false;
        
    }
    public void animateItReverse()
    {
        StartCoroutine(animateObjectReverse());
    }

    IEnumerator animateObjectReverse()
    {
        movingObject = true;
        WaitForSeconds s = new WaitForSeconds(.05f);
        Vector3 start = cachedControlPoints[cachedControlPoints.Length-1];
        Vector3 end;
        Vector3 rot = lockedObject.transform.eulerAngles;
        Vector3 v;
        for (float i = 1.01f; i >= 0f; i -= 0.01f)
        {
            cachedControlPoints = new Vector3[controlPoints.Length];
            CacheControlPoints();
            end = bezierAtT(i, cachedControlPoints, cachedControlPoints.Length);
            v = end - start;
            rot = lockedObject.transform.eulerAngles;
            if (i > .01)
            {
                //Debug.Log("y: " + v.y + "  /  x: " + v.x);

                lockedObject.transform.position = start;
                //lockedObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan(v.y / v.x));
            }
            start = end;
            cachedControlPoints = new Vector3[controlPoints.Length];
            CacheControlPoints();
            yield return s;
        }
        movingObject = false;

    }
    
    void OnDrawGizmos()
    {

        if (controlPoints.Length <= 0) return;

        cachedControlPoints = new Vector3[controlPoints.Length];

        // Cached the control points
        CacheControlPoints();

        if (cachedControlPoints.Length <= 0) return;

        //Gizmos.color = Color.blue;
        //for(int i = 0; i<controlPoints.Length-1;i++){
        //    Gizmos.DrawLine(controlPoints[i].transform.position,controlPoints[i+1].transform.position);
        //}

        // Draw the bspline lines
        Gizmos.color = Color.gray;

        Vector3 start = cachedControlPoints[0];
        Vector3 end = Vector3.zero;
        
        for (float i = 0f; i <=1.05f; i += 0.05f)
        {
              
            end = bezierAtT(i,cachedControlPoints,cachedControlPoints.Length);
                
           
             if(i>.05) Gizmos.DrawLine(start, end);
            start = end;
            cachedControlPoints = new Vector3[controlPoints.Length];
            CacheControlPoints();
        }

        
        Vector3 test = bezierAtT(theBigT,cachedControlPoints,cachedControlPoints.Length);
        //Gizmos.DrawSphere(test, 0.1f);

    }
}
