using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public float minX, maxX;

	private Ball ball;
	private LevelManager levelManager;
	private GameManager gameman;
	private bool autoPlay = false;
	
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		gameman = GameObject.FindObjectOfType<GameManager>();
		autoPlay = gameman.autoPlay;
	}
		
	// Update is called once per frame
	void Update () {
		if (!autoPlay) {
			MoveWithMouse();
		} else {
			AutoPlay();
		}
	}
	
	void AutoPlay() {
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp(ballPos.x, minX, maxX);
		this.transform.position = paddlePos;
	}
	
	void MoveWithMouse () {
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		float mousePosInBlocks = Input.mousePosition.x / Screen.width * 20.48f;
		paddlePos.x = Mathf.Clamp(mousePosInBlocks, minX, maxX);
		this.transform.position = paddlePos;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		levelManager.PaddleBounce();
	}
}
