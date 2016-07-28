using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public float splashDelay = 5.0f;
	public AudioClip splashSound;
	
	void Start() {
		if (SceneManager.GetActiveScene().name == "Splash") {
			AudioSource.PlayClipAtPoint (splashSound, transform.position, 1.0f);
			Invoke("LoadNextLevel", splashDelay);
		}
	}
	
	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Brick.breakableCount = 0;
		SceneManager.LoadScene(name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
	
	public void LoadNextLevel() {
		Brick.breakableCount = 0;
		// Application.LoadLevel(Application.loadedLevel + 1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void BrickDestoyed() {
		if (Brick.breakableCount <= 0) {
			LoadNextLevel();
		}
	}
}
