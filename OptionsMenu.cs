using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public enum Difficulty{ Easy, Medium, High }

    private static Difficulty currentDifficulty = Difficulty.Easy;

    public static Difficulty CurrentDifficulty{ get => currentDifficulty; set => currentDifficulty = value; }

    public void QualityChange(int index) => QualitySettings.SetQualityLevel(index);

    public void DifficultyChange(int index) => currentDifficulty = (Difficulty)index;

    public void StartGame() => SceneManager.LoadScene("GameScene");
}
