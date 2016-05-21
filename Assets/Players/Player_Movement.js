//#pragma strict
////Booleans
////var falling : boolean = true;
//var running : boolean = false;
//var moving : boolean = false;
//
////Speeds
//var FwalkSpeed : float = 1;
//var FRunSpeed : float = 2.5;
//var straifSpeed : float = 0.6;
//var backwardSpeed : float = 0.6;
//var jumpStrength : int = 8;
//
//
////var totalRotation : float = 0;
////
//////gameObjects
////var playerCamera : GameObject;
//
////Shortcuts
//private var AnimationScript : Player_Animations;
//
//function OnCollisionStay()
//{
//	falling = false;
//}
//
//function Start () 
//{
//	AnimationScript = this.GetComponent(Player_Animations);
//}
//
//function Update () 
//{
//	var playerRigid = this.transform.GetComponent.<Rigidbody>();
//	
//	//walking
//	if(running == false)
//	{
//		if(Input.GetKey("w"))
//		{
//			this.transform.Translate(Vector3.forward * FwalkSpeed);
//			AnimationScript.WalkingAnimation();
//		}
//	}
//	//running
//	else
//	{
//		if(Input.GetKey("w"))
//		{
//			this.transform.Translate(Vector3.forward * FRunSpeed);
//		}
//	}
//	
//	if(Input.GetKey("a"))
//	{
//	this.transform.Translate(Vector3.left * straifSpeed);
//	}
//	
//	if(Input.GetKey("d"))
//	{
//		this.transform.Translate(Vector3.right * straifSpeed);
//	}
//	
//	if(Input.GetKey("s"))
//	{
//		this.transform.Translate(Vector3.back * backwardSpeed);
//	}
//	
//	if(Input.GetKey("left shift"))
//	{
//		running = true;
//	}
//	else
//	{
//		running = false;
//	}
//	
//	if(Input.GetKey("space") && falling == false)
//	{
//		playerRigid.velocity.y = jumpStrength;
//	}
//
//	//if one of these are met then the player is moving
//	if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || falling == true)
//	{
//		moving = true;
//	}
//	else 
//	{
//		moving = false;
//	}
//	
//	//if not moving the the player is standing still so play the animation
//	if(moving == false)
//	{
//		AnimationScript.IdleAnimation ();
//	}
//	
//
////mouse movement----------------------------------------------
//	
//	//Cursor.lockState = CursorLockMode.Locked;
//	//Cursor.lockState = CursorLockMode.None;
//	//
//	//var h = Input.GetAxis("Mouse X");
//	//var v = Input.GetAxis("Mouse Y");
//	//
//	//if(totalRotation < 60 && -v > 0)
//	//{
//	//	playerCamera.transform.Rotate(-v,0,0);
//	//	totalRotation = totalRotation -v;
//	//}
//	//
//	//if(totalRotation > -60 && -v < 0)
//	//{
//	//	playerCamera.transform.Rotate(-v,0,0);
//	//	totalRotation = totalRotation -v;
//	//}
//	//
//	//this.transform.Rotate(0,h,0);
//	//
//	//falling = true;
//}
