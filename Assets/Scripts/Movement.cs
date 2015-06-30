using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public LayerMask obstructions;
	SpriteRenderer renderer;
	Animator anim;

	bool facingRight, facingNorth, stoppingX, stoppingY;
	bool usingController;

	Vector3 velocity;
	float accelRate, accelConst = 25, terminalVelocity, constantTerminalV;
	float angle;

	private DIRECTION direction;
	private enum DIRECTION{
		left, right, up, down, left_up, left_down, right_up, right_down

	};
	void Start () {

		anim = gameObject.GetComponent<Animator> ();


		velocity = new Vector3();
		terminalVelocity = 0;
		constantTerminalV = 8;
		accelRate = accelConst;

		renderer = gameObject.GetComponent<SpriteRenderer>();
		//renderer.sprite = north;
		direction = DIRECTION.up;

		Debug.Log (Input.GetJoystickNames().Length);

		if(Input.GetJoystickNames().Length > 0){

			usingController = true;
		}
		stoppingX = true;
		stoppingY = true;
	}
	public void moveRight(bool fromController, float angle){

		if(!fromController){
			this.angle = angle;
		}
		

		stoppingX = false;
		//this.renderer.sprite = this.east;
		
		if(accelRate < 0)
			accelRate = accelConst;

	}

	public void moveLeft(bool fromController, float angle){


		if(!fromController){
			this.angle = angle;
		}


		stoppingX = false;
		//this.renderer.sprite = this.east;

		if(accelRate < 0)
			accelRate = accelConst;

	}

	public void moveUp(bool fromController, float angle){

		if(!fromController){
			this.angle = angle;
		}
		

		stoppingY = false;

		if(accelRate < 0)
			accelRate = accelConst;
		
	}

	public void moveDown(bool fromController, float angle){

		if(!fromController){
			this.angle = angle;
		}
		

		stoppingY = false;
		//this.renderer.sprite = this.south;
		
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


	void controllerInput(){


		float x = Input.GetAxis("Horizontal");

		float y = Input.GetAxis ("Vertical") * -1;

		moveOnAngle(y, x);



		//Debug.Log (value);

	}

	void moveOnAngle(float y, float x){

		if(Mathf.Abs(x) + Mathf.Abs(y) > 1){
			angle = Mathf.Atan2 (y, x);

			if(accelRate < 0)
				accelRate = accelConst;
		}else{
			stopMoveX();
			stopMoveY();
		}


	}
	//if controller, pass control there, otherwise, keyboard input
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

		//RIGHT ARROW
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

		//upArrow
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

		//DOWN ARROW
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




		//CHECK IF DEION SHOULD STOP MOVING
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


	
		if(terminalVelocity < constantTerminalV || accelRate < 0){
			terminalVelocity += accelRate * Time.deltaTime;
		}

		if(accelRate < 0 && terminalVelocity <= 0){
			terminalVelocity = 0;
		}


		//Calculate angle in degrees for animator
		float inDegrees =  angle * 180 / Mathf.PI;
		if(inDegrees < 0){
			inDegrees = 180 + (180 + inDegrees);
		}

		//Set the animation variable values
		anim.SetFloat("direction", inDegrees);
		anim.SetFloat ("velocity", terminalVelocity);

		//Increase or slow velocity by acceleration
		velocity = new Vector3(terminalVelocity * Mathf.Cos(angle), terminalVelocity * Mathf.Sin(angle), velocity.z);
	
		//Change the distance for this frame
		transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime, transform.position.z);

	}
}
