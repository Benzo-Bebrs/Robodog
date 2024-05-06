using UnityEngine;
using System.Collections;

public class ObjectTransporter : MonoBehaviour
{
	public GameObject objectToTransport; // ������ �� ������, ������� ����� �����������
	public int sceneToLoad = 0; // ����� �����, �� ������� ����� ����������� ������

	void Start()
	{
		StartCoroutine(TransportObject());
	}

	private IEnumerator TransportObject()
	{	
		yield return new WaitForSeconds(5f); // ���� 5 ������

        objectToTransport.SetActive(true);

        yield return new WaitForSeconds(2f);

        // ������������� ������ �� ������� �����
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
	}
}