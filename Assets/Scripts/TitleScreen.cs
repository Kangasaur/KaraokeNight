using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject cam;
   
    // Update is called once per frame
    void Update()
    {
        cam.transform.Rotate(new Vector3(0, 5 * Time.deltaTime, 0));

        

    }
}
