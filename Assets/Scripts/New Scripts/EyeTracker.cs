using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyeTracker : MonoBehaviour
{
    //public AntiSaccadeGameManager manager;
    //public LaserEyeGameManager laserEyeManager;
    public SRanipal_GazeRaySample gazerayData;
    public ViveSR.anipal.Eye.VerboseData rightEye;
    public ViveSR.anipal.Eye.VerboseData leftEye;
    public ViveSR.anipal.Eye.SingleEyeData rightEyeData;
    public ViveSR.anipal.Eye.SingleEyeData leftEyeData;

    public float currentRightEyePupilSize;
    public float currentLeftEyePupilSize;

    Vector3 SRCombinedPoint;
    Vector3 gazePoint;
    Ray gazeRay;
    // Start is called before the first frame update
    void Start()
    {
        rightEyeData = rightEye.right;
        leftEyeData = leftEye.left;
    }
    RaycastHit hit;
    // Update is called once per frame
    void Update()
    {
        currentRightEyePupilSize = rightEyeData.pupil_diameter_mm;
        currentLeftEyePupilSize = leftEyeData.pupil_diameter_mm;


        SRCombinedPoint = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
        gazePoint = Camera.main.transform.position + gazerayData.GazeDirectionCombined;
        gazeRay = new Ray(SRCombinedPoint, gazePoint);
        if (Physics.Raycast(gazeRay, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<GazeTarget>() != null) //need to have the target class
            {
                string type = hit.transform.GetComponent<GazeTarget>().targetType;
                //EventManager.instance.FixationOnObject.Invoke(type);
                if (type == "blue cube")
                {
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "red cube")
                {
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "invis cube")
                {
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "gold cube")
                {
                    Debug.Log("looking at: " + type.ToString());
                }
            }
        }
        else
        {
           // EventManager.instance.CenterFocusLost.Invoke();
            //EventManager.instance.LaserEyeFixationLoss.Invoke();
        }

    }
}
