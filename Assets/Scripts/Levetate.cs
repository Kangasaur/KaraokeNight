using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levetate : MonoBehaviour
{
    public float speed;
    public bool spin;

    void Update()
    {
        Vector3 pos = transform.position;
        float rise = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x, rise * 0.9f, pos.z);


        if (spin)
        {
            float rot;
            rot = Random.Range(5, 20);
            gameObject.transform.Rotate(new Vector3((Time.deltaTime * rot), (Time.deltaTime * rot), (Time.deltaTime * rot)));
        }


    }
}
