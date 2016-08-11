using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance {get; private set;}
	public int theScore {get; private set;}

	private Stack<Collision> collisions;
	private const int defaultStackSize = 100;
	private Text scoreText;
	private const string levelPrefix = "Level_";
	
	void Awake() {
		if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
 		Instance = this;
		theScore = 0;
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

	public void SetUIColour(Color c) {
		scoreText.color = c;
	}

	public void CollisionScore(Collision c) {
		Debug.Log(String.Format(
			"New Collision detected, Time {0}, Type {1}, {2} in Stack",
			c.collisionTime,
			c.collisionType,
			collisions.Count ));
		int scoreMult = 1;
		if (collisions.Count > 0) {
			Collision prev = collisions.Peek();
			if (prev.collisionType != Collision.CollisionType.Bat) {
				scoreMult = 2; // double score if 2 brick hits in a row
			}
		}
		collisions.Push(c);
		switch (c.collisionType) {
			case Collision.CollisionType.Crack :
				theScore += (1 * scoreMult);
				break;
			case Collision.CollisionType.Break :
				theScore += (2 * scoreMult);
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
