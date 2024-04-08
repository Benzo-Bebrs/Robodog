using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        // Загрузить сцену "SampleScene"
        SceneManager.LoadScene("SampleScene");
    }
}

public class QuitButton : MonoBehaviour
{
    public void OnQuitButtonClicked()
    {
        // Закрыть приложение
        Application.Quit();
    }
}
