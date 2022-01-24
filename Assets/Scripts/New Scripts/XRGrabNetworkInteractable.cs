﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
{

    private BoxCollider collider;
    private PhotonView photonView;
    Cube cube;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        collider = GetComponent<BoxCollider>();
        cube = GetComponent<Cube>();
    }

    void Update()
    {

    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photonView.RequestOwnership();
        base.OnSelectEntered(interactor);

        Debug.Log("the tag is: " + interactor.transform.parent.gameObject.tag);
        Debug.Log("the NAME is: " + interactor.transform.parent.parent.gameObject.name);

        if (interactor.transform.parent.parent.gameObject.tag == "P1")
        //if (interactor.transform.parent.gameObject.tag == "P1" && (gameObject.tag == "red cube" || gameObject.tag == "invis cube"))
        {
            Debug.Log("teir 1 pass");
            if (gameObject.tag == "red cube" || gameObject.tag == "invis cube")
            {
                Debug.Log("host grabbed cube");
                PlayerGrab();
            }
        }
        if (interactor.transform.parent.parent.gameObject.tag == "P2" )
        {
            if (gameObject.tag == "blue cube" || gameObject.tag == "invis cube")
            {
                Debug.Log("host grabbed cube");
                PlayerGrab();
            }
        }
    }

    public void PlayerGrab()
    {
        cube.currentZone = cube.NoZone;
        collider.isTrigger = true;
        //rb.detectCollisions = false;
        //gameObject.layer = 2;
        //isHeld = true;
        //Debug.Log("GRABBED cube");
    }
}
