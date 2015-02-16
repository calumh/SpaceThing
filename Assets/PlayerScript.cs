//Code modified from http://pixelnest.io/tutorials/2d-game-unity/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
    /// <summary>
    /// 1 - The speed of the ship
    /// </summary>
    //public Vector2 speed = new Vector2(50, 50);
   
    // 2 - Store the movement
	private bool shoot = false;
    public float speed;
	public blockHandler bh;
	// connect virtual joystick
	public CNAbstractController MovementJoystick;
    //private Vector2 movement;
    //public bool isLinkedToCamera = true;
	void OnGUI()
	{

		//DrawQuad(MovementJoystick.GetComponent<CNJoystick>().camera.rect, new Color32(255, 255, 255, 255));
	}
	void DrawQuad(Rect position, Color color)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}/*
	bool OverGui()
	{ // return true if in forbidden region
		
		var mouse = Input.mousePosition;
		print(cam.rect.Contains(mouse));
		return cam.rect.Contains(mouse);
	}*/
    public float stopForcePoint;
	bool mouseButtondown = false;
	bool touching = false;
	
	void Start(){
		bh = GetComponent<blockHandler>();
		//MovementJoystick.FingerTouchedEvent += joystickTouched(MovementJoystick);
	}
	CNAbstractController joystickTouched()
	{
		return null;
	}
	public bool finger1Touched = false;
	public bool finger2Touched = false;
	public int numOfFing = 0;

	public int joyTouchedID = 5;
	public int mouseTouchedID= 5;
	public List<Touch> previous = new List<Touch>();
	/*void Update()
	{
		
		
		finger1Touched = MovementJoystick.GetComponent<CNJoystick>().touched;
		Touch[] currentT = Input.touches;
		List<Touch> current = new List<Touch>();
		
		foreach (Touch entry in currentT){
			current.Add(entry);
		}


		if (current.Count == previous.Count)
		{
			// do nothing
		}
		else if (current.Count > previous.Count)
		{
			//find new touch ID
			int newTouchID = 0;
			foreach (Touch entry in current){
				if(!previous.Contains(entry))
					newTouchID = entry.fingerId;
			}

			if (finger1Touched)
			{
				joyTouchedID = newTouchID;
				print("joy");
			}
			else
			{
				mouseTouchedID = newTouchID;
				print("touch");
			}
		}
		previous = current;
		//print("joy: " + joyTouchedID + "   mouse: " + mouseTouchedID);

		//print(Input.touches);
		*/
		/*
		if (Input.GetTouch(0).phase == TouchPhase.Began)
			finger1Touched = true;
		if (Input.GetTouch(0).phase == TouchPhase.Ended)
			finger1Touched = false;
		if (Input.GetTouch(1).phase == TouchPhase.Began)
			finger2Touched = true;
		if (Input.GetTouch(0).phase == TouchPhase.Ended)
			finger1Touched = false;
		if (finger1Touched && finger2Touched)
			numOfFing = 2;
		else if (finger1Touched || finger2Touched)
			numOfFing = 1;
		else
			numOfFing = 0;
		
		
		
		if (Input.GetMouseButtonDown(0))
			mouseButtondown = true;
		else if(Input.GetMouseButtonUp(0))
		{
			mouseButtondown = false;
		}
		

	}*/
	//touch stuff
	/*
	void FixedUpdate()
	{
		// 5 - Move the game object
		// Move the camera

		Vector3 inputForce = new Vector3(0,0,0);
		// rigidbody2D.velocity = movement;
		if (numOfFing == 2 && MovementJoystick.GetComponent<CNJoystick>().touched)
		{
			print("case1");
			//input force forward / backward
			float input = MovementJoystick.GetAxis("Vertical");
			float input2 = MovementJoystick.GetAxis("Horizontal");

			if (input + input2 == 0)
			{
				input = Input.GetAxis("Vertical");
				input2 = Input.GetAxis("Horizontal");
			}
			inputForce = transform.up * speed * input + transform.right * speed * input2;
			rigidbody2D.AddForce(inputForce);
			//touch location based rotation
			var touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Quaternion rot = Quaternion.LookRotation(transform.position - touchPosition, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 200 * Time.deltaTime);
		}
		else if (numOfFing == 1 && MovementJoystick.GetComponent<CNJoystick>().touched)
		{
			print("case2");
			//input force on x/y axis regardless of entity rotation
			//float input = Input.GetAxis("Vertical");
			//float input2 = Input.GetAxis("Horizontal");
			float input = MovementJoystick.GetAxis("Vertical");

			float input2 = MovementJoystick.GetAxis("Horizontal");

			if (input + input2 == 0)
			{
				input = Input.GetAxis("Vertical");
				input2 = Input.GetAxis("Horizontal");
			}
			//print(input + " " + input2);
			inputForce = (new Vector3(0, 1, 0) * speed * input + new Vector3(1, 0, 0) * speed * input2);
			//print(inputForce.y + " " + inputForce.x);
			rigidbody2D.AddForce(inputForce);
			//movement based rotation
			if (inputForce != new Vector3(0, 0, 0))
			{
				Quaternion rot = Quaternion.AngleAxis(Mathf.Atan2(inputForce.y, inputForce.x) * (180 / Mathf.PI) + 270, Vector3.forward);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 200 * Time.deltaTime);
			}
		}
		else if (numOfFing == 1 && !MovementJoystick.GetComponent<CNJoystick>().touched)
		{
			print("case3");
			//touch location based rotation
			var touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Quaternion rot = Quaternion.LookRotation(transform.position - touchPosition, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 200 * Time.deltaTime);
		}






		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
		rigidbody2D.angularVelocity = 0;

		//transform.rotation



		//decrease speed to 0 when there is not input
		if (inputForce == new Vector3(0, 0, 0) && (rigidbody2D.velocity.x > stopForcePoint || rigidbody2D.velocity.x < -stopForcePoint) && (rigidbody2D.velocity.y > stopForcePoint || rigidbody2D.velocity.y < -stopForcePoint))
		{
			rigidbody2D.AddForce(rigidbody2D.velocity.normalized * -speed);
		}
		else if (inputForce == new Vector3(0, 0, 0) && (((rigidbody2D.velocity.x > stopForcePoint || rigidbody2D.velocity.x < -stopForcePoint) && (rigidbody2D.velocity.y > stopForcePoint || rigidbody2D.velocity.y < -stopForcePoint)) == false))
		{
			rigidbody2D.velocity = new Vector2(0, 0);
		}

		//transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, 180*Time.deltaTime);


		// 5 - Shooting

		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea

		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
			}
		}

	}
	*/


	//Mouse stuff
	int cooldown = 3;
	void FixedUpdate()
    {
        // 5 - Move the game object
        // Move the camera
		speed = bh.speed;
		Vector3 inputForce;
       // rigidbody2D.velocity = movement;
		if (true)
		{

			//input force forward / backward
			float input = Input.GetAxis("Vertical");
			float input2 = Input.GetAxis("Horizontal");
			inputForce = transform.up* speed * input + transform.right * speed * input2;
			rigidbody2D.AddForce(inputForce);
			//mouse location based movement
			var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 100 * Time.deltaTime);
		}
		/*
		else
		{
			//input force on x/y axis regardless of entity rotation
			//float input = Input.GetAxis("Vertical");
			//float input2 = Input.GetAxis("Horizontal");
			float input = MovementJoystick.GetAxis("Vertical");
			
			float input2 = MovementJoystick.GetAxis("Horizontal");
			
			if (input + input2 == 0)
			{
				input = Input.GetAxis("Vertical");
				input2 = Input.GetAxis("Horizontal");
			}
			//print(input + " " + input2);
			inputForce = (new Vector3(0, 1, 0) * speed * input + new Vector3(1, 0, 0) * speed * input2);
			//print(inputForce.y + " " + inputForce.x);
			rigidbody2D.AddForce(inputForce);
			//movement based rotation
			if (inputForce != new Vector3(0, 0,0))
			{
				Quaternion rot = Quaternion.AngleAxis(Mathf.Atan2(inputForce.y, inputForce.x) * (180 / Mathf.PI) + 270, Vector3.forward);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 200 * Time.deltaTime);
			}
		}
		*/
		
		



		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rigidbody2D.angularVelocity = 0;
		
		//transform.rotation



		//decrease speed to 0 when there is not input
		if (inputForce == new Vector3(0,0,0) && (rigidbody2D.velocity.x > stopForcePoint || rigidbody2D.velocity.x < -stopForcePoint) && (rigidbody2D.velocity.y > stopForcePoint || rigidbody2D.velocity.y < -stopForcePoint))
        {
            rigidbody2D.AddForce(rigidbody2D.velocity.normalized * -speed);
        }
		else if (inputForce == new Vector3(0, 0, 0) && (((rigidbody2D.velocity.x > stopForcePoint || rigidbody2D.velocity.x < -stopForcePoint) && (rigidbody2D.velocity.y > stopForcePoint || rigidbody2D.velocity.y < -stopForcePoint)) == false))
        {
            rigidbody2D.velocity = new Vector2(0, 0);
        }



        //transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, 180*Time.deltaTime);
        
        
        // 5 - Shooting
		
       // bool shoot = Input.GetButtonDown("Fire1");
        //shoot |= Input.GetButtonDown("Fire2");
        // Careful: For Mac users, ctrl + arrow is a bad idea

		
		//int starta = Environment.TickCount;
        
			if (cooldown == 0)
			{
				if (Input.GetMouseButton(0))
				{
					bh.fireWeapons();
					cooldown = 2;
				}
			}
			else
			{
				cooldown--;
			}
       
		//int enda = Environment.TickCount;
		//print(enda-starta);
    
    }
}