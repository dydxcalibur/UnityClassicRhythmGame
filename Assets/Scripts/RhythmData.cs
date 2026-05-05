using System;
using System.Collections.Generic;
using UnityEngine;

// --- MENU DATA ---
[Serializable]
public class SongManifest {
    public List<SongEntry> allSongs;
}

[Serializable]
public class SongEntry {
    public string id; // Unique identifier for the song
    public string title; // Title of the song
    public string artist; // Artist of the song
    public string[] difficulty; // Array of difficulty levels (e.g., "Easy", "Mid", "Hard")
    public int bpm; // Beats per minute of the song
    public string[] chartFile; // Array of chart file names corresponding to each difficulty
    public string audioFile; // Audio file name for the song
    public string cover; // Cover image file name for the song
    public float offsetMs; // Timing offset in milliseconds to sync the notes with the music
}

// --- GAMEPLAY DATA ---
[Serializable]
public class SongData {
    public string title; // Title of the song
    public string artist; // Artist of the song
    public int bpm; // Beats per minute of the song
    public float offset; // Timing offset in seconds to sync the notes with the music
    public int lanes; // Number of lanes in the song
    public NoteData[] notes; // Array of note data for the song
}

[Serializable]
public class NoteData {
    public float beat; // The beat at which the note should be hit
    public int lane; // The lane in which the note appears (0-based index)
    public string type; // The type of note (e.g., "tap", "hold", "drag")
    public float duration; // Duration of the note (for hold notes)
}
