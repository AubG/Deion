using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	Deion player;
	bool shakeX, shakeY;
	int horizontalPush = 0, verticalPush = 0;
	float magnitude = .02f;
	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find("Deion");
		player = (Deion)obj.GetComponent ("Deion");
	}

	// Update is called once per frame
	void Update () {
		Vector3 newpos = player.transform.position;
		transform.position = new Vector3(newpos.x, newpos.y, -20);
		//applyCameraEffects ();

		//handleDebugInput ();
	}

	void handleDebugInput(){
		bool leftArrow, rightArrow, upArrow, downArrow, space;
		leftArrow = Input.GetKey(KeyCode.A);
		rightArrow = Input.GetKey(KeyCode.D);
		upArrow = Input.GetKey(KeyCode.W);
		downArrow = Input.GetKey(KeyCode.S);
		space = Input.GetKey (KeyCode.Space);
		float pushMag = magnitude;

		if (leftArrow) {
			pushX(-pushMag);
		}

		if (rightArrow) {
			pushX (pushMag);
		}

		if (upArrow) {
			pushY (pushMag);
		}

		if (downArrow) {
			pushY (-pushMag);
		}

		if (space) {
			this.shakeCamera(pushMag, pushMag);
		}

	}

	void applyCameraEffects(){
		Vector3 position = transform.position;
		float shakeDis = Random.Range (0, magnitude);
		int random = Random.Range (0, 9);
		//if shaking, it will flip direction the camera is pushed everytime
		if (shakeX) {
			if(random > 4)
				horizontalPush = -horizontalPush;

			if(horizontalPush == 0){
				shakeX = false;
			}
		}
		if (shakeY) {
			if(random%2 == 0)
				verticalPush = -verticalPush;

			if(verticalPush == 0){
				shakeY = false;
			}
		}


		//decrement the frames of shaking, and actually shake
		if (horizontalPush < 0) {
			horizontalPush++;
			position.x -= shakeDis * Mathf.Abs(horizontalPush);

		} else if (horizontalPush > 0) {
			horizontalPush--;
			position.x += shakeDis * Mathf.Abs(horizontalPush);
		}

		if (verticalPush < 0) {
			verticalPush++;
			position.y -= shakeDis * Mathf.Abs(verticalPush);

		} else if (verticalPush > 0) {
			verticalPush--;
			position.y += shakeDis * Mathf.Abs(verticalPush);
		}

		transform.position = new Vector3(position.x, position.y, -10);
	}

	public void shakeCamera(float horizontal, float vertical){
		pushX (horizontal);
		pushY (vertical);
	}

	public void pushX(float magnitude){
		horizontalPush = 20;

		if(verticalPush ==0)
			verticalPush = 5;

		this.magnitude = magnitude;
		shakeY = true;
	}

	public void pushY(float magnitude){
		verticalPush = 20;

		if(horizontalPush == 0)
			horizontalPush = 5;


		this.magnitude = magnitude;
		shakeX = true;
	}
}
