using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; 
    private Player player; 

    private void Start()
    {
        player = Player.Instance;
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
        if (player.isCheckPoint)
        {
            SceneManager.LoadScene("LaboratoryCP");
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}