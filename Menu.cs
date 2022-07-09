using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void SinglePlayer(string optionsSceneName) => SceneManager.LoadScene(optionsSceneName);

    public void QuitGame(){
        #if UNITY_EDITOR
        Debug.Log("Application.Exit() ignores in editor.");
        #elif UNITY_ANDROID
        Application.Quit();
        #endif
    }
}
