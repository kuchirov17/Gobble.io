using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public PlayerControler pc;
	public MenuManager ui;
	public ItemsGenerator ig;
	public DragControl dc;
	
	

	public bool started;
	public bool gameOver=false;
	public bool soundOn = true;
	public float gravity = 10f;
	public float time = 120;
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
		dc.gameObject.SetActive(true);
		pc.StartCoroutine(pc.dropFuel());
		ig.StartCoroutine(ig.FoodsGenerator());
		ig.StartCoroutine(ig.ObstaclesGenerator());


	}

	public void GameOver()
	{
		gameOver = true;
		dc.gameObject.SetActive(false);
		pc.StopCoroutine(pc.dropFuel());
		ig.gameObject.SetActive(false);
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