using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
	public Text lastScore;
	public Text levelText;
	public Text highScoreText;

	private bool toggleMusic = true;
	private bool togglePause = true;
	public Text soundOn;
	public Text soundOff;

	void Start()
	{
		if (lastScore != null)
		{
			lastScore.text = PlayerPrefs.GetInt("lastscore").ToString();
		}

		if (levelText != null)
		{
			levelText.text = "0";
		}
		//PlayerPrefs.SetInt("highscore", 0);
		if (highScoreText != null)
		{
			highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
		}
	}
	public void MenuButton()
	{
		if (FindObjectOfType<Game>() != null)
		{
			FindObjectOfType<Game>().ResumeGame();
		}
		SceneManager.LoadScene("MenuScreen");
	}

	public void PlayGame()
	{
		if (Game.StartingLevel == 0)
		{
			Game.startingAtLevelZero = true;
		}
		else
		{
			Game.startingAtLevelZero = false;
		}

		SceneManager.LoadScene("GameScene");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ChangeValue(float value)
	{
		Game.StartingLevel = (int)value;
		levelText.text = value.ToString();
	}

	public void ToggleMusic()
	{
		if (toggleMusic)
		{
			toggleMusic = false;
			soundOn.GetComponent<Text>().enabled = false;
			soundOff.GetComponent<Text>().enabled = true;
			PlayerPrefs.SetInt("volume", 1);
		}
		else
		{
			toggleMusic = true;
			soundOn.GetComponent<Text>().enabled = true;
			soundOff.GetComponent<Text>().enabled = false;
			PlayerPrefs.SetInt("volume", 0);
		}
	}

	public void TogglePause()
	{		
		if (togglePause)
		{
			togglePause = false;
			FindObjectOfType<Game>().PauseGame();
		}
		else
		{
			togglePause = true;
			FindObjectOfType<Game>().ResumeGame();
		}
	}

	public void LeaderBoard()
	{
		SceneManager.LoadScene("LeaderBoard");
	}

	public void AddScore()
	{
		UIHandler handle = new UIHandler();
		handle.AddScore();
	}
}