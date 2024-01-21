using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject coin;
    public GameObject animal;
    // Start is called before the first frame update
    void Start()
    {
        coin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateCoin(float initialPos, Vector2Int index) {
        switch (UnityEngine.Random.Range(0,2))
        {
            case 0:
                this.coin.SetActive(true);
                this.coin.GetComponent<CoinController>().initialize(initialPos, index);
                break;
            case 1:
                this.animal.SetActive(true);
                break;
            default:
                break;
        }
        
    }
}
