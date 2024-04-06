using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; // ������ �� ������ ������ ������
    public Player player; // ������ �� ������ Player

    private void Start()
    {
        // ������ ������ ������ ������ ��� ������� �����
        deathPanel.SetActive(false);
    }

    private void Update()
    {
        // ��������� �������� ������
        if (player.healthPoint <= 0)
        {
            // ���������� ������ ������ ������ � ��������� ��������
            deathPanel.SetActive(true);
            player.Disable();

            // ��������� ������������ ������ ����� 3 �������
            Invoke(nameof(RestartLevel), 3f);
        }
    }

    private void RestartLevel()
    {
        // �������� ������ ������� ����������� �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ��������� ������� ����� �����
        SceneManager.LoadScene(currentSceneIndex);
    }
}