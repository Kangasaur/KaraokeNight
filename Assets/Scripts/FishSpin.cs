using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpin : MonoBehaviour
{
    public float rot1;
    public float rot2;
    float rot;

    public GameObject cam;

    void Update()
    {
        rot = Random.Range(rot1, rot2);
        gameObject.transform.Rotate(new Vector3((Time.deltaTime*rot), (Time.deltaTime*rot), (Time.deltaTime*rot)));


        cam.transform.RotateAround(gameObject.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
