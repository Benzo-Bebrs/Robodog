using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public GameObject Fade;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        Fade.SetActive(true);
        yield return new WaitForSeconds(3f);
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1 + (index == 3 ? 1 : 0));
    }
}