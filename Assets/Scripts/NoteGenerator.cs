using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    FallingNote currentNote;
    public GameObject noteStartPrefab;
    public GameObject noteEndPrefab;
    //the "cue" here can be a number of things
    //for now it's just the spawn time offset (in number of beats)
    //right now, this assumes that each beatEvent will have the same cue offset.
    //if you don't want this to be the case, have a seperate beatmap/input evaluator pair for these other events
    //(example - rhythm heaven has varying cue lengths)
    [Header("Cue Offset in Beats")]
    public int cueBeatOffset;

    //Make sure the OkWindow > GoodWindow > PerfectWindow!!!  Also make sure that you don't have successive notes at shorter timespans than your OkWindow
    [Header("Window Sizes in MS")]


    [Header("Assign this - Gems won't work otherwise")]
    public WwiseSyncNoteCreator wwiseSync;

    //We connect these next three methods to relevant events on our Note Highway Wwise Sync

    public void GenerateCueStart(int midiNote)
    {
        float yPosition = (midiNote / 10) - 4;
        GameObject newCue = Instantiate(noteStartPrefab, new Vector3(5.5f, yPosition, -5), Quaternion.identity);

        FallingNote fallingGem = newCue.GetComponent<FallingNote>();

        fallingGem.sustain = true;
        fallingGem.sustainChild = newCue.GetComponentInChildren<Dummy>().gameObject.transform;
        fallingGem.sustainType = FallingNote.SustainType.start;

        currentNote = fallingGem;

        SetGemTimings(fallingGem);
    }

    public void GenerateCueEnd(int midiNote)
    {
        float yPosition = (midiNote / 10) - 4;
        GameObject newCue = Instantiate(noteEndPrefab, new Vector3(5.5f, yPosition, -5), Quaternion.identity);

        FallingNote fallingGem = newCue.GetComponent<FallingNote>();

        fallingGem.sustainType = FallingNote.SustainType.end;

        currentNote.sustain = false;

        SetGemTimings(fallingGem);
    }

    

    void SetGemTimings(FallingNote fallingGem)
    {

        fallingGem.wwiseSync = wwiseSync;

        fallingGem.crossingTime = (float)wwiseSync.SetCrossingTimeInMS(cueBeatOffset);
    }
}
