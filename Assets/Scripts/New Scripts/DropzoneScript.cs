using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropzoneScript : MonoBehaviour
{
    int column;
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

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "blue cube" || other.tag == "red cube" || other.tag == "gold cube" || other.tag == "invis cube") && other.GetComponent<Cube>().currentZone != "BuildWall")
        {
            Debug.Log("drop zone script");
            other.transform.rotation = transform.rotation;
            buildWall.GetComponent<BuildWall>().DropBox(other.gameObject, column);
        }
    }
}
