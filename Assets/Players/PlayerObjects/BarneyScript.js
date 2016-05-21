#pragma strict
var moving : boolean = false;
var movingState : String = "Idle";
var falling : boolean = true;
var SystemScript : SystemS;
var ObjectLoopS : ObjectLoop;
var BarneyHealth : float = 100;
var AttackRange : float = 10;
var AttackDamage : float = 10;
var Kills : int = 0;
var totalRotation : float = 0;
var PlayingAttackAnimation : boolean = false;
var BarneySize : boolean = true;
var carried : boolean = false;
//gameObjects
var playerCamera : GameObject;
var MainSystemS : GameObject;
var ActionCamera : GameObject;
var Kill : GameObject;

//Playervariables
var FSpeed : float = 0.5;
var BSpeed : float = 0.2;

var timeStart : boolean = false;
var aTime : float = 0;
var timeConstant : float = 1.5; 


function Start () 
{
	SystemScript = MainSystemS.GetComponent(SystemS);
	ObjectLoopS = MainSystemS.GetComponent(ObjectLoop);
	HealthLost();
	AddKill();
}

function Update () 
{

	var hit : RaycastHit;



	//Setting size of barney------------------------
	if(SystemScript.Big == true)
	{
		this.transform.localScale = Vector3(1,1,1);
	}
	else
	{
		this.transform.localScale = Vector3(0.1,0.1,0.1);
	}
	
	
	if(Input.GetKey("q"))
	{
		SystemScript.Big = true;
		BarneySize = true;
	}
	
	if(Input.GetKey("e"))
	{
		SystemScript.Big = false;
		BarneySize = false;
	}
	
	
	//Setting size of barney------------------------
	//Death of barney ---------------------------------------
	
	if(BarneyHealth  <= 0)
	{
		Destroy(this.gameObject);
	}
	
	//Death of barney ---------------------------------------
	
	if(Input.GetKeyDown("r"))
	{
		var ray = Physics.Raycast(this.transform.position,this.transform.forward,hit,AttackRange);
	}
	
	if(ray == true)
	{
	
	var chosenMan : GameObject;
	var angle : float;
	
		if(hit.transform.tag == "Man")
		{
			chosenMan = hit.collider.gameObject;
			angle = Vector3.Angle(this.transform.position - chosenMan.transform.position,chosenMan.transform.forward);
			Debug.Log("chosenMan"+chosenMan);
			chosenMan.GetComponent(Bagie_Script).Alive = false;
			Debug.Log("Set Tag MAN");

			if(chosenMan.GetComponent(Bagie_Script).ManType == "Mom" && chosenMan.GetComponent(Bagie_Script).ChildrenList.Count > 0)
			{
				chosenMan.GetComponent(Bagie_Script).DeadMom();
			}


			
			if(angle > 120)
			{
				Debug.Log("Reached");
				this.GetComponent.<Animation>().Play("AttackAnimation0");
				PlayingAttackAnimation = true;
				chosenMan.GetComponent(Bagie_Script).Stop = true;
				Debug.Log(Kill);
				Kill = chosenMan;

				//ObjectLoopS.Bagie.Remove(Kill);
			}
			else
			{
				chosenMan.GetComponent(Bagie_Script).Health = chosenMan.GetComponent(Bagie_Script).Health - AttackDamage;
				chosenMan.GetComponent(Bagie_Script).LastHit = this.gameObject;
			}
		}
		
		if(hit.transform.tag == "Child")
		{
			chosenMan = hit.collider.gameObject;
			angle = Vector3.Angle(this.transform.position - chosenMan.transform.position,chosenMan.transform.forward);
			Debug.Log(angle);
			chosenMan.GetComponent(Children).Alive = false;
			//Debug.Log("Set Tag");
			
			if(angle > 120)
			{
				this.GetComponent.<Animation>().Play("AttackAnimation0");
				PlayingAttackAnimation = true;
				chosenMan.GetComponent(Children).Stop = true;
				Kill = chosenMan;
			}
			else
			{
				chosenMan.GetComponent(Children).Health = chosenMan.GetComponent(Children).Health - AttackDamage;
			}
		}
	}

	if(Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("a"))
	{
		moving = true;
	}
	else
	{
		moving = false;
	}
if(PlayingAttackAnimation == false)
{	
	if(Input.GetKey("w"))
	{
		movingState = "forward";
		
		this.transform.Translate(Vector3.forward * FSpeed);
	}
	
	if(Input.GetKey("s"))
	{
		movingState = "backward";
		
		this.transform.Translate(Vector3.back * BSpeed);
	}
	
	if(Input.GetKey("a"))
	{
		movingState = "left";
		
		this.transform.Translate(Vector3.left * BSpeed);
	}
	
	if(Input.GetKey("d"))
	{
		movingState = "right";
		
		this.transform.Translate(Vector3.right * BSpeed);
	}
}
	
if(PlayingAttackAnimation == false)
{
	if(moving == true)
	{
		if(Input.GetKey("w"))
		{
			this.GetComponent.<Animation>().Play("Walking0");
			this.GetComponent.<Animation>()["Walking0"].speed = 2;
		}
		
		if(Input.GetKey("s"))
		{
			this.GetComponent.<Animation>().Play("Walking1");
			this.GetComponent.<Animation>()["Walking1"].speed = 1.2;
			
		}
		
		//if(movingState == "right")
		//{
		//	if(!Input.GetKey("w") && !Input.GetKey("s"))
		//	{
		//		this.GetComponent.<Animation>().Play("Walking2");
		//		this.GetComponent.<Animation>()["Walking2"].speed = 2;
		//		
		//	}
		//	else
		//	{
		//		this.transform.Translate(Vector3.right * BSpeed);
		//	}
		//	
		//	if(this.GetComponent.<Animation>()["Walking2"].time > 1)
		//	{
		//		this.transform.Translate(Vector3.right * BSpeed);
		//	}
		//	
		//}
		//
		//if(movingState == "left")
		//{
		//	if(!Input.GetKey("w") && !Input.GetKey("s"))
		//	{
		//		this.GetComponent.<Animation>().Play("Walking3");
		//		this.GetComponent.<Animation>()["Walking3"].speed = 2;
		//	}
		//	else
		//	{
		//		this.transform.Translate(Vector3.left * BSpeed);
		//	}
		//	
		//	if(this.GetComponent.<Animation>()["Walking3"].time > 1)
		//	{
		//		this.transform.Translate(Vector3.left * BSpeed);
		//	}
		//	
		//}
	}
	else
	{
		this.GetComponent.<Animation>().Play("Idle0");
	}
}

if(PlayingAttackAnimation == true)	
{
	ActionCamera.GetComponent.<Camera>().enabled = true;
	playerCamera.GetComponent.<Camera>().enabled = false;
}
else
{
	ActionCamera.GetComponent.<Camera>().enabled = false;
	playerCamera.GetComponent.<Camera>().enabled = true;
}

if(PlayingAttackAnimation == true && timeStart == false)
{
	timeStart = true;
	this.transform.position = Kill.GetComponent(AIShare).BehindePos.transform.position;
}
if(aTime <= timeConstant && timeStart == true)
{
	aTime = aTime + 1 * Time.deltaTime;
}
if(aTime >= timeConstant)
{
	Destroy(Kill);
	Kills ++;
	AddKill();
	Debug.Log("destroy"+Kill);
	timeStart = false;
	aTime = 0;
	PlayingAttackAnimation = false;

}

//mouse movement----------------------------------------------
if(PlayingAttackAnimation == false)
{
	
	Cursor.lockState = CursorLockMode.Locked;
	Cursor.lockState = CursorLockMode.None;
	
	var h = Input.GetAxis("Mouse X");
	var v = Input.GetAxis("Mouse Y");
	
	if(totalRotation < 60 && -v > 0)
	{
		playerCamera.transform.Rotate(-v,0,0);
		totalRotation = totalRotation -v;
	}
	
	if(totalRotation > -60 && -v < 0)
	{
		playerCamera.transform.Rotate(-v,0,0);
		totalRotation = totalRotation -v;
	}
	
	this.transform.Rotate(0,h,0);
}
	//falling = true;


}

function HealthLost()
{
	SystemScript.PlayerOneHealthText.GetComponent(UI.Text).text = "Health" + BarneyHealth;
}

function AddKill()
{
	SystemScript.PlayerOneKillsText.GetComponent(UI.Text).text = "Kills: " + Kills;
}
