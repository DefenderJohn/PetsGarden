using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
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
    public HashSet<Vector2Int> occupySet;
    public HashSet<Vector2Int> edgeEmptySet;
    public Camera mainCamera;
    public TMP_Text catchedCountText;

    // Start is called before the first frame update
    void Start()
    {
        length = specialBlock.GetComponent<Renderer>().bounds.size.x;
        height = specialBlock.GetComponent<Renderer>().bounds.size.y;
        width = specialBlock.GetComponent<Renderer>().bounds.size.z;
        this.blockDict = new Dictionary<Vector2Int, GameObject>();
        this.decorationDict = new Dictionary<Vector2Int, GameObject>();
        this.edgeEmptySet = new HashSet<Vector2Int>();
        this.occupySet = new HashSet<Vector2Int>();
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
        checkAndGeneratePet();
    }

    private void handleFinishPickup(CoinController coin)
    {
        Debug.Log("picked");
        this.occupySet.Remove(coin.index);
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
        if (this.blockDict.Keys.Count - this.occupySet.Count > 0)
        {
            Random random = new Random();
            Vector2Int selectBlockPos = new HashSet<Vector2Int>(this.blockDict.Keys).Except(this.occupySet).ToArray()[random.Next(0, this.blockDict.Keys.Count - this.occupySet.Count)];
            this.blockDict[selectBlockPos].GetComponent<Block>().generateCoin((float)random.NextDouble(), selectBlockPos);
            this.occupySet.Add(selectBlockPos);
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
                        this.occupySet.Remove(hitObject.GetComponent<PetController>().index);
                        this.catchedCountText.text = (int.Parse(this.catchedCountText.text) + 1).ToString();
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

    private void checkAndGeneratePet()
    {
        HashSet<Vector2Int> availablePetSet = new HashSet<Vector2Int>();
        foreach (Vector2Int index in this.blockDict.Keys)
        {
            if (this.blockDict[index].tag == "SpecialBlock")
            {
                List<Vector2Int> surrondings = new List<Vector2Int>() {
                    index + new Vector2Int(0, 1),
                    index + new Vector2Int(0, -1),
                    index + new Vector2Int(-1, 0),
                    index + new Vector2Int(1, 0)
                };
                foreach (Vector2Int surronding in surrondings)
                {
                    if (this.blockDict.ContainsKey(surronding) && this.blockDict[surronding].tag == "SpecialBlock")
                    {
                        if (!this.occupySet.Contains(index))
                        {
                            availablePetSet.Add(index);
                        }
                        if (!this.occupySet.Contains(index))
                        {
                            availablePetSet.Add(surronding);
                        }
                    }
                }
            }
        }
        if (availablePetSet.Count == 0)
        {
            StartCoroutine(generatePets(5.0f, Vector2Int.zero, false));
            return;
        }
        Vector2Int selected = availablePetSet.ToArray()[UnityEngine.Random.Range(0, availablePetSet.Count)];
        occupySet.Add(selected);
        StartCoroutine(generatePets(5.0f, selected));
    }

    private IEnumerator generatePets(float delayTime, Vector2Int pos, bool isGenerating = true)
    {
        yield return new WaitForSeconds(delayTime);
        if (isGenerating)
        {
            this.blockDict[pos].GetComponent<Block>().generatePet(pos); 
        }
        checkAndGeneratePet();
    }
}
