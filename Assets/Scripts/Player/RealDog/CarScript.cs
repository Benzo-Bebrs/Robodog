using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrigger : MonoBehaviour
{
	public GameObject targetObject;
	public GameObject VirtualCamera;
	public GameObject Fade;
	public GameObject player;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			targetObject.SetActive(true);
			
			StartCoroutine(DeactivatePlayer());
		}
	}

	private IEnumerator DeactivatePlayer()
	{
		yield return new WaitForSeconds(0.8f);
		VirtualCamera.SetActive(false);
		player.SetActive(false);
		yield return new WaitForSeconds(3f);
		Fade.SetActive(true);
	}
}