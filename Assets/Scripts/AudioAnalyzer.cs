using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    /*
     * This script reads the input from the microphone and translates the specific pitch into a Y value.
     * Another script attached to the Player 1 controlled object will adjust itself based on this Y value;
     * Maybe not in this script, but there should always be a set value that Player 1 wants the pitch to be.
     * This value is affected by Player 2 and by native effects in the song.
     * But again, that doesn't need to be here.
     */

    //Init the audioclip recorded by the microphone.
    public AudioClip audioClip;
    public string selectedDevice;

    public Player2Controller controller;

    private const int SAMPLE_SIZE = 1024;

    //Some values that will be taken from the microphone.
    public float rmsValue;
    public float dbValue;
    public float pitchValue;

    //AudioSource information.
    private AudioSource audioSource;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    //Pitch averaging data.
    public int avgRange;
    public List<float> avgValues = new List<float>();
    public float avgSum;

    //Assign the actual microphone used and take the device dictated by the user's computer.
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        selectedDevice = Microphone.devices[0].ToString();
        audioSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);

        audioSource.Play();
        samples = new float[SAMPLE_SIZE];
        spectrum = new float[SAMPLE_SIZE];
        sampleRate = AudioSettings.outputSampleRate;
    }

    //Call the function that will actually take the audio input and put it into a Y value.
    private void Update()
    {
        AnalyzeSound();
        //Debug.Log(pitchValue);
    }

    //Take the input from the microphone and turn it into a readable worldspace value.
    private void AnalyzeSound()
    {
        audioSource.GetOutputData(samples, 0);
        //Getting the RMS.
        int i = 0;
        float sum = 0;
        for (; i < SAMPLE_SIZE; i++)
        {
            sum = samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);

        //Get the descibel value.
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        //Get the sound spectrum.
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        //Find the pitch.
        float maxV = 0;
        var maxN = 0;
        for( i = 0; i < SAMPLE_SIZE; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
            {
                continue;
            }

            maxV = spectrum[i];
            maxN = i;
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < SAMPLE_SIZE - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        pitchValue = freqN * (sampleRate) / SAMPLE_SIZE;

        pitchValue = ((pitchValue * 430f) / 11000f) + 170f;

        pitchValue = Mathf.Round(pitchValue / 5.0f) * 5.0f;

        //Averaging out up to five frames worth of values. Resets after those five frames.
        avgValues.Add(pitchValue);
        for(i = 0; i < avgValues.Count; i++)
        {
            avgSum += avgValues[i];
            pitchValue = avgSum / avgValues.Count;
        }
        avgSum = 0f;

        if(avgValues.Count >= avgRange)
        {
            avgValues.Clear();
        }
        
        controller.ScrollAdjust();
    }
}