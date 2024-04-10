using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; 
    public Player player; 

    private void Start()
    {
        deathPanel.SetActive(false);
        player.OnDeath += ShowDeathScreen;
    }

    private void OnDestroy()
    {
        player.OnDeath -= ShowDeathScreen;
    }

    private void ShowDeathScreen()
    {
        deathPanel.SetActive(true);
        player.gameObject.SetActive(false);

        Invoke(nameof(RestartLevel), 3f);
    }

    private void RestartLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}