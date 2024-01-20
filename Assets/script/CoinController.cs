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
    // Start is called before the first frame update
    void Start()
    {
        originalHeight = this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(
            this.gameObject.transform.position.x,
            originalHeight + Mathf.Sin(Time.time + randomInitialPosition) * waveRange,
            this.gameObject.transform.position.z
        );
    }
}
