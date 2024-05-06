using UnityEngine;
using System.Collections;

public class ObjectTransporter : MonoBehaviour
{
	public GameObject objectToTransport; // Ссылка на объект, который нужно переместить
	public int sceneToLoad = 0; // Номер сцены, на которую нужно переместить объект

	void Start()
	{
		StartCoroutine(TransportObject());
	}

	private IEnumerator TransportObject()
	{	
		yield return new WaitForSeconds(5f); // Ждем 5 секунд

        objectToTransport.SetActive(true);

        yield return new WaitForSeconds(2f);

        // Перебрасываем объект на нулевую сцену
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
	}
}