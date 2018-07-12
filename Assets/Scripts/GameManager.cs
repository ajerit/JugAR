using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int lives = 3;
	public int bricks = 12;
	public float resetDelay = 1f;
	public Text livesText;
	public Text gameOver;
	public Text youWon;
	public GameObject bricksPrefab;
	public GameObject ballPrefab;
	public GameObject ball;
	public GameObject paddle;
	//public GameObject deathParticles;
	public static GameManager Instance = null;

	//private GameObject clonePaddle;

	// Use this for initialization
	void Awake () {
		if (Instance == null) 
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

	}
	void Start() 
	{
		Setup();
	}

	public void Setup() {
		//clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
		//Instantiate(bricksPrefab, transform.position, Quaternion.identity);
		//ballClone = Instantiate(ballPrefab, transform.position, Quaternion.identity) as GameObject;
	}
	
	void CheckGameOver()
	{
		if (bricks < 1)
		{
			youWon.enabled = true;
			Time.timeScale = .25f;
			Invoke("Reset", resetDelay);
		}

		if (lives < 1) 
		{
			gameOver.enabled = true;
			Time.timeScale = .25f;
			ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
			Invoke("Reset", resetDelay);
		}
	}

	void Reset()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoseLife() 
	{
		lives--;
		livesText.text = "Vidas: " + lives;

		//Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
		//Destroy(ballClone);
		//Invoke("SetUpBall", resetDelay);
		ball.transform.position = new Vector3(paddle.transform.position.x + 0.2f, 0, paddle.transform.position.z + 0.2f);
		ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
		CheckGameOver();
	}

	void SetUpBall()
	{
		//ballClone = Instantiate(ballPrefab, transform.position, Quaternion.identity) as GameObject;
	}

	public void DestroyBrick()
	{
		bricks--;
		CheckGameOver();
	}
}
