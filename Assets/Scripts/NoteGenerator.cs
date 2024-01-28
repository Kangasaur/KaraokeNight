using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteGenerator : MonoBehaviour
{
    FallingNote currentNote;
    public GameObject noteStartPrefab;
    public GameObject noteEndPrefab;
    public GameObject lyricPrefab;
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
        if (currentNote != null && currentNote.sustain == true)
        {
            GenerateCueEnd(currentNote.note);
        }

        float yPosition = (midiNote / 6f) - 13.1667f;
        GameObject newCue = Instantiate(noteStartPrefab, new Vector3(5.5f, yPosition, -5), Quaternion.identity);

        FallingNote fallingGem = newCue.GetComponent<FallingNote>();

        fallingGem.sustain = true;
        fallingGem.note = midiNote;
        fallingGem.sustainChild = newCue.GetComponentInChildren<Dummy>().gameObject.transform;
        fallingGem.sustainType = FallingNote.SustainType.start;

        newCue.GetComponent<SpriteRenderer>().color = NoteToHue(midiNote);
        newCue.GetComponentInChildren<Dummy>().GetComponent<SpriteRenderer>().color = NoteToHue(midiNote);

        currentNote = fallingGem;

        

        SetGemTimings(fallingGem);
    }

    public void GenerateCueEnd(int midiNote)
    {
        float yPosition = (midiNote / 6f) - 13.1667f;
        GameObject newCue = Instantiate(noteEndPrefab, new Vector3(5.5f, yPosition, -5), Quaternion.identity);

        FallingNote fallingGem = newCue.GetComponent<FallingNote>();
        fallingGem.note = midiNote;


        fallingGem.sustainType = FallingNote.SustainType.end;

        currentNote.sustain = false;

        newCue.GetComponent<SpriteRenderer>().color = NoteToHue(midiNote);

        //print(currentNote.note);
        //print(newCue.transform.position);
        SetGemTimings(fallingGem);
    }

    public void GenerateLyric(string text)
    {
        float yPosition = (currentNote.note / 6f) - 13.9667f;
        GameObject newCue = Instantiate(lyricPrefab, new Vector3(5.2f, -2.25f, -5), Quaternion.identity);

        newCue.GetComponent<TextMeshPro>().text = text;

        FallingNote fallingGem = newCue.GetComponent<FallingNote>();
        fallingGem.note = currentNote.note;

        fallingGem.sustainType = FallingNote.SustainType.none;
        fallingGem.sustain = false;

        SetGemTimings(fallingGem);
    }

    void SetGemTimings(FallingNote fallingGem)
    {

        fallingGem.wwiseSync = wwiseSync;

        fallingGem.crossingTime = (float)wwiseSync.SetCrossingTimeInMS(cueBeatOffset);
    }

    Color NoteToHue(int note)
    {
        return Color.HSVToRGB((note % 12) / 12f, 0.7f, 1);
    }
}
