using UnityEngine;
using UnityEngine.UI;

public class TetrisPiece : MonoBehaviour
{
	float fall = 0;
	private float fallSpeed;
	public bool allowRotation = true;
	public bool limitRotation = false;

	public Button Right;

	public AudioClip moveSound;
	public AudioClip rotateSound;
	public AudioClip landSound;

	private float continuousVerticalSpeed = 0.05f; // Down tusuna basınca parcanın inme hızı
	private float continuousHorizontalSpeed = 0.1f; // Right Left tuslarına basınca parcanın hareket hızı
	private float buttonDownWaitMax = 0.2f; // Parcanın hareketinin baslaması icin kac saniye beklemeliyiz.

	private float verticalTimer = 0;
	private float horizontalTimer = 0;
	private float buttonDownWaitTimerHorizontal = 0;
	private float buttonDownWaitTimerVertical = 0;

	private bool movedImmediateHorizontal = false;
	private bool movedImmediateVertical = false;

	public int individualScore = 100;
	private float individualScoreTime;

	private AudioSource audioSource;

	// Variables for touch event
	private int touchSensitivityHorizontal = 4;
	private int touchSensitivityVertical = 2;

	Vector2 previusUnitPosition = Vector2.zero;
	Vector2 direction = Vector2.zero;

	bool moved = false;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (!Game.isPaused)
		{
			CheckUserInput();
			UpdateIndividualScore();
			UpdateFallSpeed();
		}
	}

	void UpdateFallSpeed()
	{
		fallSpeed = Game.fallSpeed;
	}

	void UpdateIndividualScore()
	{
		if (individualScoreTime < 1)
		{
			individualScoreTime += Time.deltaTime;
		}
		else
		{
			individualScoreTime = 0;
			individualScore = Mathf.Max(individualScore - 10, 0);
		}
	}

	private void CheckUserInput()
	{
		// Mobile
		/*
		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				previusUnitPosition = new Vector2(t.position.x, t.position.y);
			}
			else if (t.phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPosition = t.deltaPosition;
				direction = touchDeltaPosition.normalized;

				if (Mathf.Abs(t.position.x - previusUnitPosition.x) >= touchSensitivityHorizontal && direction.x < 0 && t.deltaPosition.y > -10 && t.deltaPosition.y < 10)
				{
					// Move Left
					MoveLeft();
					previusUnitPosition = t.position;
					moved = true;
				}
				else if (Mathf.Abs(t.position.x - previusUnitPosition.x) >= touchSensitivityHorizontal && direction.x > 0 && t.deltaPosition.y > -10 && t.deltaPosition.y < 10)
				{
					// Move Right
					MoveRight();
					previusUnitPosition = t.position;
					moved = true;
				}
				else if (Mathf.Abs(t.position.y - previusUnitPosition.y) >= touchSensitivityVertical && direction.y < 0 && t.deltaPosition.x > -10 && t.deltaPosition.x < 10)
				{
					// Move Down
					MoveDown();
					previusUnitPosition = t.position;
					moved = true;
				}
			}
			else if (t.phase == TouchPhase.Ended)
			{
				if (!moved && t.position.x > Screen.width / 4)
				{
					Rotate();
				}

				moved = false;
			}
		}
		if (Time.time - fall > fallSpeed)
		{
			MoveDown();
		}
		*/
		if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Game.MoveRight || Game.MoveLeft)
		{
			movedImmediateHorizontal = false;
			horizontalTimer = 0;
			buttonDownWaitTimerHorizontal = 0;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			movedImmediateVertical = false;
			verticalTimer = 0;
			buttonDownWaitTimerVertical = 0;
		}

		if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed || Game.MoveDown)
		{			
			MoveDown();
		}
		if (Input.GetKey(KeyCode.RightArrow) || Game.MoveRight)
		{
			MoveRight();
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Game.MoveLeft)
		{
			MoveLeft();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Game.Rotate)
		{
			Rotate();
		}
	}

	public void MoveRight()
	{
		if (Game.MoveRight)
		{
			if (movedImmediateHorizontal)
			{
				if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
				{
					buttonDownWaitTimerHorizontal += Time.deltaTime;
					return;
				}

				if (horizontalTimer < continuousHorizontalSpeed)
				{
					horizontalTimer += Time.deltaTime;
					return;
				}
			}
			if (!movedImmediateHorizontal)
			{
				movedImmediateHorizontal = true;
			}

			horizontalTimer = 0;

			transform.position += new Vector3(1, 0, 0);

			if (CheckIsValidPosition())
			{
				FindObjectOfType<Game>().UpdateGrid(this);
				PlayMoveAudio();
			}
			else
			{
				transform.position += new Vector3(-1, 0, 0);
			}
		}
		Game.MoveRight = false;


	}

	public void MoveLeft()
	{
		if (Game.MoveLeft)
		{
			if (movedImmediateHorizontal)
			{
				if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
				{
					buttonDownWaitTimerHorizontal += Time.deltaTime;
					return;
				}

				if (horizontalTimer < continuousHorizontalSpeed)
				{
					horizontalTimer += Time.deltaTime;
					return;
				}
			}

			if (!movedImmediateHorizontal)
			{
				movedImmediateHorizontal = true;
			}

			horizontalTimer = 0;

			transform.position += new Vector3(-1, 0, 0);
			if (CheckIsValidPosition())
			{
				FindObjectOfType<Game>().UpdateGrid(this);
				PlayMoveAudio();
			}
			else
			{
				transform.position += new Vector3(1, 0, 0);
			}
		}
		Game.MoveLeft = false;

	}

	public void MoveDown()
	{
		if (Game.MoveDown)
		{			
			if (movedImmediateVertical)
			{
				if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
				{
					buttonDownWaitTimerHorizontal += Time.deltaTime;
					return;
				}

				if (verticalTimer < continuousVerticalSpeed)
				{
					verticalTimer += Time.deltaTime;
					return;
				}
			}
			if (!movedImmediateVertical)
			{
				movedImmediateVertical = true;
			}

			verticalTimer = 0;

			transform.position += new Vector3(0, -1, 0);

			if (CheckIsValidPosition())
			{
				FindObjectOfType<Game>().UpdateGrid(this);
				if (Input.GetKey(KeyCode.DownArrow))
				{
					PlayMoveAudio();
				}
			}
			else
			{
				transform.position += new Vector3(0, 1, 0);

				FindObjectOfType<Game>().DeletedRow();

				if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
				{
					FindObjectOfType<Game>().GameOver();
				}


				PlayLandAudio();
				FindObjectOfType<Game>().SpawnNextPiece();
				Game.MoveDown = false;

				Game.CurrentScore += individualScore;

				FindObjectOfType<Game>().UpdateHighScore();

				enabled = false;
			}

			fall = Time.time;
		}
		else
		{
			if (movedImmediateVertical)
			{
				if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
				{
					buttonDownWaitTimerHorizontal += Time.deltaTime;
					return;
				}

				if (verticalTimer < continuousVerticalSpeed)
				{
					verticalTimer += Time.deltaTime;
					return;
				}
			}
			if (!movedImmediateVertical)
			{
				movedImmediateVertical = true;
			}

			verticalTimer = 0;

			transform.position += new Vector3(0, -1, 0);

			if (CheckIsValidPosition())
			{
				FindObjectOfType<Game>().UpdateGrid(this);
				if (Input.GetKey(KeyCode.DownArrow))
				{
					PlayMoveAudio();
				}
			}
			else
			{
				transform.position += new Vector3(0, 1, 0);

				FindObjectOfType<Game>().DeletedRow();

				if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
				{
					FindObjectOfType<Game>().GameOver();
				}


				PlayLandAudio();
				FindObjectOfType<Game>().SpawnNextPiece();


				Game.CurrentScore += individualScore;

				FindObjectOfType<Game>().UpdateHighScore();

				enabled = false;
			}

			fall = Time.time;
		}
	}

	public void Rotate()
	{
		if (Game.Rotate)
		{
			if (allowRotation)
			{
				if (limitRotation)
				{
					if (transform.rotation.eulerAngles.z >= 90)
					{
						transform.Rotate(0, 0, -90);
					}
					else
					{
						transform.Rotate(0, 0, 90);
					}
				}
				else
				{
					transform.Rotate(0, 0, 90);
				}

				if (CheckIsValidPosition())
				{
					FindObjectOfType<Game>().UpdateGrid(this);
					PlayRotateAudio();
				}
				else
				{
					if (limitRotation)
					{
						if (transform.rotation.eulerAngles.z >= 90)
						{
							transform.Rotate(0, 0, -90);
						}
						else
						{
							transform.Rotate(0, 0, 90);
						}

					}
					else
					{
						transform.Rotate(0, 0, -90);
					}
				}
			}
		}
		Game.Rotate = false;
	}

	void PlayMoveAudio()
	{
		audioSource.PlayOneShot(moveSound);
	}

	void PlayRotateAudio()
	{
		audioSource.PlayOneShot(rotateSound);
	}

	void PlayLandAudio()
	{
		audioSource.PlayOneShot(landSound);
	}

	bool CheckIsValidPosition()
	{
		foreach (Transform mino in transform)
		{
			Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
			if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
			{
				return false;
			}

			if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
			{
				return false;
			}
		}

		return true;
	}
}
