using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEvaluator : MonoBehaviour
{
    [HideInInspector] public float currentPitch;
    [HideInInspector] public float pitchOffset; //This is the value controlled by P2 (scroll wheel)

    public float pitchWindow = 30; //how lenient the pitch tracking is

    public AudioAnalyzer input;

    private void Update()
    {
        if (currentPitch != 0)
        {
            if (Mathf.Abs(currentPitch + pitchOffset - input.pitchValue) < pitchWindow)
            {
                //increment the score
            }
        }
    }
}
