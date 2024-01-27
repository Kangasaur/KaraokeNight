using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNote : MonoBehaviour
{

    [HideInInspector] public float crossingTime;

    public WwiseSyncNoteCreator wwiseSync;
    NoteEvaluator noteEvaluator;

    public enum CueState { DontScore = -1, Early = 0, OK = 1, Good = 2, Perfect = 3, Late = 4, AlreadyScored = 5 }
    public CueState gemCueState;

    public Vector3 destination;

    public int note;
    public float pitch;

    private Vector3 velocity;

    bool set = false;

    //may be needed to make the game feel more fair
    public float crossPositionOffset;

    //debugging crossing sync issues
    private bool _gemCrossed = false;
    [HideInInspector] public bool sustain = false;
    [HideInInspector] public Transform sustainChild = null;

    public string playerInput;

    public GameObject hitParticles;


    public enum SustainType { none, start, end }
    [HideInInspector] public SustainType sustainType;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        noteEvaluator = FindObjectOfType<NoteEvaluator>();

        destination = GameObject.FindGameObjectWithTag("NowCrossing").transform.position;

        //we want to stay in the lane, so the destination will have the same x and y coordinates as the start.
        destination.x -= crossPositionOffset;
        destination.y = transform.position.y;
        destination.z = transform.position.z;

        //velocity = distance/time -- we want to make sure that the cue crosses our destination on beat
        velocity = (destination - transform.position) / (float)(0.001f * (crossingTime - wwiseSync.GetMusicTimeInMS()));

        gemCueState = CueState.DontScore;


    }


    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        float posY = ((note / 10) - 4) + (noteEvaluator.pitchOffset / 100);
        if (sustain)
        {
            float distance = startPosition.y - transform.position.y;
            sustainChild.transform.localPosition = new Vector3(0f, distance / 2, 0.5f);
            sustainChild.transform.localScale = new Vector2(0.35f, distance);
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
            noteEvaluator.currentPitch = pitch;
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
}
