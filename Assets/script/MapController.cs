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
    public Dictionary<Tuple<int, int>, GameObject> blockDict;
    public Set<Tuple<int,int>> edgeEmptySet;

    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<BoxCollider>().size.x;
        width = specialBlock.GetComponent<BoxCollider>().size.z;
        blockDict = new Dictionary<Tuple<int, int>, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void initialize()
    {
        for(int xPos = -1; xPos <= 1; xPos++){
            for(int yPos = -1; yPos <= 1; yPos++){
                this.blockDict.Add(new Tuple<int, int>(xPos, yPos), Instantiate(regularBlock, new Vector3(xPos, yPos, 0.0f)));
                
            }
        }
    }
}
