using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject[] particles;

    public float penaltyTime = 2f; // penalty time if the player presses the wrong button
    public Text score_text;
    public Text gameOver_text;
    public Text instructions_text;
    public Text highScore_text;
    public bool gameOverStatus;
    public bool gameInitializedStatus;
    public AudioSource hit_sfx;
    public AudioSource miss_sfx;

    private static int highScore = 0;

    private Queue<GameObject> cubeQueue = new Queue<GameObject>(); // array of the cubes that are present in the game
    private Queue<int> colorQueue = new Queue<int>(); // specifier that tells the colors of the cube that are present
    private int total_score;
    private bool timeOut; // bool that tells the function on whether the player has pressed the wrong button

    const int MAX_CUBES = 8;
    const float TIME_TO_RESTART = 4f;

	// Use this for initialization
	void Start () {
        gameOverStatus = false;
        timeOut = false;
        total_score = 0;
        if (PlayerPrefs.HasKey("highScore")) // checks if highscore data exists
        {
            highScore_text.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString(); // if data exist, just load
        }
        else
        {
            highScore_text.text = "High Score: " + highScore.ToString(); // else, just use the static variable
        }
        
        gameInitializedStatus = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameOverCheck())
        {
            EndGame();
        }
        QuitGameCheck();
        if (colorQueue.Count != 0 && timeOut == false)
        {
            if(colorQueue.Peek() == 0)
            {
                if (Input.GetKeyDown("w"))
                {
                    hit_sfx.Play();
                    Dequeue();
                    total_score++;
                    score_text.text = total_score.ToString();
                    if (!gameInitializedStatus)
                    {
                        InitializeGame();
                    }
                }
                else if (Input.GetKeyDown("e") && gameInitializedStatus)
                {
                    miss_sfx.Play();
                    timeOut = true;
                    Invoke("ReleaseTimeOut", penaltyTime);
                }
            }
            else if(colorQueue.Peek() == 1)
            {
                if (Input.GetKeyDown("e"))
                {
                    hit_sfx.Play();
                    Dequeue();
                    total_score++;
                    score_text.text = total_score.ToString();
                    if (!gameInitializedStatus)
                    {
                        InitializeGame();
                    }
                }
                else if(Input.GetKeyDown("w") && gameInitializedStatus)
                {
                    miss_sfx.Play();
                    timeOut = true;
                    Invoke("ReleaseTimeOut", penaltyTime);
                }
            }
        }
	}

    public void Enqueue(GameObject cube, int cube_id) // Enqueues the cube GameObject and the color associated with it in a seperate queue.
    {
        cubeQueue.Enqueue(cube);
        colorQueue.Enqueue(cube_id);
    }

    public void Dequeue() // Destroys last object and dequeues both the cubeQueue and colorQueue.
    {
        Instantiate(particles[colorQueue.Peek()], cubeQueue.Peek().transform.position, Quaternion.identity);
        Destroy(cubeQueue.Peek());
        cubeQueue.Dequeue();
        colorQueue.Dequeue();
    }

    public int Peek() // Peeks the colorQueue.
    {
        return colorQueue.Peek();   
    }

    private void ReleaseTimeOut() // Switches the timeOut bool variable to false
    {
        timeOut = false;
    }

    private bool GameOverCheck()
    {
        return colorQueue.Count >= MAX_CUBES;
    }

    private void EndGame()
    {
        timeOut = true;
        gameOver_text.enabled = true;
        gameOverStatus = true;
        if(total_score > highScore)
        {
            highScore = total_score;
            PlayerPrefs.SetInt("highScore", highScore); // saves highscore externally
        }
        Invoke("Restart", TIME_TO_RESTART);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InitializeGame()
    {
        gameInitializedStatus = true;
        instructions_text.enabled = false;
        highScore_text.enabled = false;
        score_text.enabled = true;
    }

    private void QuitGameCheck()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
