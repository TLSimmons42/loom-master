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

    public float rightEyePupil_diameter;
    public float leftEyePupil_diameter;


    public VerboseData verboseData;


    private Vector3[] LRpoints = new Vector3[2];
    public GameObject endCollider;
    LineRenderer LR;


    Vector3 SRCombinedPoint;
    Vector3 gazePoint;
    Ray gazeRay;
    // Start is called before the first frame update
    void Start()
    {
        
        gazerayData = gazerayData.GetComponent<SRanipal_GazeRaySample>();
        rightEyeData = rightEye.right;
        leftEyeData = leftEye.left;
        LR = gazerayData.GetComponent<LineRenderer>();



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
        leftEyePupil_diameter = verboseData.left.pupil_diameter_mm;
        rightEyePupil_diameter = verboseData.right.pupil_diameter_mm;



        // Debug.Log("Left eye size: " + pupil_diameter);


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
                    Analytics.instance.WriteData("looking at blue cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "red cube")
                {
                    Analytics.instance.WriteData("looking at red cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "invis cube")
                {
                    Analytics.instance.WriteData("looking at red cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                   // Debug.Log("looking at: " + type.ToString());
                }
                if (type == "gold cube")
                {
                    Analytics.instance.WriteData("looking at red cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                    //Debug.Log("looking at: " + type.ToString());
                }
                if (type == "DropZone")
                {
                    Analytics.instance.WriteData("looking at Drop Zone", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                    //Debug.Log("looking at: " + type.ToString());
                }
            }
        }
        else
        {
           // EventManager.instance.CenterFocusLost.Invoke();
            //EventManager.instance.LaserEyeFixationLoss.Invoke();
        }
        LR.GetPositions(LRpoints);
        endCollider.transform.position = LRpoints[LRpoints.Length - 1];
        


    }
}
