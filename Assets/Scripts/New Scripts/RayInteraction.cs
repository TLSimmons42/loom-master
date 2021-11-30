using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteraction : XRRayInteractor
{


    public XRRayInteractor rightRay;
    public XRRayInteractor leftRay;

    public Vector3[] rightRayPoints;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int noPoins;
    public void ResetRayObjectPos()
    {
       rightRay.GetLinePoints(ref rightRayPoints);

       //rightRay.TryGetHitInfo()
    }

    

}
