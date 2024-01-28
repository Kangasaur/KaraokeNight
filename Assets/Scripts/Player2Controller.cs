using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    /*
     * This script takes the input from the mouse scroll wheel, controlled by player 2, and translates it into a pitch-shifting value to send to the AudioAnalyzer script.
     */

    public AudioAnalyzer input;
    public float pitchOffset;

    //Tracks the scroll wheel input and adjusts the pitch
    public void ScrollAdjust()
    {
        if (input.pitchValue != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                pitchOffset = 5f;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                pitchOffset = -5f;
            }
            input.pitchValue += pitchOffset;
        }
    }
}
