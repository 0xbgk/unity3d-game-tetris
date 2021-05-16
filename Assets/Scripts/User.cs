using UnityEngine;

public class User : MonoBehaviour
{
	public string userName;
	public int userScore;

	public User(string username, int userscore)
	{
		userName = username;
		userScore = userscore;
	}
}
