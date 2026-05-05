using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject tapNotePrefab; // Prefab for the note to spawn
    //public GameObject holdNotePrefab; // Prefab for the hold note to spawn
    //public GameObject dragNotePrefab; // Prefab for the drag note to spawn
    public float[] spawnX = {-1.5f, -0.5f, 0.5f, 1.5f}; // Array of spawn points for the notes
    public float noteSpeed = 15f; // Speed at which the notes move down the screen
    private float spawnY = 8f; // Y position where notes spawn
    private int spawnedIndex = 0;
    private SongData currentSong;
    private float audioTime = 0f; // Track elapsed time since the audio system started
    public Conductor conductor; // Reference to Conductor to get current song time
    public float[] spawnTimings; // Directly from GameController, the time at which each note should be spawned (hit time - noteTravelTime)
    public float[] hitTimings; // /From ChartLoader, the time at which each note should be hit
    public InputManager inputManager; // Reference to InputManager to enqueue active notes
    public float noteTravelTime = 2f; // Time it takes a note to travel from spawn to hit (seconds)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnY = noteSpeed * noteTravelTime - 55; // Spawn notes 2 seconds before they need to be hit 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSong == null || Conductor.Instance == null) return;

        var notes = currentSong.notes;
        // Use Conductor's audio playback position as the authoritative song time
        
        if (conductor != null) {
            audioTime = conductor.getCurrentSongTime();
        } else {
            Debug.LogWarning("NoteSpawner: Conductor reference not set; using local audioTime which may be inaccurate.");
        }

        // Spawn any notes whose spawnTime has arrived (handle multiple per frame)
        if (spawnTimings == null) return;

        // Spawn when hitTime <= playbackTime + noteTravelTime (i.e. hitTime - noteTravelTime <= playbackTime)
        while (spawnedIndex < notes.Length && spawnedIndex < spawnTimings.Length && spawnTimings[spawnedIndex] <= audioTime) {
            Debug.Log($"NoteSpawner: spawning index={spawnedIndex} hitTime={spawnTimings[spawnedIndex]:F2} playbackTime={audioTime:F2} travel={noteTravelTime:F2}");
            Spawn(notes[spawnedIndex]);
            spawnedIndex++;
        }
    }
    public void StartSpawning(SongData song) {
        currentSong = song;
        spawnedIndex = 0;
    }

    void Spawn(NoteData data) {
        GameObject prefab = tapNotePrefab;
        // choose prefab based on note properties if available; default to tap prefab
        GameObject go = Instantiate(prefab, new Vector3(spawnX[data.lane], spawnY, 0), Quaternion.identity);
        
        // Initialize spawned note
        NoteObject noteObject = go.GetComponent<NoteObject>();
        Debug.Log($"NoteSpawner.Spawn: data.lane={data.lane} beat={data.beat}");
        if (noteObject != null) {
            noteObject.moveSpeed = noteSpeed;
            if (hitTimings != null && spawnedIndex < hitTimings.Length) {
                noteObject.Initialize(data, hitTimings[spawnedIndex]);
            }
            if (inputManager != null) {
                if (data.lane >= 0 && data.lane < inputManager.activeNotes.Length) {
                    inputManager.activeNotes[data.lane].Enqueue(noteObject);
                    Debug.Log($"NoteSpawner: Enqueued note in lane {data.lane} into inputManager.activeNotes[{data.lane}]. Queue count: {inputManager.activeNotes[data.lane].Count}");
                } else {
                    Debug.LogWarning($"NoteSpawner: invalid lane {data.lane} for activeNotes enqueuing");
                }
            } else {
                Debug.LogWarning("NoteSpawner: inputManager reference not set; cannot enqueue note into activeNotes.");
            }
        } else {
            Debug.LogError("NoteSpawner: Spawned prefab does not have a NoteObject component.");
        }
    }
}
