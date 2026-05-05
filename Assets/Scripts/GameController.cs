using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameController : MonoBehaviour
{
    public ChartLoader chartLoader;
    public TextAsset chartJson; // Assign the JSON file for the chart in the inspector
    public Conductor conductor; // Reference to the Conductor script
    public NoteSpawner noteSpawner; // Reference to the NoteSpawner script
    public float songStartDelay = 2f; // Delay before the song starts, in seconds
    private float[] currentSpawnTimings; // Spawn timings from ChartLoader
    private float[] currentHitTimings; // Hit timings from ChartLoader
    private SongData currentSongData; // The current song data loaded from the chart
    private float sceneTime = 0f; // Track elapsed time since the scene started
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        
        // Ensure all references are set
        if (chartLoader == null) Debug.LogError("GameController: ChartLoader reference is not set.");
        if (conductor == null) Debug.LogError("GameController: Conductor reference is not set.");
        if (noteSpawner == null) Debug.LogError("GameController: NoteSpawner reference is not set.");


        // Load the chart
        if (chartLoader != null && chartJson != null)
        {
            currentSongData = chartLoader.LoadChart(chartJson);
            if (currentSongData != null)
            {
                currentHitTimings = chartLoader.GetHitTimings(currentSongData);
                currentSpawnTimings = chartLoader.GetSpawnTimings(currentSongData);

            }
            else
            {
                Debug.LogError("Failed to load song data from chart; aborting song start.");
            }
        }
        else
        {
            Debug.LogError("ChartLoader or ChartJson is not set; cannot load chart.");
        }

         // Start the song after a short delay to ensure everything is initialized
        

        
    }
    void Start() {
        sceneTime = 0f;
        // Pause the music at the start; it will be played when the song starts
        if (conductor != null && currentSongData != null)
        {
            conductor.StartSong(currentSongData, songStartDelay);
        }

        if (noteSpawner != null && currentSpawnTimings != null)
        {
            noteSpawner.hitTimings = currentHitTimings;
            noteSpawner.spawnTimings = currentSpawnTimings;

            Debug.Log("Starting note spawner with loaded spawn timings.");
            noteSpawner.StartSpawning(currentSongData);
        }
    }
    void Update() {
        sceneTime += Time.deltaTime;
    }

    public float GetCurrentSceneTime() {
        return sceneTime;
    }
}
