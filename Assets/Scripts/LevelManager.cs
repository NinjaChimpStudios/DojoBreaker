using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public float splashDelay = 5.0f;
	public AudioClip splashSound;

	private ScoreManager score;
	
	void Start() {
		score = GameObject.FindObjectOfType<ScoreManager>();
		if (SceneManager.GetActiveScene().name == "Splash") {
			AudioSource.PlayClipAtPoint (splashSound, transform.position, 1.0f);
			Invoke("LoadNextLevel", splashDelay);
		}
	}
	
	public void LoadNextLevel() {
		Scene current = SceneManager.GetActiveScene();
		int newindex = current.buildIndex + 1;
		Debug.Log ("New Level load: " + newindex);
		Brick.breakableCount = 0;
		score.ResetCollisionCount();
		SceneManager.LoadScene(newindex);
	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Brick.breakableCount = 0;
		score.ResetCollisionCount();
		SceneManager.LoadScene(name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
		
	public void BrickDestroyed() {
		Debug.Log("Ball destroyed Brick");
		score.CollisionScore(new Collision(Time.time, Collision.CollisionType.Break));
		if (Brick.breakableCount <= 0) {
			LoadNextLevel();
		}
	}

	public void BrickBounce() {
		Debug.Log("Ball bounced off Brick");
		score.CollisionScore(new Collision(Time.time, Collision.CollisionType.Crack));
	}

	public void PaddleBounce() {
		Debug.Log("Ball bounced off Paddle");
		score.CollisionScore(new Collision(Time.time, Collision.CollisionType.Bat));
	}
}
