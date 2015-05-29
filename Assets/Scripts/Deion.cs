using UnityEngine;
using System.Collections;

public class Deion : MonoBehaviour {

	Sprite north, south, east;
	SpriteRenderer renderer;

	bool facingRight, facingNorth, stoppingX, stoppingY;
	bool usingController;

	Vector3 velocity;
	float accelRate, terminalVelocity, constantTerminalV;
	float angle;

	void Start () {

		north = Resources.Load("Sprites/Deion_North", typeof(Sprite)) as Sprite;
		south = Resources.Load("Sprites/Deion_South", typeof(Sprite)) as Sprite;
		east = Resources.Load("Sprites/Deion_East", typeof(Sprite)) as Sprite;

		velocity = new Vector3();
		terminalVelocity = 0;
		constantTerminalV = 10;
		accelRate = 5;

		renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = north;

		stoppingX = true;
		stoppingY = true;
	}

	public void moveLeft(bool fromController, float angle){

		if(!fromController){
			this.angle = angle;
		}

		flipX (false);

		stoppingX = false;
		this.renderer.sprite = this.east;

		if(accelRate < 0)
			accelRate *= -1;

	}

	public void stopMoveX(){
		stoppingX = true;
		if(stoppingX && stoppingY && accelRate > 0)
			accelRate *= -1;
	}
	public void stopMoveY(){
		stoppingY = true;

		if(stoppingX && stoppingY && accelRate > 0)
			accelRate *= -1;
	}

	public void flipX(bool facing){
		facingRight = facing;

		if((facingRight && transform.localScale.x < 0) || (!facingRight && transform.localScale.x > 0))
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	public void flipY(bool facing){
		facingNorth = facing;

		if((facingNorth && transform.localScale.y < 0) || (!facingNorth && transform.localScale.y > 0))
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.LeftArrow)){
			Debug.Log("asoetuhasoet");
			moveLeft (false, Mathf.PI);

		}else{
			stopMoveX();

		}



		if(usingController){
	
		}


		

		if(terminalVelocity < constantTerminalV){
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
