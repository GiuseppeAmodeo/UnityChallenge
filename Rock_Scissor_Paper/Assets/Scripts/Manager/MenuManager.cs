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

	
}
