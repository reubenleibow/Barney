#pragma strict

var Gender : int = 0;
var Target : GameObject;
var TargetRange : float = 40;
var SightAngle : int = 60;
var Alert : float = 0;
var ALERTCONSTANT : int = 2;
var SPOTTEDCONSTANT : int = 2;
var Spotted : float = 0;
var TargetAquired : boolean = false;
var SYSTEM : GameObject;
var Terra : float = 2;
var TERRACONSTANT : int = 2;
var RunDistance : int = 3;
var GotPlayPlace : boolean = false;
var ClosestPlayPlace : GameObject;
var ObjectLoopsScript : ObjectLoop;
var Health : float = 100;
var CarringBarney : boolean = false;
var BarneyCarried : GameObject;
var ArrivedAtPoint : boolean = true;
var Point : Vector3;

//Mom Stuff
var HasMom : boolean = false;
var RunToMom : boolean = false;
var ItsMom : GameObject;
var AccountedFor : boolean = false;

var State : String = "Spotted";

var SystemScript : SystemS;

var TargetType : String = "Nothing";

var Stop : boolean = false;

var Alive : boolean = true;

function Start () 
{
	SYSTEM = gameObject.Find("System");
	SystemScript = SYSTEM.GetComponent(SystemS);
	ObjectLoopsScript = SYSTEM.GetComponent(ObjectLoop);
	ObjectLoopsScript.ChildrenM.Add(this.gameObject);
}

function Update () 
{

	//if(Alive == false)
	//{
	//	return;
	//}

	var angle : float = Vector3.Angle(Target.transform.position - this.transform.position,this.transform.forward);
	var targetDistance : float = Vector3.Distance(this.transform.position,Target.transform.position);
	var hit : RaycastHit;
	var Xdif : int = Target.transform.position.x - this.transform.position.x;
	var Zdif : int = Target.transform.position.z - this.transform.position.z;
	
	
	
	var ray = Physics.Raycast(this.transform.position,Target.transform.position - this.transform.position,hit);
	
	//if player is in line of sight and if the angle/range is right and is big the add to terra/spotted (barney not being carried)
	//Prevent messing up "hit"
	if(CarringBarney == false && ray == true)
	{
		if(hit.transform.name == Target.name && targetDistance <= TargetRange && angle <= SightAngle)
		{
			Debug.DrawRay(this.transform.position,Target.transform.position - this.transform.position,Color.magenta);
			
			
			if(Spotted <= SPOTTEDCONSTANT)
			{
				Spotted = Spotted + 1*Time.deltaTime;
			}
			if(Spotted >= SPOTTEDCONSTANT)
			{
				Alert = ALERTCONSTANT;
				TargetAquired = true;
				
				//find path to the baby barney
				if(SYSTEM.GetComponent(SystemS).Big == false && Terra <= 0 && CarringBarney == false)
				{
					this.GetComponent.<NavMeshAgent>().SetDestination(Target.transform.position);
					TargetType = "SmallBarney";
				}
				if(SYSTEM.GetComponent(SystemS).Big == true)
				{
					TargetType = "BigBarney";
					Terra = TERRACONSTANT;
				}
			}
		}
	}
	
	//Run away because of terra
	if(Terra > 0)
	{
		this.GetComponent.<NavMeshAgent>().SetDestination(Vector3(Xdif *-RunDistance,this.transform.position.y,Zdif *-RunDistance));
		//Debug.Log("Tera");
	}
	
	
	//PickUpBarney
	if(TargetAquired == true)
	{
		var distance = Vector3.Distance(this.transform.position, Target.transform.position);
		
		if(Target.GetComponent(BarneyScript).BarneySize == false && Terra <= 0 && distance < 1)
		{
			Target.GetComponent(BarneyScript).carried = true;
			CarringBarney = true;
			BarneyCarried = Target;
			ArrivedAtPoint = true;
		}
	}
	// when have baby barney the run around the park
	if(CarringBarney == true)
	{
		BarneyCarried.transform.position = this.transform.position;
		
		if(Alert <= 0 && ArrivedAtPoint == true && RunToMom == false)
		{
			Point = Vector3(Random.Range(SystemScript.ParkXStart,SystemScript.ParkXEnd),0,Random.Range(SystemScript.ParkZStart,SystemScript.ParkZEnd));
			this.GetComponent.<NavMeshAgent>().SetDestination(Point);
			ArrivedAtPoint = false;
		}
		
		if(BarneyCarried.GetComponent(BarneyScript).BarneySize == true)
		{
			CarringBarney = false;
			
		}
		if((this.transform.position.x < Point.x + 2 && this.transform.position.x > Point.x - 2) &&(this.transform.position.z < Point.z + 2 && this.transform.position.z > Point.z - 2))
		{
			ArrivedAtPoint = true;
		}
	}
	
	//When not in sight: terra goes down first then alert

	//If it is in sight Or being carried
	var HitRayPosible : boolean = false; 
	if(ray == true)
	{
	if(CarringBarney == false && hit.transform.name != Target.name)
	{
		HitRayPosible = true;
	}
	if(CarringBarney == false && hit.transform.name == Target.name)
	{
		HitRayPosible = false;
	}
	}
	if(HitRayPosible == true || targetDistance > TargetRange || angle > SightAngle || CarringBarney == true)
	{
		if(Spotted > 0)
		{
			Spotted = Spotted - 1*Time.deltaTime;

		}
		
		if(Alert > 0 && Terra <= 0)
		{
			Alert = Alert - 1*Time.deltaTime;
		}
		if(Alert > 0)
		{
			State = "Spotted";
		}
		if(Terra > 0)
		{
			Terra = Terra - 1*Time.deltaTime;
			State = "Spotted";
		}
	}
	
	if(Spotted <= SPOTTEDCONSTANT)
	{
		TargetAquired = false;
	}
	
	if(Alert <= 0 && State == "Spotted")
	{
		ObjectLoopsScript.ChildrenP.Add(this.gameObject);
		State = "Nothing";
	}
	
	if(Alert > 0 && GotPlayPlace == true)
	{
		ResetPlayPlace();
		GotPlayPlace = false;
	}	
	
	if(Health <= 0)
	{
		Destroy(gameObject);
	}
	
	
	if(Stop == true)
	{
		this.GetComponent.<NavMeshAgent>().enabled = false;
	}
	else
	{
		this.GetComponent.<NavMeshAgent>().enabled = true;
	}
	
	//When the children are close enough, make mom avialable to move.
	var MomDist : float;
	
	if(RunToMom == true && ItsMom != null)
	{ 
		MomDist = Vector3.Distance(this.transform.position,ItsMom.transform.position);
	}
	//When it is called by there mother, annd now is adjacent from her........
	if(MomDist < 2 && RunToMom == true && AccountedFor == false)
	{
		AccountedFor = true;
		ItsMom.GetComponent(Bagie_Script).ChildrenGot = ItsMom.GetComponent(Bagie_Script).ChildrenGot +1;
		this.GetComponent.<NavMeshAgent>().enabled = false;
		this.GetComponent.<NavMeshAgent>().enabled = true;
	}
	
	// after children have see there mom, follow her.
	if(Alert <= 0 && AccountedFor == true && ItsMom != null && this.GetComponent.<NavMeshAgent>().enabled == true)
	{
		this.GetComponent.<NavMeshAgent>().SetDestination(ItsMom.transform.position);
	}


	if(Alive == false)
	{
		Debug.Log("LLLLLL");
		if(ItsMom != null)
		{
			ItsMom.GetComponent(Bagie_Script).KidDead(this.gameObject);
			ItsMom = null;
		}
	}
}



function ResetPlayPlace()
{
	ClosestPlayPlace.GetComponent(Objects).Taken = false;
}

function RunToMomNow(MomPos : Vector3)
{
	// keep finding path to mom.
	if(Alert <= 0 && Alive == true)
	{
		this.GetComponent.<NavMeshAgent>().SetDestination(MomPos);
		//Debug.Log(MomPos);
	}

}



function DeadMommy ()
{
	//ItsMom = null;
	ObjectLoopsScript.ChildrenM.Add(this.gameObject);
}



//Add variable that only work when a live.