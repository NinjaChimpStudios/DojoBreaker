using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float splashDelay = 5.0f;
	public AudioClip splashSound;
	public Color UIColour;

	private GameManager gameman;
	private const string startPrefix = "Start";
	private const string endPrefix = "End";
	private const string levelPrefix = "Level";
	private const string firstLevel = "Level_01";
	
	void Start() {
		gameman = GameObject.FindObjectOfType<GameManager>();
		gameman.SetUIColour(UIColour);
		if (SceneManager.GetActiveScene().name == "Splash") {
			AudioSource.PlayClipAtPoint (splashSound, transform.position, 1.0f);
			Invoke("LoadNextLevel", splashDelay);
		} else if(SceneManager.GetActiveScene().name == firstLevel) {
			gameman.ReportGameStart();
		} else if (SceneManager.GetActiveScene().name.StartsWith(endPrefix)) {
			gameman.ReportGameEnd();
			GameObject.Find("FinalScore").GetComponent<Text>().text = string.Format("Final Score: {0}", gameman.theScore);
		}
		if (SceneManager.GetActiveScene().name.StartsWith(levelPrefix)) {
			gameman.ReportLevelStart(SceneManager.GetActiveScene().name);
		}
	}
	
	public void LoadNextLevel() {
		Scene current = SceneManager.GetActiveScene();
		int newindex = current.buildIndex + 1;
		Debug.Log ("New Level load: " + newindex);
		CheckScore();
		SceneManager.LoadScene(newindex);
	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		CheckScore();
		SceneManager.LoadScene(name);
	}

	private void CheckScore() {
		Brick.breakableCount = 0;
		gameman.ResetCollisionCount();
		if (SceneManager.GetActiveScene().name.StartsWith(startPrefix)) {
			gameman.ResetScore();
		} 
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
		
	public void BrickDestroyed() {
		Debug.Log("Ball destroyed Brick");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Break));
		if (Brick.breakableCount <= 0) {
			LoadNextLevel();
		}
	}

	public void BrickBounce() {
		Debug.Log("Ball bounced off Brick");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Crack));
	}

	public void PaddleBounce() {
		Debug.Log("Ball bounced off Paddle");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Bat));
	}
}
