using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteEvaluator : MonoBehaviour
{
    /*
     * This script takes the pitchValue variable from the AudioAnalyzer and checks it against the desired pitch, represented by targetPitch.
     * This script stores targetPitch.
     * The pitchWindow variable determines how close the player needs to be to improve their score.
     * pitchWindow is also used in an inverse equation to increase the score. The closer you are to targetPitch, the better the score, scaling linearly backwards.
     * score is also stored in this script, and is displayed in the UI object which this script is attached to.
     */

    [HideInInspector] public float targetPitch; //Desired pitch value.

    public float pitchWindow = 40f; //Scoring circle for the pitch tracking.

    public int score; //You know what this is, don't be stupid.
    public TMP_Text scoreDisplay;

    public AudioAnalyzer input;

    private void Update() //Actually update the score depending on how close the pitchValue with pitchOffset is to targetPitch.
    {
        if (targetPitch != 0)
        {
            if (Mathf.Abs(targetPitch - input.pitchValue) < pitchWindow)
            {
                score += 2 * (int) (pitchWindow - (targetPitch - input.pitchValue));
            }
        }
        scoreDisplay.text = score.ToString();
    }
}