using UnityEngine;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int currentCombo = 0;
    private int maxCombo = 0;
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI comboText;
    public void JudgeNote(float timingError, NoteObject note) {
        // Implement logic to determine if the hit is Perfect, Great, Good, or Miss based on timingError
        // For example:
        if (timingError <= 0.05f) {
            Debug.Log("Perfect!");
            ScoreUpdate(1000);
            ComboUpdate();
        } else if (timingError <= 0.1f) {
            Debug.Log("Great!");
            ScoreUpdate(800);
            ComboUpdate();
        } else if (timingError <= 0.15f) {
            Debug.Log("Good!");
            ScoreUpdate(500);
            ComboUpdate();
        } else {
            Debug.Log("Miss!");
            ComboReset();
        }
    }

    private void ScoreUpdate(int scoreToAdd)
    {
        // This method can be called to update the score display on the UI
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

    private void ComboUpdate() {
        // This method can be called to update the combo count and display on the UI
        currentCombo++;
        if (currentCombo > maxCombo) {
            maxCombo = currentCombo;
        }
        if  (currentCombo >= 2) {
            comboText.text = $"{currentCombo} Combo";
        } else {
            comboText.text = "";
        }
    }

    private void ComboReset() {
        currentCombo = 0;
        comboText.text = "";
    }

    public int GetScore() {
        return score;
    }

    public int GetMaxCombo() {
        return maxCombo;
    }

}
