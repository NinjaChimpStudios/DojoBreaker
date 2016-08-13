using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

	public static GameManager Instance {get; private set;}

	public float splashDelay;
	public AudioClip splashSound;
	public string levelPrefix;
	public string startPrefix;
	public string endPrefix;
	public string firstLevel;
	public string splashPrefix;
	public bool allowLevelSkip;
	public bool autoPlay;

	public int theScore {get; private set;}
	public int latestLevel {get; private set;}

	private Stack<Collision> collisions;
	private const int defaultStackSize = 100;
	private Text scoreText;
	private int highestLevel;
	private int highestScore = 0;
	private int gameCount = 0;
	
	void Awake() {
		if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
 		Instance = this;
		theScore = 0;
 		DontDestroyOnLoad(gameObject);
		collisions = new Stack<Collision>(defaultStackSize);
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		latestLevel = 0;
	}

	void Update() {
		// TODO DOJO-20
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
		// Debug.Log(String.Format(
		//	"New Collision detected, Time {0}, Type {1}, {2} in Stack",
		//	c.collisionTime,
		//	c.collisionType,
		//	collisions.Count ));
		int scoreMult = 1;
		bool doubleBat = false;
		if (collisions.Count > 0) {
			Collision prev = collisions.Peek();
			if (prev.collisionType != Collision.CollisionType.Bat) {
				scoreMult = 2; // double score if 2 brick hits in a row
			} else if (c.collisionType == Collision.CollisionType.Bat) {
				doubleBat = true; // 2 bat hits in a row = bad
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
			case Collision.CollisionType.Bat :
				theScore += (doubleBat) ? -1 : 0;
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

	public void ReportGameStart() {
		Debug.Log("Game Start Reported");
		gameCount++;
	}

	public void ReportLevelStart(int level) {
		latestLevel = level;
		Debug.Log("Level Start Reported for :" + level + ":");
		Debug.Log("Latest Level :" + latestLevel + ":");
		Debug.Log("Highest Level :" + highestLevel + ":");
		if (level > highestLevel) {
			highestLevel = level;
			Debug.Log("Highest Level updated to :" + highestLevel + ":");
		}
	}

	public void ReportMenu(int level) {
		Debug.Log("StartMenu Reported");
		latestLevel = level;
	}
	
	public void ReportLevelEnd() {
		Debug.Log("Level End Reported");
	}
	
	public void ReportGameEnd() {
		if (theScore > highestScore) {
			highestScore = theScore;
		}
		Debug.Log(string.Format("Game End Reported: Score {0} Level {1} Highest Score {2} Highest Level {3} Game Count {4}",
			theScore,
			latestLevel,
			highestScore,
			highestLevel,
			gameCount));
		Dictionary<string, object> d = new Dictionary<string, object> {
			{ "score", theScore },
			{ "level", latestLevel },
			{ "highestScore", highestScore },
			{ "highestLevel", highestLevel },
			{ "gameCount", gameCount }
		};
		ReportAnalytics("dojobreaker.game.end", d);
	}
	
	public void ReportQuit() {

	}

	private void ReportAnalytics(string tag, Dictionary<string, object> data) {
		Analytics.CustomEvent(tag, data);
  	}

}
