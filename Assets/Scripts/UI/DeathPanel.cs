using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; // Ссылка на панель экрана смерти
    public Player player; // Ссылка на скрипт Player

    private void Start()
    {
        // Скрыть панель экрана смерти при запуске сцены
        deathPanel.SetActive(false);
    }

    private void Update()
    {
        // Проверить здоровье игрока
        if (player.healthPoint <= 0)
        {
            // Отобразить панель экрана смерти и отключить персонаж
            deathPanel.SetActive(true);
            player.Disable();

            // Запустить перезагрузку уровня через 3 секунды
            Invoke(nameof(RestartLevel), 3f);
        }
    }

    private void RestartLevel()
    {
        // Получить индекс текущей загруженной сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Загрузить текущую сцену снова
        SceneManager.LoadScene(currentSceneIndex);
    }
}