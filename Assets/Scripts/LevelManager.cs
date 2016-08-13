using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public Color UIColour;
	public int levelNumber;
	public bool skipThisLevel;

	private GameManager gameman;
	
	void Start() {
		gameman = GameObject.FindObjectOfType<GameManager>();
		gameman.SetUIColour(UIColour);
		if (skipThisLevel && gameman.allowLevelSkip) {
			LoadNextLevel();
		}
		if (SceneManager.GetActiveScene().name.StartsWith(gameman.splashPrefix)) {
			// Splash scene
			AudioSource.PlayClipAtPoint (gameman.splashSound, transform.position, 1.0f);
			Invoke("LoadNextLevel", gameman.splashDelay);
		} else if (SceneManager.GetActiveScene().name.StartsWith(gameman.endPrefix)) {
			// End scene
			gameman.ReportGameEnd();
			GameObject.Find("FinalScore").GetComponent<Text>().text = string.Format("Final Score: {0}", gameman.theScore);
		} else if (SceneManager.GetActiveScene().name.StartsWith(gameman.levelPrefix)) {
			if (SceneManager.GetActiveScene().name == gameman.firstLevel) {
				// First Game Level
				gameman.ReportGameStart();
			}
			// Game Level
			gameman.ReportLevelStart(levelNumber);
		} else if (SceneManager.GetActiveScene().name.StartsWith(gameman.startPrefix)) {
			// Start Menu scene
			gameman.ReportMenu(levelNumber);
		}
	}
	
	public void LoadNextLevel() {
		Scene current = SceneManager.GetActiveScene();
		int newindex = current.buildIndex + 1;
		Debug.Log ("New Next Level load index: " + newindex);
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
		if (SceneManager.GetActiveScene().name.StartsWith(gameman.startPrefix)) {
			gameman.ResetScore();
		} 
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
		
	public void BrickDestroyed() {
		// Debug.Log("Ball destroyed Brick");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Break));
		if (Brick.breakableCount <= 0) {
			LoadNextLevel();
		}
	}

	public void BrickBounce() {
		// Debug.Log("Ball bounced off Brick");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Crack));
	}

	public void PaddleBounce() {
		// Debug.Log("Ball bounced off Paddle");
		gameman.CollisionScore(new Collision(Time.time, Collision.CollisionType.Bat));
	}
}
