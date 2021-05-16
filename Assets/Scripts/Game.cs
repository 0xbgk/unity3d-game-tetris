using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Game : MonoBehaviour
{	
	public static bool MoveRight = false;
	public static bool MoveLeft = false;
	public static bool MoveDown = false;	
	public static bool Rotate = false;	

	public static int gridWidth = 10;
	public static int gridHeight = 20;

	public static Transform[,] grid = new Transform[gridWidth, gridHeight];

	public static bool startingAtLevelZero;
	public static int StartingLevel;

	public Canvas hud_Canvas;
	public Canvas hud_PauseCanvas;

	public int scoreOneLine = 40;
	public int scoreTwoLine = 100;
	public int scoreThreeLine = 300;
	public int scoreFourLine = 1200;

	public int currentLevel = 1;
	private int clearedLinesCount = 0;
	private int didLevelChanged = 0;

	public static float fallSpeed = 1.0f;
	public static bool isPaused = false;

	public Text hud_ScoreText;
	public Text hud_LinesText;
	public Text hud_LevelText;

	public AudioClip lineClearedAudio;

	private int numbersOfRowsThisTurn = 0;

	private AudioSource audioSource;
	private AudioSource audioPitch;

	public static int CurrentScore = 0;

	private GameObject previewPiece;
	private GameObject nextPiece;

	private bool gameStarted = false;

	private int startingHighScore;

	private Vector2 previewPiecePosition = new Vector2(-2.0f, 26.5f);

	void Start()
	{
		Debug.Log("USER :  "+ PlayerPrefs.GetString("user"));
		CurrentScore = 0;

		hud_ScoreText.text = "0";

		hud_LevelText.text = currentLevel.ToString();

		hud_LinesText.text = "0";

		currentLevel = StartingLevel;

		SpawnNextPiece();
		audioSource = GetComponent<AudioSource>();
		audioPitch = GetComponent<AudioSource>();

		if (PlayerPrefs.GetInt("volume") == 0)
		{
			audioSource.volume = 0;
		}
		else if (PlayerPrefs.GetInt("volume") == 1)
		{
			audioSource.volume = 0.10f;
		}

		startingHighScore = PlayerPrefs.GetInt("highscore");
	}
	void Update()
	{
		if (PlayerPrefs.GetInt("volume") == 0)
		{
			audioSource.volume = 0;
		}
		else if (PlayerPrefs.GetInt("volume") == 1)
		{
			audioSource.volume = 0.10f;
		}
		UpdateScore();
		UpdateUI();
		UpdateLevel();
		UpdateSpeed();

		CheckUserInput();
	}

	void CheckUserInput()
	{
		if (Input.GetKeyUp(KeyCode.P))
		{
			if (Time.timeScale == 1)
			{
				PauseGame();
			}
			else
			{
				ResumeGame();
			}
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
		isPaused = true;
		audioSource.Pause();

		hud_Canvas.enabled = false;
		hud_PauseCanvas.enabled = true;
		Camera.main.GetComponent<Blur>().enabled = true;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		isPaused = false;
		audioSource.UnPause();

		hud_Canvas.enabled = true;
		hud_PauseCanvas.enabled = false;
		Camera.main.GetComponent<Blur>().enabled = false;

	}

	public void UpdateLevel()
	{
		if (startingAtLevelZero || (startingAtLevelZero == false && clearedLinesCount / 5 > StartingLevel))
		{
			currentLevel = clearedLinesCount / 5;
		}

		if (didLevelChanged < currentLevel)
		{
			audioPitch.pitch += 0.05f;
			didLevelChanged = currentLevel;
		}
	}
	public void UpdateSpeed()
	{
		fallSpeed = 1.0f - ((float)currentLevel * 0.1f);
	}
	public void UpdateUI()
	{
		hud_ScoreText.text = "" + CurrentScore.ToString();
		hud_LinesText.text = "" + clearedLinesCount.ToString();
		hud_LevelText.text = "" + currentLevel.ToString();
	}
	public void UpdateScore()
	{
		if (numbersOfRowsThisTurn > 0)
		{
			if (numbersOfRowsThisTurn == 1)
			{
				ClearedOneLine();
			}
			else if (numbersOfRowsThisTurn == 2)
			{
				ClearedTwoLines();
			}
			else if (numbersOfRowsThisTurn == 3)
			{
				ClearedThreeLines();
			}
			else if (numbersOfRowsThisTurn == 4)
			{
				ClearedFourLines();
			}
			numbersOfRowsThisTurn = 0;

			FindObjectOfType<Game>().UpdateHighScore();

			PlayLineClearedAudio();
		}
	}

	public void UpdateHighScore()
	{
		if (CurrentScore > startingHighScore)
		{
			PlayerPrefs.SetInt("highscore", CurrentScore);
		}

		PlayerPrefs.SetInt("lastscore", CurrentScore);
	}
	

	public void ClearedOneLine()
	{
		CurrentScore += scoreOneLine + (currentLevel * 10);
		clearedLinesCount++;
	}

	public void ClearedTwoLines()
	{
		CurrentScore += scoreTwoLine + (currentLevel * 15);
		clearedLinesCount += 2;
	}

	public void ClearedThreeLines()
	{
		CurrentScore += scoreThreeLine + (currentLevel * 20);
		clearedLinesCount += 3;
	}

	public void ClearedFourLines()
	{
		CurrentScore += scoreFourLine + (currentLevel * 50);
		clearedLinesCount += 4;
	}

	public void PlayLineClearedAudio()
	{
		audioSource.PlayOneShot(lineClearedAudio);
	}

	public bool CheckIsAboveGrid(TetrisPiece piece)
	{
		for (int x = 0; x < gridWidth; ++x)
		{
			foreach (Transform mino in piece.transform)
			{
				Vector2 pos = Round(mino.position);
				if (pos.y > gridHeight - 1)
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool IsFullRowAt(int y)
	{
		for (int x = 0; x < gridWidth; ++x)
		{
			if (grid[x, y] == null)
			{
				return false;
			}
		}

		numbersOfRowsThisTurn++;
		return true;
	}

	public void DeletePieceAt(int y)
	{
		for (int x = 0; x < gridWidth; ++x)
		{
			Destroy(grid[x, y].gameObject);
			grid[x, y] = null;
		}
	}

	public void MoveRowDown(int y)
	{
		for (int x = 0; x < gridWidth; ++x)
		{
			if (grid[x, y] != null)
			{
				grid[x, y - 1] = grid[x, y];
				grid[x, y] = null;

				grid[x, y - 1].position += new Vector3(0, -1, 0);
			}
		}
	}

	public void MoveAllRowsDown(int y)
	{
		for (int i = y; i < gridHeight; ++i)
		{
			MoveRowDown(i);
		}
	}

	public void DeletedRow()
	{
		for (int y = 0; y < gridHeight; ++y)
		{
			if (IsFullRowAt(y))
			{
				DeletePieceAt(y);
				MoveAllRowsDown(y + 1);

				--y;
			}
		}
	}

	public void UpdateGrid(TetrisPiece tetrisPiece)
	{
		for (int y = 0; y < gridHeight; ++y)
		{
			for (int x = 0; x < gridWidth; ++x)
			{
				if (grid[x, y] != null)
				{
					if (grid[x, y].parent == tetrisPiece.transform)
					{
						grid[x, y] = null;
					}
				}
			}
		}

		foreach (Transform piece in tetrisPiece.transform)
		{
			Vector2 pos = Round(piece.position);
			if (pos.y < gridHeight)
			{
				grid[(int)pos.x, (int)pos.y] = piece;
			}
		}
	}

	public Transform GetTransformAtGridPosition(Vector2 pos)
	{
		if (pos.y > gridHeight - 1)
		{
			return null;
		}
		else
		{
			return grid[(int)pos.x, (int)pos.y];
		}
	}

	public void SpawnNextPiece()
	{
		if (!gameStarted)
		{
			gameStarted = true;


			nextPiece = (GameObject)Instantiate(Resources.Load(GetRandomPiece(), typeof(GameObject)), new Vector2(4.0f, 20.0f), Quaternion.identity);
			previewPiece = (GameObject)Instantiate(Resources.Load(GetRandomPiece(), typeof(GameObject)), previewPiecePosition, Quaternion.identity);
			previewPiece.GetComponent<TetrisPiece>().enabled = false;
		}
		else
		{
			previewPiece.transform.localPosition = new Vector2(5.0f, 20.0f);
			nextPiece = previewPiece;
			nextPiece.GetComponent<TetrisPiece>().enabled = true;

			previewPiece = (GameObject)Instantiate(Resources.Load(GetRandomPiece(), typeof(GameObject)), previewPiecePosition, Quaternion.identity);
			previewPiece.GetComponent<TetrisPiece>().enabled = false;
		}
	}

	public bool CheckIsInsideGrid(Vector2 position)
	{
		return ((int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0);
	}

	public Vector2 Round(Vector2 position)
	{
		return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
	}

	string GetRandomPiece()
	{
		int randomPiece = Random.Range(1, 8);
		string randomPieceName = "Prefabs/Piece_Long";

		switch (randomPiece)
		{
			case 1:
				randomPieceName = "Prefabs/Piece_Long";
				break;
			case 2:
				randomPieceName = "Prefabs/Piece_Square";
				break;
			case 3:
				randomPieceName = "Prefabs/Piece_J";
				break;
			case 4:
				randomPieceName = "Prefabs/Piece_L";
				break;
			case 5:
				randomPieceName = "Prefabs/Piece_S";
				break;
			case 6:
				randomPieceName = "Prefabs/Piece_T";
				break;
			case 7:
				randomPieceName = "Prefabs/Piece_Z";
				break;
		}

		return randomPieceName;
	}
	
	public void MoveRightFunc()
	{		
		MoveRight = true;
	}
	public void MoveLeftFunc()
	{
		MoveLeft = true;
	}
	public void RotateFunc()
	{
		Rotate = true;
	}

	public void MoveDownFunc()
	{		
		MoveDown = true;
	}
	public void GameOver()
	{
		UIHandler handler = new UIHandler();
		handler.AddScore();
		SceneManager.LoadScene("GameOver");
	}
}
