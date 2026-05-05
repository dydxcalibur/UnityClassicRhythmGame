using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneColorer : MonoBehaviour
{
    private SpriteRenderer laneSpriteRenderer;
    public Sprite defaultLaneSprite;
    public Sprite highlightedLaneSprite;
    public KeyCode keyToPress;
    float highlightDuration = 0f; // Duration to keep the lane highlighted
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laneSpriteRenderer = GetComponent<SpriteRenderer>();
        laneSpriteRenderer.sprite = defaultLaneSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && highlightDuration <= 0.5f) // Only start highlighting if not already highlighted
        {
            laneSpriteRenderer.sprite = highlightedLaneSprite;
            highlightDuration += Time.deltaTime; // Start counting the duration
        }
        else if (Input.GetKeyUp(keyToPress) || highlightDuration > 0.5f)
        {
            laneSpriteRenderer.sprite = defaultLaneSprite;
            highlightDuration = 0f; // Reset the duration when the highlight is removed
        }
    }
}
