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
        if (animal != null)
        {
            animal.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void generateCoin(float initialPos, Vector2Int index)
    {
        this.coin.SetActive(true);
        this.coin.GetComponent<CoinController>().initialize(initialPos, index);
    }

    public void generatePet(Vector2Int index) { 
        this.animal.SetActive(true);
        this.animal.GetComponent<PetController>().index = index;
    }
}
