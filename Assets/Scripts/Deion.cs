using UnityEngine;
using System.Collections;

public class Deion : MonoBehaviour {

	Sprite north, south, east;
	SpriteRenderer renderer;

	bool facingRight, facingNorth, stoppingX, stoppingY;
	bool usingController;

	Vector3 velocity;
	float accelRate, accelConst = 25, terminalVelocity, constantTerminalV;
	float angle;

	void Start () {

		north = Resources.Load("Sprites/Deion_North", typeof(Sprite)) as Sprite;
		south = Resources.Load("Sprites/Deion_South", typeof(Sprite)) as Sprite;
		east = Resources.Load("Sprites/Deion_East", typeof(Sprite)) as Sprite;

		velocity = new Vector3();
		terminalVelocity = 0;
		constantTerminalV = 6;
		accelRate = accelConst;

		renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = north;

		if(Input.GetJoystickNames().Length > 0){
			Debug.Log ("It ain't zero");

			usingController = true;
		}
		stoppingX = true;
		stoppingY = true;
	}
	public void moveRight(bool fromController, float angle){

		Debug.Log ("Right");
		if(!fromController){
			this.angle = angle;
		}
		
		flipX (true);
		
		stoppingX = false;
		this.renderer.sprite = this.east;
		
		if(accelRate < 0)
			accelRate = accelConst;

	}

	public void moveLeft(bool fromController, float angle){

		Debug.Log ("Left");

		if(!fromController){
			this.angle = angle;
		}

		flipX (false);

		stoppingX = false;
		this.renderer.sprite = this.east;

		if(accelRate < 0)
			accelRate = accelConst;

	}

	public void moveUp(bool fromController, float angle){
		Debug.Log ("Up");

		if(!fromController){
			this.angle = angle;
		}
		

		stoppingY = false;
		this.renderer.sprite = this.north;
		
		if(accelRate < 0)
			accelRate = accelConst;
		
	}

	public void moveDown(bool fromController, float angle){
		Debug.Log ("Down");
		if(!fromController){
			this.angle = angle;
		}
		

		stoppingY = false;
		this.renderer.sprite = this.south;
		
		if(accelRate < 0)
			accelRate = accelConst;
		
	}

	public void stopMoveX(){
		stoppingX = true;
		if(stoppingX && stoppingY && accelRate > 0)
			accelRate = -3 * accelConst;
	}
	public void stopMoveY(){
		stoppingY = true;

		if(stoppingX && stoppingY && accelRate > 0)
			accelRate = -3 * accelConst;
	}

	public void flipX(bool facing){
		facingRight = facing;

		if((facingRight && transform.localScale.x < 0) || (!facingRight && transform.localScale.x > 0))
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}



	void controllerInput(){
		float value = Input.GetAxis("Horizontal");
		if(value < -.6f){
			moveLeft (true, 0);
		}else if(value > .6f){
			moveRight (true, 0);
		}else{
			stopMoveX();
		}

		value = Input.GetAxis ("Vertical") * -1;
		if(value < -.6f){
			moveDown (true, 0);
		}else if(value > .6f){
			moveUp (true, 0);
		}else{
			stopMoveY();
		}

		//Debug.Log (value);

	}
	void handleInput(){

		if(usingController){
			controllerInput();
			return;
		}


		bool leftArrow, rightArrow, upArrow, downArrow;
		leftArrow = Input.GetKey(KeyCode.LeftArrow);
		rightArrow = Input.GetKey(KeyCode.RightArrow);
		upArrow = Input.GetKey(KeyCode.UpArrow);
		downArrow = Input.GetKey(KeyCode.DownArrow);



		//Handle Left Arrow being down
		if(leftArrow && !rightArrow){

			if(upArrow && !downArrow){
				//Up Left
			}

			if(downArrow && !upArrow){
				//Down left
			}

			if((downArrow && upArrow) || (!downArrow && !upArrow)){

				moveLeft (false, Mathf.PI);
			}

				
		}

		if(rightArrow && !leftArrow){

				if(upArrow && !downArrow){
					//Up Right
				}
				
				if(downArrow && !upArrow){
					//Down Right
				}
				
				if((downArrow && upArrow) || (!downArrow && !upArrow)){
					
					moveRight (false, 0);
				}
		}

		//Handle Left Arrow being down
		if(upArrow && !downArrow){
			
			if(leftArrow && !rightArrow){
				//Up Left
			}
			
			if(rightArrow && !leftArrow){
				//Down left

			}
			
			if((leftArrow && rightArrow) || (!leftArrow && !rightArrow)){
				
				moveUp (false, Mathf.PI / 2);
			}
		}
		
		if(downArrow && !upArrow){
			
			if(leftArrow && !rightArrow){
				//Up up
			}
			
			if(rightArrow && !leftArrow){
				//Down up
			}
			
			if((leftArrow && rightArrow) || (!leftArrow && !rightArrow)){
				
				moveDown (false, Mathf.PI * 3 / 2);
			}
		}





		if((!rightArrow && !leftArrow) || (rightArrow && leftArrow)){
			stopMoveX();
		}
		if((!upArrow && !downArrow) || (upArrow && downArrow)){
			stopMoveY();
		}

	}
	// Update is called once per frame
	void Update () {

		handleInput();



		if(usingController){
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");

			if(Mathf.Abs(x) + Mathf.Abs(y) > 1)
			angle = Mathf.Atan2 (y * -1, x);

			Debug.Log (angle);


		}





		if(terminalVelocity < constantTerminalV || accelRate < 0){
			terminalVelocity += accelRate * Time.deltaTime;
		}

		if(accelRate < 0 && terminalVelocity <= 0){
			terminalVelocity = 0;
		}

		//Increase or slow velocity by acceleration
		velocity = new Vector3(terminalVelocity * Mathf.Cos(angle), terminalVelocity * Mathf.Sin(angle), velocity.z);




		//Change the distance for this frame
		transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime, transform.position.z);

	}
}
