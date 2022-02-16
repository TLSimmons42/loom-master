using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCubeHalf : Singleton<GoldCubeHalf>
{

    public string currentZone;
    public string NoZone = "No Zone";
    public string buildWallZone = "BuildWall";

    public GameObject replacedCube;
    public GameObject rightRay;
    public GameObject leftRay;

    public LineRenderer rightLineRenderer;
    public LineRenderer leftLineRenderer;

    private Vector3[] rightRayPoints = new Vector3[2];
    public Vector3[] leftRayPoints;

    void Start()
    {
        currentZone = NoZone;
        rightRay = GameObject.FindGameObjectWithTag("right ray");
        rightLineRenderer = rightRay.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentZone == NoZone)
        {
            PlayerMovesHalf();
        }
    }

    public void PlayerMovesHalf()
    {
        rightLineRenderer.GetPositions(rightRayPoints);
        gameObject.transform.position = rightRayPoints[rightRayPoints.Length - 1];
    }

    public void SetMirrorObj(GameObject obj)
    {
        obj = replacedCube;
    }
}
