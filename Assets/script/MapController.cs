using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject specialBlock;
    public GameObject regularBlock;
    public float length;
    public float width;
    public List<GameObject> blockList;
    public int initializeCount;
    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<BoxCollider>().size.x;
        width = specialBlock.GetComponent<BoxCollider>().size.z;
        blockList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initialize()
    {
        for (int i = 0; i < initializeCount; i++) { 
            
        }
    }
}
