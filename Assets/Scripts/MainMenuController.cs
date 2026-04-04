using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Difficulty is static so WaveManager can read it
    // from any scene without losing the value
    public static int difficulty = 1; // 1=Easy 2=Medium 3=Hard

    public void PlayEasy()
    {
        difficulty = 1;
        StartGame();
    }

    public void PlayMedium()
    {
        difficulty = 2;
        StartGame();
    }

    public void PlayHard()
    {
        difficulty = 3;
        StartGame();
    }

    void StartGame()
    {
        // Load scene by name — must match exactly
        SceneManager.LoadScene("SampleScene");
    }
}