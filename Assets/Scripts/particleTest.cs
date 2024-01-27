using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            print("yay");
            gameObject.GetComponent<ParticleSystem>().Emit(1);
        }
        
    }
}
