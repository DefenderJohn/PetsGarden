using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapController : MonoBehaviour
{
    public GameObject specialBlock;
    public GameObject regularBlock;
    public GameObject jar;
    public float length;
    public float width;
    public float height;
    public Dictionary<Vector2Int, GameObject> blockDict;
    public Dictionary<Vector2Int, GameObject> decorationDict;
    public HashSet<Vector2Int> edgeEmptySet;

    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<Renderer>().bounds.size.x;
        height = specialBlock.GetComponent<Renderer>().bounds.size.y;
        width = specialBlock.GetComponent<Renderer>().bounds.size.z;
        this.blockDict = new Dictionary<Vector2Int, GameObject>();
        this.decorationDict = new Dictionary<Vector2Int, GameObject>();
        this.edgeEmptySet = new HashSet<Vector2Int>();
        initialize();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void addDecoration(Vector2Int position, GameObject decoration) { 
        this.decorationDict.Add(position, Instantiate(decoration, new Vector3(position.x, height/2 + decoration.GetComponent<Renderer>().bounds.size.y/2, position.y), Quaternion.identity));
    } 

    private void initialize()
    {
        for (int xPos = -1; xPos <= 1; xPos++)
        {
            for (int yPos = -1; yPos <= 1; yPos++)
            {
                addBlock(new Vector2Int(xPos, yPos));
            }
        }
        addDecoration(new Vector2Int(0, 0), this.jar);
    }

    private void addBlock(Vector2Int pos)
    {
        this.edgeEmptySet.Remove(pos);
        this.blockDict.Add(pos, Instantiate(regularBlock, new Vector3(pos.x, 0.0f, pos.y), Quaternion.identity));
        addEdgeToEmptyList(pos);
    }


    private void addEdgeToEmptyList(Vector2Int target)
    {
        List<Vector2Int> surrondings = new List<Vector2Int>() {
            target + new Vector2Int(0, 1),
            target + new Vector2Int(0, -1),
            target + new Vector2Int(-1, 0),
            target + new Vector2Int(1, 0)
        };
        foreach (Vector2Int surronding in surrondings)
        {
            if (!this.blockDict.ContainsKey(surronding))
            {
                this.edgeEmptySet.Add(surronding);
            }
        }
    }
}
