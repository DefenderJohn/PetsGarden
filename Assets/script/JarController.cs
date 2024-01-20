using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestination {
    public Vector3 getDestination();
    public void finishPickup(CoinController coin);
}

public class JarController : MonoBehaviour, IDestination
{
    public GameObject pickUpDestination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getDestination() {
        return this.pickUpDestination.transform.position;
    }

    public void finishPickup(CoinController coin)
    {
        MapController.FinishedPickedUp(coin);
    }
}
