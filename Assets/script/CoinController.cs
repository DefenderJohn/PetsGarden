using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinController : MonoBehaviour
{
    public float waveRange = 1.0f;
    public float originalHeight;
    public float randomInitialPosition;
    public float pickDuration;
    public float timer;
    public float flyHeight;
    public Vector2Int index;
    public Vector3 destination;
    public Vector3 controlPoint;
    public Vector3 currentPosition;
    public Vector3 originalPos;
    public GameObject destinationObject;
    public bool isPicked;
    // Start is called before the first frame update
    void Start()
    {
        originalHeight = this.gameObject.transform.position.y;
        originalPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPicked)
        {
            timer += Time.deltaTime;
            if (timer >= pickDuration)
            {
                this.destinationObject.GetComponent<IDestination>().finishPickup(this);
                this.initialize(Random.value, this.index);
                this.gameObject.transform.position = originalPos;
                this.gameObject.SetActive(false);
            }
            else
            {
                float normalizedTimer = timer / pickDuration;
                Vector3 nextPos = CalculateRoutePoint(normalizedTimer, this.currentPosition, this.controlPoint, this.destination);
                this.transform.position = nextPos;
            }
        }
        else
        {
            this.gameObject.transform.position = new Vector3(
                this.gameObject.transform.position.x,
                originalHeight + Mathf.Sin(Time.time + randomInitialPosition) * waveRange,
                this.gameObject.transform.position.z
            );
        }
    }

    public void initialize(float initialPos, Vector2Int index)
    {
        this.randomInitialPosition = initialPos;
        this.timer = 0.0f;
        this.isPicked = false;
        this.index = index;
    }

    public void pickUp(GameObject destinationObject)
    {
        this.isPicked = true;
        this.destination = destinationObject.transform.position;
        this.destinationObject = destinationObject;
        this.currentPosition = this.transform.position;
        this.controlPoint = new Vector3(){
            x = (currentPosition.x + destination.x) / 2,
            y = this.flyHeight,
            z = (currentPosition.z + destination.z) / 2
        };
    }

    private Vector3 CalculateRoutePoint(float t, Vector3 beginPoint, Vector3 highestPoint, Vector3 destinationPoint)
    {
        return (1 - t) * (1 - t) * beginPoint + 2 * (1 - t) * t * highestPoint + t * t * destinationPoint;
    }
}
