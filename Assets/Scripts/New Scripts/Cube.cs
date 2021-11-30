using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cube : XRGrabInteractable
{
    public GameManager gameManager;
    private Rigidbody rb;

    public Vector3 playWallTargetPos;

    public string currentZone;

    public string playWallZone = "PlayWall";
    public string BuildWallZone = "BuildWall";
    public bool isHeld = false;
    public string NoZone = "No Zone";

    private float playZoneFallSpeed = 2f;

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

    public void PlayerGrab()
    {
        if(currentZone == playWallZone)
        {
            currentZone = NoZone;
            isHeld = true;

        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        PlayerGrab();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "cube despawner")
        {
            Destroy(this.gameObject);
            Debug.Log("cube destroyed");
        }
    }


}
