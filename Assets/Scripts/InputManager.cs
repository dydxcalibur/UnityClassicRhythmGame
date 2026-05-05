using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    public Queue<NoteObject>[] activeNotes = new Queue<NoteObject>[4]; // Assuming 4 lanes
    public float judgeThreshold = 0.15f; // time to enter or quit judge time range
    public ScoreManager scoreManager;
    public Conductor conductor;

    void Awake()
    {
        if (conductor == null) Debug.LogError("InputManager: Conductor reference is not set.");
        if (scoreManager == null) Debug.LogError("InputManager: ScoreManager reference is not set.");

        // Initialize the activeNotes array
        for (int i = 0; i < 4; i++)
        {
            activeNotes[i] = new Queue<NoteObject>();
        }
    }
    void Update()
    {
        float currentSongTime = conductor.getCurrentSongTime();

        for (int i = 0; i < 4; i++)
        {
            if (activeNotes[i].Count > 0)
            {
                NoteObject note = activeNotes[i].Peek();
                float diff = currentSongTime - note.targetTime;

                if (diff > judgeThreshold)
                {
                    MissNote(note);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            TryHitNote(currentSongTime, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            TryHitNote(currentSongTime, 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            TryHitNote(currentSongTime, 2);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            TryHitNote(currentSongTime, 3);
        }
    }

    void TryHitNote(float actualHitTime, int laneIndex = 0) // Default to lane 0 for now; this should be determined based on which key was pressed
    {
        if (activeNotes[laneIndex].Count == 0) return;

        // Check the oldest note in the lane
        NoteObject upcomingNote = activeNotes[laneIndex].Peek();
        float diff = actualHitTime - upcomingNote.targetTime;
        Debug.Log($"TryHitNote: lane={laneIndex} actual={actualHitTime:F2} target={upcomingNote.targetTime:F2} diff={diff:F3}");

        if (Mathf.Abs(diff) <= judgeThreshold)
        {
            HitNote(upcomingNote, diff);
        }
    }

    void HitNote(NoteObject note, float timingError)
    {
        activeNotes[note.lane].Dequeue();
        // Send timingError to ScoreManager for Perfect/Great/Good logic
        scoreManager.JudgeNote(timingError, note);
        Debug.Log($"Hit! Error: {timingError * 1000}ms");
        Destroy(note.gameObject);
    }

    public void MissNote(NoteObject note) {
        activeNotes[note.lane].Dequeue();
        scoreManager.JudgeNote(float.MaxValue, note); // Use a large value to indicate a miss
        Debug.Log("Missed note!");
        Destroy(note.gameObject);
    }

}
