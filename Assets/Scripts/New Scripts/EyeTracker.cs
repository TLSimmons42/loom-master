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

    public ViveSR.anipal.Eye.EyeData_v2 eye_data = new ViveSR.anipal.Eye.EyeData_v2();

    //new stuff
    public ViveSR.anipal.Eye.EyeData_v2 eyeData = new ViveSR.anipal.Eye.EyeData_v2();

    public float pupil_diameter;

    public VerboseData verboseData;



    Vector3 SRCombinedPoint;
    Vector3 gazePoint;
    Ray gazeRay;
    // Start is called before the first frame update
    void Start()
    {
        
        gazerayData = gazerayData.GetComponent<SRanipal_GazeRaySample>();
        rightEyeData = rightEye.right;
        leftEyeData = leftEye.left;

        

        //ViveSR.anipal.Eye.SRanipal_Eye_API.GetEyeData(ref eye_data);

        //ViveSR.anipal.Eye.SRanipal_Eye_v2.GetVerboseData(out vive.VerboseData verbose_data);

        //pupil_diameter_L = verbose_data.left.pupil_diameter_mm;    // read pupil diameter of left eye.

    }
    RaycastHit hit;
    // Update is called once per frame
    void Update()
    {
        //SRanipal_Eye_API(ref data);
        currentRightEyePupilSize = rightEyeData.pupil_diameter_mm;
        currentLeftEyePupilSize = leftEyeData.pupil_diameter_mm;
        SRanipal_Eye.GetVerboseData(out verboseData);
        pupil_diameter = verboseData.left.pupil_diameter_mm;


        //Debug.Log("Left eye size: " + pupil_diameter);



        SRCombinedPoint = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
        //Debug.Log(gazerayData.GazeDirectionCombined);
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
