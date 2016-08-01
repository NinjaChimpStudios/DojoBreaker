using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager Instance {get; private set;}
	
	private Stack<Collision> collisions;
	private const int defaultStackSize = 100;

	private int theScore = 0;
	private Text scoreText;
	private const string levelPrefix = "Level_";
	
	void Awake() {
		if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
 		Instance = this;
 		DontDestroyOnLoad(gameObject);
		collisions = new Stack<Collision>(defaultStackSize);
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
	}

	void Update() {
		if (SceneManager.GetActiveScene().name.StartsWith(levelPrefix)) {
			scoreText.enabled = true;
			scoreText.text = "SCORE: " + theScore;
		} else {
			scoreText.enabled = false;
		}
		
	}

	public void CollisionScore(Collision c) {
		collisions.Push(c);
		Debug.Log(String.Format(
			"New Collision detected, Time {0}, Type {1}, {2} in Stack",
			c.collisionTime,
			c.collisionType,
			collisions.Count ));
		// TODO handle scoring
		switch (c.collisionType) {
			case Collision.CollisionType.Crack :
				theScore++;
				break;
			case Collision.CollisionType.Break :
				theScore += 2;
				break;
			default :
				break;
		}
	}

	public void ResetScore() {
		Debug.Log("Resetting score to 0");
		theScore = 0;
	}

	public void ResetCollisionCount() {
		collisions.Clear();
	}

}
