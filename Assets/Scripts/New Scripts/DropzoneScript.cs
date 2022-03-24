using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneScript : MonoBehaviour
{
    public int column;
    GameObject buildWall;

    // Start is called before the first frame update
    void Start()
    {
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
                buildWall.GetComponent<BuildWall>().DropBox(other.gameObject, column);
            }
        }
        else
        if ((other.tag == "blue cube" || other.tag == "red cube" || other.tag == "invis cube") && (other.gameObject.GetComponent<XRGrabNetworkInteractable>().currentZone != "BuildWall") && other.gameObject.GetComponent<XRGrabNetworkInteractable>().canBeDroped)
        {
            
                Debug.Log("drop zone script");
                buildWall.GetComponent<BuildWall>().DropBox(other.gameObject, column);
            
        }
        else if ( other.tag == "right gold cube" || other.tag == "left gold cube" && (other.gameObject.GetComponent<GoldCubeHalf>().currentZone != "BuildWall")&& other.gameObject.GetComponent<GoldCubeHalf>().canBeDroped)
        {
            if (GameManager.instance.host)
            {
                Debug.Log("bout to drop a cube");
                buildWall.GetComponent<BuildWall>().DropBox(other.gameObject, column);
            }
        }
        else if(other.tag == "gold cube")
        {
            if (GameManager.instance.host)
            {
                buildWall.GetComponent<BuildWall>().DropBox(other.gameObject, column);
            }
        }
        else
        {
            Debug.Log("nothing happens in this collision");
        }
    }
}
