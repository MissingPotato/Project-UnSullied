using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	private void Start()
	{
		playMenu.SetActive(false);
		optionsMenu.SetActive(false);
	}

	[Header("Scene Swapper")]
	[Space]

	public GameObject screenLoading;
	public Slider slider;

	public List<GameObject> screenOthers;

	[Space]
	[Space]
	[Space]

	[Header("Menu Controller")]

	public GameObject playMenu;
	public GameObject optionsMenu;

	// ------------------------------------------------------------------------------------------------------------------------

	public void ChangeMenu(int choice)
	{

		if (choice == 0 )
		{
			playMenu.SetActive(false);
			optionsMenu.SetActive(false);
		}

		else
			if (choice == 1)
		{
			playMenu.SetActive(true);
			optionsMenu.SetActive(false);
		}

		else
			if (choice == 2 )
		{
			playMenu.SetActive(false);
			optionsMenu.SetActive(true);
		}

	}

	// ------------------------------------------------------------------------------------------------------------------------

	public void QuitGame()
	{
		Application.Quit();
	}

	// ------------------------------------------------------------------------------------------------------------------------

	public void LoadScene(string name)
	{
		StartCoroutine(LoadAsynchronously(name));
	}
	
	IEnumerator LoadAsynchronously (string name)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(name);

		foreach (GameObject scene in screenOthers)
		{
			scene.SetActive(false);
		}

		screenLoading.SetActive(true);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);

			slider.value = progress;

			yield return null;
		}

	}



}
