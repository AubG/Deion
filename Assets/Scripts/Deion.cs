using UnityEngine;
using System.Collections;

public class Deion : MonoBehaviour {

	Sprite north, south, east;
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

		north = Resources.Load("Sprites/Deion_North", typeof(Sprite)) as Sprite;
		south = Resources.Load("Sprites/Deion_South", typeof(Sprite)) as Sprite;
		east = Resources.Load("Sprites/Deion_East", typeof(Sprite)) as Sprite;

		velocity = new Vector3();
		terminalVelocity = 0;
		constantTerminalV = 6;
		accelRate = accelConst;

		renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = north;
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
		
		flipX (true);
		
		stoppingX = false;
		this.renderer.sprite = this.east;
		
		if(accelRate < 0)
			accelRate = accelConst;

	}

	public void moveLeft(bool fromController, float angle){


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

		if(!fromController){
			this.angle = angle;
		}
		

		stoppingY = false;
		this.renderer.sprite = this.north;
		
		if(accelRate < 0)
			accelRate = accelConst;
		
	}

	public void moveDown(bool fromController, float angle){

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


		float x = Input.GetAxis("Horizontal");

		float y = Input.GetAxis ("Vertical") * -1;

		moveOnAngle(y, x);



		//Debug.Log (value);

	}

	void moveOnAngle(float y, float x){

		if(Mathf.Abs(x) + Mathf.Abs(y) > 1){
			angle = Mathf.Atan2 (y, x);
			float inDegrees =  angle * 180 / Mathf.PI;

			anim.SetFloat("direction", inDegrees);

			if(inDegrees < 22.5 && inDegrees > 337.5){
				//Image should be right
				if(direction != DIRECTION.right){
					//animate right

					direction = DIRECTION.right;
				}


			}else if(inDegrees > 22.5 && inDegrees < 67.5){
				//Image should be right up
				if(direction != DIRECTION.right_up){
					//animate right up
					direction = DIRECTION.right_up;
				}
			}else if(inDegrees > 67.5 && inDegrees < 112.5){

				//Image should be up 
				if(direction != DIRECTION.up){
					//animate up
					direction = DIRECTION.up;
				}

			}else if(inDegrees > 112.5 && inDegrees < 157.5){

				//Image should be up left
				if(direction != DIRECTION.left_up){
					//animate left up

					direction = DIRECTION.left_up;
				}
			}else if(inDegrees > 157.5 && inDegrees < 202.5){
				//Image should be left

				if(direction != DIRECTION.left){
					//animate left


					direction = DIRECTION.left;
				}
			}else if(inDegrees > 202.5 && inDegrees < 247.5){
				//Image should be down left
				if(direction != DIRECTION.left_down){
					//animate left_down

					direction = DIRECTION.left_down;
				}
			}else if(inDegrees > 247.5 && inDegrees < 292.5){
				//Image should be down
				if(direction != DIRECTION.down){
					//animate down
					direction = DIRECTION.down;
				}
			}else if(inDegrees > 292.5 && inDegrees < 337.5){
				//Image should be down right

				if(direction != DIRECTION.right_down){
					//animate down right

					direction = DIRECTION.right_down;
				}
			}

			if(accelRate < 0)
				accelRate = accelConst;
		}else{
			stopMoveX();
			stopMoveY();
			anim.SetFloat("direction", 0);
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

		//Increase or slow velocity by acceleration
		velocity = new Vector3(terminalVelocity * Mathf.Cos(angle), terminalVelocity * Mathf.Sin(angle), velocity.z);
	
		//Change the distance for this frame
		transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime, transform.position.z);

	}
}
