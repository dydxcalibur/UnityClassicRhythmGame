using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Conductor : MonoBehaviour {
    public static Conductor Instance; // Singleton for easy access
    public SongData currentSong;
    public AudioSource audioSource;
    public TextMeshProUGUI songTimerUI; // UI element to display the song title
    
    public float songPosInBeats; // Current position of the song in beats
    public float songPosInSeconds; // Current position of the song in seconds
    private float dspTimeStart; // The time when the song started, in DSP time
    private int min;
    private int sec;

    void Awake() {
        Instance = this;
        
        if (audioSource != null) {
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
        
    }

    public void StartSong(SongData data, float startDelay) {
        currentSong = data;
        double scheduledTime = AudioSettings.dspTime + startDelay;
        Debug.Log($"Scheduling song '{data.title}' to start at DSP time {scheduledTime:F2} (current DSP time: {AudioSettings.dspTime:F2})");
        audioSource.PlayScheduled(scheduledTime);
        dspTimeStart = (float)scheduledTime;
    }

    public void StopSong() {
        audioSource.Stop();
        audioSource.Pause();
        currentSong = null;
    }

    void Update() {
        if (currentSong == null) return;
        // Compute playback position from DSP time so timing stays accurate even when
        // AudioSource.playOnAwake is false or when scheduled with PlayScheduled.
        float seconds = 0f;
        if (dspTimeStart > 0f) {
            seconds = (float)(AudioSettings.dspTime - dspTimeStart) - currentSong.offset;
        }
        songPosInSeconds = seconds;
        songPosInBeats = seconds * (currentSong.bpm / 60f);
        //Debug.Log($"Song Position in Beats: {songPosInBeats}");
        min = Mathf.FloorToInt(songPosInSeconds / 60f);
        sec = Mathf.FloorToInt(songPosInSeconds % 60f);
        if (songTimerUI != null) {
            songTimerUI.text = $"{min:00}:{sec:00}";
        }
    }
    public float getCurrentSongTime() {
        // for note spawning
        return (float)(AudioSettings.dspTime - dspTimeStart); // Apply offset to get the time relative to the music's beat
    }
}