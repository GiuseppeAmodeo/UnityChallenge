using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	//Quit the game
	public void QuitButton()
	{
		Application.Quit();
	}

	//Start the game
	public void StartGameButton()
	{
		SceneManager.LoadScene(1);
	}

	public void ReloadLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitLevel()
	{
		SceneManager.LoadScene(0);
	}

}
