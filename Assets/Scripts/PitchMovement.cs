using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchMovement : MonoBehaviour
{
    /*
     * This script takes the pitch value recorded from the AudioAnalyser script and translates it into a Y value.
     * This Y value will be visible to player 1 as a marker for their current pitch.
     * It will also be score based on its relation to the target pitch, although that function may be handled in a different script.
     */

    public Vector2 pPos;

    private void Update()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        AudioAnalyzer audioAnalyze = mainCamera.GetComponent<AudioAnalyzer>();

        pPos = new Vector2(transform.position.x, audioAnalyze.pitchValue / 50);
        transform.position = pPos;
    }
}
