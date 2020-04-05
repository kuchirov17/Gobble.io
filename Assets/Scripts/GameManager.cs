using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public PlayerControler pc;
	public MenuManager ui;
	

	public bool started;
	public bool gameOver=false;

	public float gravity = 10f;
	public float time = 120; // Total game time in seconds
	private float tempTime;

	private void Awake()
	{
		if (!gm)
		{
			gm = this;
			tempTime = time;
			DontDestroyOnLoad(this);
		}
	}

	public void Play()
	{
		started = true;
		pc.StartCoroutine(pc.dropFuel());
	}

	public void GameOver()
	{
		gameOver = true;
		ui.gameplay.SetActive(false);
		ui.gameOver.SetActive(true);
		ui.finalScoreText.text = pc.score.ToString();
		ui.finalBestText.gameObject.SetActive(false);

		var curBest = PlayerPrefs.GetInt("best");
		if (pc.score > curBest)
		{
			PlayerPrefs.SetInt("best", pc.score);
			ui.bestScore.gameObject.SetActive(true);
			ui.finalBestText.gameObject.SetActive(true);
		}

		if (pc.score < curBest)
		{
			ui.bestScore.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.R))
		{
			RestartScene();
		}

		if (!started || gameOver)
		{
			return;
		}

		ui.fuelSlider.value = pc.fuel;

		if (pc.fuel < 0)
		{
			GameOver();
			return;
		}

	}

	public void RestartScene()
	{
		time = tempTime;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}