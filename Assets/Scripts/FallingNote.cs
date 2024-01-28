using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNote : MonoBehaviour
{

    [HideInInspector] public float crossingTime;

    public WwiseSyncNoteCreator wwiseSync;
    NoteEvaluator noteEvaluator;
    Player2Controller controller;

    public enum CueState { DontScore = -1, Early = 0, OK = 1, Good = 2, Perfect = 3, Late = 4, AlreadyScored = 5 }
    public CueState gemCueState;

    public Vector3 destination;
    public Vector3 offset;

    public int note;
    public float pitch;

    private Vector3 velocity;

    bool set = false;

    public float killzone;

    //may be needed to make the game feel more fair
    public float crossPositionOffset;

    //debugging crossing sync issues
    private bool _gemCrossed = false;
    [HideInInspector] public bool sustain = false;
    [HideInInspector] public Transform sustainChild = null;

    public string playerInput;

    public GameObject hitParticles;

    static Dictionary<int, float> notePitches = new Dictionary<int, float>();

    public enum SustainType { none, start, end }
    [HideInInspector] public SustainType sustainType;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        noteEvaluator = FindObjectOfType<NoteEvaluator>();
        controller = FindObjectOfType<Player2Controller>();

        destination = GameObject.FindGameObjectWithTag("NowCrossing").transform.position + offset;

        //we want to stay in the lane, so the destination will have the same x and y coordinates as the start.
        destination.x -= crossPositionOffset;
        destination.y = transform.position.y;
        destination.z = transform.position.z;

        //velocity = distance/time -- we want to make sure that the cue crosses our destination on beat
        velocity = (destination - transform.position) / (float)(0.001f * (crossingTime - wwiseSync.GetMusicTimeInMS()));

        gemCueState = CueState.DontScore;

        notePitches[97] = 554.37f;
        notePitches[96] = 523.25f;
        notePitches[95] = 493.88f;
        notePitches[94] = 466.16f;
        notePitches[93] = 440f;
        notePitches[92] = 415.3f;
        notePitches[91] = 392f;
        notePitches[90] = 369.99f;
        notePitches[89] = 349.23f;
        notePitches[88] = 329.63f;
        notePitches[87] = 311.13f;
        notePitches[86] = 293.66f;
        notePitches[85] = 277.18f;
        notePitches[84] = 261.63f;
        notePitches[83] = 246.94f;
        notePitches[82] = 233.08f;
        notePitches[81] = 220f;
        notePitches[80] = 207.63f;
        notePitches[79] = 196f;
        notePitches[78] = 185f;
        notePitches[77] = 174.61f;
        notePitches[76] = 164.81f;
        notePitches[75] = 155.56f;
        notePitches[74] = 146.83f;
        notePitches[73] = 138.59f;
        notePitches[72] = 130.81f;
        notePitches[71] = 123.47f;
        notePitches[70] = 116.54f;
        notePitches[69] = 110f;
        notePitches[68] = 103.83f;
    }


    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        float posY = ((note / 6f) - 13.1667f) + (controller.pitchOffset / 100);
        if (sustain)
        {
            float distance = startPosition.x - transform.position.x;
            sustainChild.transform.localPosition = new Vector3(distance * 2, 0, 0.02f);
            sustainChild.transform.localScale = new Vector2(distance * 4, 1f);
        }
        if (sustainChild == null && gemCueState == CueState.AlreadyScored) Destroy(gameObject);
        UpdateWindow();
    }

    public void UpdateWindow()
    {
        //check our cue state against Wwise event

        //we set the evaluator to our note when we cross
        if (wwiseSync.GetMusicTimeInMS() > crossingTime && sustainType == SustainType.start && !set)
        {
            noteEvaluator.targetPitch = pitch;
            set = true;
        }
    }

    //remove late objects in a couple of seconds
    public void RemoveLateGem()
    {
        gemCueState = CueState.AlreadyScored;
        Destroy(gameObject, 2f);
    }

    public void GemScored()
    {
        gemCueState = CueState.AlreadyScored;
        //instantiate particles
        GameObject newParticles = Instantiate(hitParticles, transform.position + (Vector3.zero * 0.3f), transform.rotation);
        Destroy(newParticles, 0.8f);
    }

    //Return the pitch associated with the current note
    public static float GetTargetPitch(int note)
    {
        return notePitches[note];
    }
}
