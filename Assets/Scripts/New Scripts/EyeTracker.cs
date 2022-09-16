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
    Vector3 LRstartPos;
    Vector3 LRendPos;

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

        Analytics.instance.WriteData3("RightPupil", "", "", leftEyePupil_diameter.ToString(), rightEyePupil_diameter.ToString(), "");

        // Debug.Log("Left eye size: " + pupil_diameter);

        //LR.GetPositions(LRpoints);
        //LRstartPos = LRpoints[LRpoints.Length - 2];
        //LRendPos = LRpoints[LRpoints.Length - 1];
        //Debug.Log("start: " + LRstartPos + "   " + "end: " + LRendPos);
        //gazeRay = new Ray(LRstartPos, LRendPos);
        //Debug.DrawRay(LRstartPos, LRendPos, Color.yellow);


        SRCombinedPoint = Camera.main.transform.position - Camera.main.transform.up * 0.05f;
        gazePoint = gazerayData.GazeDirectionCombined * gazerayData.LengthOfRay;
        gazeRay = new Ray(SRCombinedPoint, gazePoint);
        Ray gazeRay2 = new Ray(SRCombinedPoint, gazePoint);
        int layerMask = ~LayerMask.GetMask("walls");
        //Debug.Log("start: " + SRCombinedPoint + "   " + "end: " + gazerayData.GazeDirectionCombined * gazerayData.LengthOfRay);
        Debug.DrawRay(SRCombinedPoint, gazePoint, Color.yellow);

        if (Physics.Raycast(gazeRay, out hit, 10000, layerMask))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<GazeTarget>() != null) //need to have the target class
            {
                string type = hit.transform.GetComponent<GazeTarget>().targetType;
                Debug.Log(type);
                //EventManager.instance.FixationOnObject.Invoke(type);
                if (type == "blue cube")
                {
                    Debug.Log("hit a blue cube gen");
                    if (GameManager.instance.playerCount == 2)
                    {
                        if (hit.transform.GetComponent<XRGrabNetworkInteractable>().currentZone == hit.transform.GetComponent<XRGrabNetworkInteractable>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at blue cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                    else
                    {
                        Debug.Log("looking at blue cube before play wall");
                        if (hit.transform.GetComponent<Cube>().currentZone == hit.transform.GetComponent<Cube>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at blue cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                }
                if (type == "red cube")
                {
                    if (GameManager.instance.playerCount == 2)
                    {
                        if (hit.transform.GetComponent<XRGrabNetworkInteractable>().currentZone == hit.transform.GetComponent<XRGrabNetworkInteractable>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at red cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                    else
                    {
                        if (hit.transform.GetComponent<Cube>().currentZone == hit.transform.GetComponent<Cube>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at red cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                }
                if (type == "invis cube")
                {
                    if (GameManager.instance.playerCount == 2)
                    {
                        if (hit.transform.GetComponent<XRGrabNetworkInteractable>().currentZone == hit.transform.GetComponent<XRGrabNetworkInteractable>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at invis cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                    else
                    {
                        if (hit.transform.GetComponent<Cube>().currentZone == hit.transform.GetComponent<Cube>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at invis cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                }
                if (type == "gold cube")
                {
                    if (GameManager.instance.playerCount == 2)
                    {
                        if (hit.transform.GetComponent<GoldCubeWhole>().currentZone == hit.transform.GetComponent<GoldCubeWhole>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at gold cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                    else
                    {
                        if (hit.transform.GetComponent<Cube>().currentZone == hit.transform.GetComponent<Cube>().playWallZone)
                        {
                            Analytics.instance.WriteData("looking at gold cube", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                            Debug.Log("looking at: " + type.ToString());
                        }
                    }
                }
                if (type == "DropZoneEye")
                {
                    
                    Analytics.instance.WriteData("looking at Drop Zone", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                    
                }
                //if(type == "View Wall")
                //{
                //    Analytics.instance.WriteData2("looking at View wall", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                //    Debug.Log("looking at: " + type.ToString());
                //}
                //if (type == "Play Wall")
                //{
                //    Analytics.instance.WriteData2("looking at Play wall", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                //    Debug.Log("looking at: " + type.ToString());
                //}
                //if (type == "Build Wall")
                //{
                //    Analytics.instance.WriteData2("looking at Build wall", "", "", hit.transform.position.x.ToString(), hit.transform.position.y.ToString(), hit.transform.position.z.ToString());
                //    Debug.Log("looking at: " + type.ToString());
                //}
            }
        }
        if (Physics.Raycast(gazeRay2, out hit, 10000))
        {
            if (hit.transform.GetComponent<GazeTarget>() != null) //need to have the target class
            {
                string type = hit.transform.GetComponent<GazeTarget>().targetType;
                if (type == "View Wall")
                {
                    Analytics.instance.WriteData2("looking at View wall", "", "", hit.point.x.ToString(), hit.point.y.ToString(), hit.point.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "Play Wall")
                {
                    Analytics.instance.WriteData2("looking at Play wall", "", "", hit.point.x.ToString(), hit.point.y.ToString(), hit.point.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                }
                if (type == "Build Wall")
                {
                    Analytics.instance.WriteData2("looking at Build wall", "", "", hit.point.x.ToString(), hit.point.y.ToString(), hit.point.z.ToString());
                    Debug.Log("looking at: " + type.ToString());
                }
            }
        }
        
        


    }
}
