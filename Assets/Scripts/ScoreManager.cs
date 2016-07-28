using UnityEngine;
using System;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager Instance {get; private set;}
	
	private Stack<Collision> collisions;
	private const int defaultStackSize = 100;

	void Awake() {
		if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
 		Instance = this;
 		DontDestroyOnLoad(gameObject);
		collisions = new Stack<Collision>(defaultStackSize);
	}

	public void CollisionScore(Collision c) {
		collisions.Push(c);
		Debug.Log(String.Format(
			"New Collision detected, Time {0}, Type {1}, {2} in Stack",
			c.collisionTime,
			c.collisionType,
			collisions.Count ));
		// TODO handle scoring
	}

	public void ResetScore() {
		// TODO
	}

	public void ResetCollisionCount() {
		collisions.Clear();
	}

}
