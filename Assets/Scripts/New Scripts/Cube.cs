using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameManager gameManager;
    private Rigidbody rb;

    public Vector3 playWallTargetPos;

    public string currentZone;

    public string playWallZone = "PlayWall";
    public string BuildWallZone = "BuildWall";
    public string PlayerZone = "Player";
    public string NoZone = "No Zone";

    public float playZoneFallSpeed = 25f;

    // Update is called once per frame

    void Start()
    {
        currentZone = playWallZone;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (currentZone == playWallZone)
        {
            MoveCubePlayWall();
        }
    }

    public void MoveCubePlayWall() 
    {
        //if(currentZone == playWallZone && transform.position == gameManager.PlaywallDropPoints[0].transform.position)
        if (currentZone == playWallZone)
        {
            transform.position = Vector3.MoveTowards(transform.position, playWallTargetPos, Time.deltaTime* playZoneFallSpeed);
        }
    }
}
