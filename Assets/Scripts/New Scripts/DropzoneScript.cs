﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneScript : MonoBehaviour
{
    public int column;
    GameObject buildWall;
    public string direction;
    public Vector2Int index;
    public bool hostDropZone = false;
    public GameObject masterBuildWall;

    // Start is called before the first frame update
    void Start()
    {
        //masterBuildWall = GameObject.FindGameObjectWithTag("master build wall");
        buildWall = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColumn(int c)
    {
        column = c;
    }
    //&& (other.GetComponent<Cube>().currentZone != "BuildWall" || other.GetComponent<XRGrabNetworkInteractable>().currentZone != "BuildWall")
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " THIS IS THE TAG");
        if (GameManager.instance.playerCount == 1)
        {
            if ((other.tag == "blue cube" || other.tag == "red cube" || other.tag == "gold cube" || other.tag == "invis cube") && (other.gameObject.GetComponent<Cube>().currentZone != "BuildWall") )
            {
                Debug.Log("drop zone script");
                MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            }
        }
        else
        if ((other.tag == "blue cube" || other.tag == "red cube" || other.tag == "invis cube") && (other.gameObject.GetComponent<XRGrabNetworkInteractable>().currentZone != "BuildWall") && other.gameObject.GetComponent<XRGrabNetworkInteractable>().canBeDroped)
        {
            
                Debug.Log("drop zone script");
            MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);

        }
        else if ( other.tag == "right gold cube" || other.tag == "left gold cube" && (other.gameObject.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")&& other.gameObject.GetComponent<GoldCubeHalf>().canBeDroped)
        {
            if (GameManager.instance.host)
            {
                Debug.Log("bout to drop a cube");
                MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            }
        }
        else if(other.tag == "gold cube")
        {
            if (GameManager.instance.host)
            {
                MasterBuildWall.instance.dropZoneHit(index, direction, other.gameObject);
            }
        }
        else
        {
            Debug.Log("nothing happens in this collision");
        }
    }
}
