using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float moveSpeed = 15f; // Note movement speed
    public float destroyYPosition = -60f; // Y position at which the note will be destroyed
    public KeyCode noteKey; // The key associated with this note's lane (for input detection)
    public int lane; // lane index for this note
    public float targetTime; // expected hit time in seconds

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < destroyYPosition) // If the note goes below a certain point, destroy it
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(NoteData data, float targetTime = -1f) {
        // Set up the note based on the NoteData (e.g., lane, type)
        this.lane = data.lane;
        if (targetTime >= 0f) this.targetTime = targetTime;
        // This can be expanded to handle different note types and visuals
        // First, set the lane key based on the lane index
        switch (lane) {
            case 0:
                noteKey = KeyCode.D; // Example: Lane 0 corresponds to the D key
                Debug.Log($"Note initialized in lane {lane} with key {noteKey}");
                break;
            case 1:
                noteKey = KeyCode.F; // Example: Lane 1 corresponds to the F key
                Debug.Log($"Note initialized in lane {lane} with key {noteKey}");
                break;
            case 2:
                noteKey = KeyCode.J; // Example: Lane 2 corresponds to the J key
                Debug.Log($"Note initialized in lane {lane} with key {noteKey}");
                break;
            case 3:
                noteKey = KeyCode.K; // Example: Lane 3 corresponds to the K key
                Debug.Log($"Note initialized in lane {lane} with key {noteKey}");
                break;
            default:
                Debug.LogWarning($"NoteData has invalid lane index: {lane}");
                break;
        }
    }
}
