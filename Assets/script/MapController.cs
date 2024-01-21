using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class MapController : MonoBehaviour
{
    public GameObject specialBlock;
    public GameObject regularBlock;
    public GameObject jar;
    public float length;
    public float width;
    public float height;
    public float delayTime = 20.0f;
    public Dictionary<Vector2Int, GameObject> blockDict;
    public Dictionary<Vector2Int, GameObject> decorationDict;
    public HashSet<Vector2Int> coinSet;
    public HashSet<Vector2Int> edgeEmptySet;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<Renderer>().bounds.size.x;
        height = specialBlock.GetComponent<Renderer>().bounds.size.y;
        width = specialBlock.GetComponent<Renderer>().bounds.size.z;
        this.blockDict = new Dictionary<Vector2Int, GameObject>();
        this.decorationDict = new Dictionary<Vector2Int, GameObject>();
        this.edgeEmptySet = new HashSet<Vector2Int>();
        this.coinSet = new HashSet<Vector2Int>();
        initialize();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnDestroy()
    {
        MapController.OnPickedUp -= handleFinishPickup;
    }

    public void addDecoration(Vector2Int position, GameObject decoration, float additionalLift)
    {
        this.decorationDict.Add(position, Instantiate(decoration, new Vector3(position.x, height / 2 + decoration.GetComponent<Renderer>().bounds.size.y / 2 + additionalLift, position.y), Quaternion.identity));
    }

    private void initialize()
    {
        for (int xPos = -1; xPos <= 1; xPos++)
        {
            for (int yPos = -1; yPos <= 1; yPos++)
            {
                addBlock(new Vector2Int(xPos, yPos), this.regularBlock);
            }
        }
        addDecoration(new Vector2Int(0, 0), this.jar, 0.0f);
        StartCoroutine(this.generateCoins(delayTime));
        MapController.OnPickedUp += handleFinishPickup;
    }

    private void handleFinishPickup(CoinController coin)
    {
        Debug.Log("picked");
        this.coinSet.Remove(coin.index);
        addRandomBlock();
    }

    private void addRandomBlock()
    {
        Vector2Int targetBlockIndex = this.edgeEmptySet.ToArray()[UnityEngine.Random.Range(0, this.edgeEmptySet.Count)];
        this.edgeEmptySet.Remove(targetBlockIndex);
        GameObject targetBlock = UnityEngine.Random.Range(0, 2) == 1 ? this.regularBlock : this.specialBlock;
        addBlock(targetBlockIndex, targetBlock);
    }

    private void addBlock(Vector2Int pos, GameObject block)
    {
        this.edgeEmptySet.Remove(pos);
        this.blockDict.Add(pos, Instantiate(block, new Vector3(pos.x, 0.0f, pos.y), Quaternion.identity));
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

    private IEnumerator generateCoins(float delayTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            generateCoin();
        }
    }

    private void generateCoin()
    {
        if (this.blockDict.Keys.Count - this.coinSet.Count > 0)
        {
            Random random = new Random();
            Vector2Int selectBlockPos = new HashSet<Vector2Int>(this.blockDict.Keys).Except(this.coinSet).ToArray()[random.Next(0, this.blockDict.Keys.Count - this.coinSet.Count)];
            this.blockDict[selectBlockPos].GetComponent<Block>().generateCoin((float)random.NextDouble(), selectBlockPos);
            this.coinSet.Add(selectBlockPos);
        }
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        Debug.Log("Clicked");
        if (context.phase == InputActionPhase.Performed)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 10000))
            {
                GameObject hitObject = hit.rigidbody.gameObject;
                switch (hitObject.tag)
                {
                    case "Coin":
                        hitObject.GetComponent<CoinController>().pickUp(this.decorationDict[new Vector2Int(0, 0)]);
                        break;
                    case "Pet":
                        hitObject.SetActive(false);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public delegate void FinishedPickedUpEventHandler(CoinController coin);
    public static event FinishedPickedUpEventHandler OnPickedUp;
    public static void FinishedPickedUp(CoinController coin)
    {
        OnPickedUp(coin);
    }

    private void checkAndGeneratePet() { 
    
    }
}
