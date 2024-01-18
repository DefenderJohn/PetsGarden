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
    public Dictionary<Vector2Int, GameObject> blockDict;
    public HashSet<Vector2Int> edgeEmptySet;

    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<BoxCollider>().size.x;
        width = specialBlock.GetComponent<BoxCollider>().size.z;
        this.blockDict = new Dictionary<Vector2Int, GameObject>();
        this.edgeEmptySet = new HashSet<Vector2Int>();
        initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addBlock(Vector2Int pos)
    {
        this.edgeEmptySet.Remove(pos);
        this.blockDict.Add(pos, Instantiate(regularBlock, new Vector3(pos.x, 0.0f, pos.y), Quaternion.identity));
        addEdgeToEmptyList(pos);
    }

    public void initialize()
    {
        for (int xPos = -1; xPos <= 1; xPos++)
        {
            for (int yPos = -1; yPos <= 1; yPos++)
            {
                addBlock(new Vector2Int(xPos, yPos));
            }
        }
    }

    public void addEdgeToEmptyList(Vector2Int target)
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
