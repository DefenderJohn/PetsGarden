using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject coin;
    // Start is called before the first frame update
    void Start()
    {
        coin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateCoin(float initialPos) {
        this.coin.GetComponent<CoinController>().randomInitialPosition = initialPos;
        this.coin.SetActive(true);
    }
}
