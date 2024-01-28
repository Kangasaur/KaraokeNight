using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShake : MonoBehaviour
{
    public float positionJitter;
    public float rotationJitter;

    Vector3 startPosition;
    Quaternion startRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posOffset = new Vector3
            (
            Random.Range(-positionJitter, positionJitter),
            Random.Range(-positionJitter, positionJitter),
            0
            );

        Vector3 rotOffset = new Vector3(0, 0, Random.Range(-rotationJitter, rotationJitter));
        transform.position = startPosition + posOffset;
        transform.rotation = Quaternion.Euler(startRotation.eulerAngles + rotOffset);
    }
}
