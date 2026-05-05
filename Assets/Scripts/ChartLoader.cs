using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ChartLoader : MonoBehaviour {
    public NoteSpawner noteSpawner; // Reference to NoteSpawner to access noteTravelTime for spawn timing calculations
    // This method takes the JSON chart as a TextAsset and returns the parsed SongData.
    public SongData LoadChart(TextAsset jsonText) {
        if (jsonText == null) {
            Debug.LogError("ChartLoader.LoadChart called with null TextAsset.");
            return null;
        }

        SongData data = null;
        try {
            data = JsonUtility.FromJson<SongData>(jsonText.text);
        } catch (System.Exception ex) {
            Debug.LogError($"Failed to parse chart JSON: {ex.Message}\nContent:\n{jsonText.text}");
            return null;
        }

        if (data == null) {
            Debug.LogError($"JsonUtility returned null while parsing chart. Content:\n{jsonText.text}");
            return null;
        }

        if (data.notes == null) {
            Debug.LogWarning($"Parsed song has no notes. Title: {data.title} Artist: {data.artist}");
        } else {
            Debug.Log($"Loaded chart for song: {data.title} by {data.artist} with {data.notes.Length} notes.");
        }

        if (data.notes.Length > 0) {
            foreach (var note in data.notes) {
                Debug.Log($"Note - Beat: {note.beat}, Lane: {note.lane}");
            }
        } else {
            Debug.LogWarning("data.notes array is empty.");
        }
        return data;
    }
    public float[] GetHitTimings(SongData data) {
        float[] hitTimings = new float[data.notes.Length];
        float bpm = data.bpm;
        float secondsPerBeat = 60f / bpm;

        for (int i = 0; i < data.notes.Length; i++) {
            var note = data.notes[i];
            // When should it be hit? (Exactly on the beat) (= 0 when the music starts)
            hitTimings[i] = (note.beat * secondsPerBeat) + data.offset; // Apply offset to hit timing
        }

        return hitTimings;
    }
    
    public float[] GetSpawnTimings(SongData data) {
        float[] spawnTimings = new float[data.notes.Length];
        float bpm = data.bpm;
        float secondsPerBeat = 60f / bpm;

        for (int i = 0; i < data.notes.Length; i++) {
            var note = data.notes[i];
            Debug.Log($"Calculating spawn timing for Note {i}: Beat {note.beat}");

            // When should it spawn? (Exactly 2 seconds before) (=0 when scene starts)
            spawnTimings[i] = (note.beat * secondsPerBeat) + data.offset - noteSpawner.noteTravelTime; // Apply offset to spawn timing
            Debug.Log($"Note {i}: Beat {note.beat}, Spawn Time: {spawnTimings[i]:F2} seconds");
        }

        return spawnTimings;
    }
    
}
