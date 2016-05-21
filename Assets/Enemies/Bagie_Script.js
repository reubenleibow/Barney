#pragma strict

var NightOrDay : String = "Day";
var WeaponInHand : GameObject;
var State : String = "Nothing";
var Speed : float = 1.0;
var TargetAcquired : boolean = false;
var Target : GameObject;
var SightRange : int = 40;
private var SPOTDELAY : int = 3;
var SpotMeter : float = 0;
var Spotted : boolean = false;
private var Randomx : float = 0;
var Alert : float = 0;
private var ALERTCONSTANT : int = 2;
var closestObject : GameObject;
private var ObjectLoopsScript : ObjectLoop;
var GotBench : boolean = false;
var GotJob : boolean = true;
var Health : float = 100;
var Stop : boolean = false;
var LastHit : GameObject;
var AtSpot : boolean = false;
//ForMoms
var pickupchildren : boolean;
var ParkPosition : GameObject;
var CallChildren : boolean = false;
var ChildrenGot : int = 0;

var SystemScript : SystemS;
var ArrivedPoint : Vector3;
var ChildrenNeeded : int = 2;
var ChildrenList = new List.<GameObject>();
var DistanceFromPark : Vector3;
//is player in
var RoadPoint : GameObject;
var HasHouse : boolean = false;
var ItsHouse : GameObject;

private var AttackRange : float = 10;
var AttackDamage : int = 10;

var CurrentRoadPoint : int = 0;

var ManType : String = "Man";

var SYSTEM : GameObject;

var StartRoadPos : boolean = true;
var StartRoadPosInt : int = 0;

var Animations: boolean = false;
var TimeToPickChildren : int = 0;

var Alive : boolean = true;

function Start () 
{
	//if(Alive == false)
	//{
	//	return;
	//}

	TimeToPickChildren  = 90;
	
	SpotMeter = 3;
	
	Randomx = Random.Range(0,2);
	ObjectLoopsScript = SYSTEM.GetComponent(ObjectLoop);
	SystemScript = SYSTEM.GetComponent(SystemS);
	
	if(ManType == "Mom" || ManType == "Man")
	{
		SYSTEM.GetComponent(ObjectLoop).NeedHouses.Add(this.gameObject);
	}
	
	if(ManType == "Man")
	{
		GotJob = true;
	}
	
	if(ManType == "Bagie")
	{
		GotJob = false;
	}
	
	if(ManType == "Mom")
	{
		GotJob = true;
		ObjectLoopsScript.Moms.Add(this.gameObject);
	}
	
	Stop = false;

	if(Animations == true)
	{
		this.GetComponent.<Animation>().Play("MWalking0");
		this.GetComponent.<Animation>()["MWalking0"].speed = 3;
	}

	//if(ManType == "Walker")
	//{
	//	this.GetComponent(Bagie_Script).CurrentRoadPoint = StartRoadPosInt;
	//	this.GetComponent(Bagie_Script).State = "FindingWalkSpot";
	//	this.GetComponent.<NavMeshAgent>().SetDestination(RoadPoint.transform.position);
	//	//ArrivedPoint = RoadPoint.transform.position;
	//}
}

function Update () 
{
	
	var hit : RaycastHit;
	var angle : float = Vector3.Angle(Target.transform.position - this.transform.position,this.transform.forward);
	var targetDistance = Vector3.Distance(Target.transform.position,this.transform.position);
	var path : NavMeshPath;

	if(ItsHouse != null)
	{
		var distanceFromHouse = Vector3.Distance(ItsHouse.transform.position,this.transform.position);
	}

	
	//ForMom
	if(ManType == "Mom")
	{
		var DistanceFromPark = Vector3.Distance(this.transform.position,ParkPosition.transform.position);

		if(ItsHouse != null)
		{
			if(ChildrenGot == ChildrenList.Count && State == "GoToHouse")
			{
				//got to her house(origin point must be low or there can't find the piont to travel to(no error given)).
				this.GetComponent.<NavMeshAgent>().enabled = true;
				this.GetComponent.<NavMeshAgent>().SetDestination(ItsHouse.transform.position);
				//Debug.Log("Goto house");
			}
			
			if(State == "GoToHouse" && distanceFromHouse <= 2)
			{
				State = "Nothing";
			}
		}
	}
	
	//Nocturnal or not Nocturnal
	if(Randomx == 1)
	{
		NightOrDay = "Night";
	}
	else
	{
		NightOrDay = "Day";
	}
	
	var ray = Physics.Raycast(this.transform.position,Target.transform.position - this.transform.position,hit);
	//var ObjectHit = hit.transform.gameObject;
	//when target is aquired
	if(ray == true)	
	{
	if(hit.transform.name == Target.name && targetDistance <= SightRange)
	{
		if(angle < 50)
		{
			Debug.DrawRay(this.transform.position,Target.transform.position - this.transform.position,Color.green);
			
			if(SpotMeter < SPOTDELAY)
			{
				SpotMeter = SpotMeter + 1 * Time.deltaTime;
				Spotted = true;
			}
			
			if(SpotMeter >= SPOTDELAY)
			{
				TargetAcquired = true;
				Alert = ALERTCONSTANT;
				State = "Spotted";
			}
		}
	}
	}
	//when target is NOT aquired
	if(ray == true)
	{
	if(hit.transform.name != Target.name || angle > 50 || targetDistance > SightRange)
	{
		TargetAcquired = false;
		Debug.DrawRay(this.transform.position,Target.transform.position - this.transform.position,Color.red);
		Spotted = false;
	}
	}
	//when spotted counter is 0 then start subtracting alert counter, only once the alert counter is 0 then return to natual state.
	if(Spotted == false)
	{
		if(SpotMeter > 0)
		{
			SpotMeter = SpotMeter - 1 * Time.deltaTime;
		}
		if(SpotMeter <= 0 && Alert > 0)
		{
			Alert = Alert - 1 * Time.deltaTime;
		}
	}
	//once all spotting things are in order....
	if(TargetAcquired == true)
	{
		if(Vector3.Distance(Target.transform.position,this.transform.position) > AttackRange && Alive == true)
		{
			this.GetComponent.<NavMeshAgent>().SetDestination(Target.transform.position);
		}
		//If Man Prevention<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		if(Vector3.Distance(Target.transform.position,this.transform.position) <= AttackRange)
		{
			this.GetComponent.<NavMeshAgent>().enabled = false;
			this.GetComponent.<NavMeshAgent>().enabled = true;
		}
		
		//If barney is in range of attack
		if(Vector3.Distance(Target.transform.position,this.transform.position) <= AttackRange)
		{
			//attack
			Target.GetComponent(BarneyScript).BarneyHealth = Target.GetComponent(BarneyScript).BarneyHealth - (AttackDamage * Time.deltaTime);
			Target.GetComponent(BarneyScript).HealthLost();
		}
	}
	var distance : float = 9999;
	
	if(Alert <= 0 && State == "Spotted")
	{
		State = "Nothing";
	}
	
	// Reset Point
	if(Alert > 0 && GotBench == true)
	{
		ResetBench();
		GotBench = false;
	}	
	
	if(State == "LookForChildren" && GotBench == true)
	{
		ResetBench();
		GotBench = false;
	}	
	
	//If not in alert and, nothing is pending in state then......
	if(Alert <= 0 && State == "Nothing" && ManType != "Walker")
	{
		ResetBench();
		ObjectLoopsScript.Bagie.Add(this.gameObject);
		State = "FindingWorkSpot";
	}
	//If this man is a walker and has not spotted anything the continue walking and start pos has been found
	//newRoadWork
	if(Alert <= 0 && State == "Nothing" && ManType == "Walker" )//&& StartRoadPos == false)
	{
		ObjectLoopsScript.Walkers.Add(this.gameObject);
		//State = "FindingWalkSpot";
		//Debug.Log("Walking called");
	}
	if(Alert <= 0 && State == "WalkSpotFound" && ManType == "Walker" && Alive == true)
	{
		this.GetComponent.<NavMeshAgent>().SetDestination(RoadPoint.transform.position);
		ArrivedPoint = RoadPoint.transform.position;
	}
	//ForMom
	//Time Setting-----------------------------
	if(ManType == "Mom" && SystemScript.GlobalTime > TimeToPickChildren && State != "LookForChildren" && ChildrenGot < ChildrenList.Count && Alert <= 0)
	{
		State = "LookForChildren";
		this.GetComponent.<NavMeshAgent>().SetDestination(ParkPosition.transform.position);
	}
	
	if(ManType == "Mom" && SystemScript.GlobalTime > 0 && SystemScript.GlobalTime < TimeToPickChildren && State == "GoToHouse" && Alert <= 0)
	{
		State = "Nothing";
	}
	//140
	//when bagie arrives at the bench
	if(State == "GoToBench" || State == "GoToWork")
	{
		
		if(AtSpot == true)
		{
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, closestObject.transform.rotation, 5);
		}
		
		if(this.GetComponent.<NavMeshAgent>().remainingDistance < 1)
		{	
			AtSpot = true;
		}
		else
		{
			AtSpot = false;
		}
		
		if(this.transform.rotation == closestObject.transform.rotation)
		{
			State = "PlaySitting";
		}
	}
	
	//When the walker arrives at the spot then find a new one.
	if(State == "WalkSpotFound")
	{
		if((this.transform.position.x < ArrivedPoint.x + 2 && this.transform.position.x > ArrivedPoint.x - 2) &&(this.transform.position.z < ArrivedPoint.z + 2 && this.transform.position.z > ArrivedPoint.z - 2))
		{
			State = "Nothing";

			if(StartRoadPos == true)
			{
				StartRoadPos = false;
			}
		}
	}
	
	if(Health <= 0)
	{
		LastHit.GetComponent(BarneyScript).Kills = LastHit.GetComponent(BarneyScript).Kills + 1;
		LastHit.GetComponent(BarneyScript).AddKill();
		Destroy(gameObject);
	}
	
	if(Stop == true)
	{
		this.GetComponent.<NavMeshAgent>().enabled = false;
		//Debug.Log("stopCalled");
	}
	
	if(Stop == false && this.GetComponent.<NavMeshAgent>().enabled == false)
	{
		this.GetComponent.<NavMeshAgent>().enabled = true;
	}
	//ForMom
	//When the mom is the right distance away from the park. 
	if(DistanceFromPark < 25 && State == "LookForChildren" && ChildrenGot < ChildrenList.Count)
	{
		CallChildren = true;
		Stop = true;
	}
	if(SystemScript.GlobalTime > TimeToPickChildren && ChildrenGot == ChildrenList.Count && Alert <= 0 && ManType == "Mom")
	{
		State = "GoToHouse";
		Stop = false;
	}
	//ForMom
	//call for children is the command
	//When calling fo children,the mom sets each childs runtomom counter, and call the function
	if(CallChildren == true && ChildrenGot < ChildrenList.Count)
	{
		for(var MyChildren in ChildrenList)
		{
			MyChildren.GetComponent(Children).RunToMom = true;
			MyChildren.GetComponent(Children).RunToMomNow(this.transform.position);
		}
		
		CallChildren = false;
	}
	
	if(State == "Nothing" && Stop == true)
	{
		Stop = false;

	}

}


function ResetBench()
{
	if(closestObject != null)
	{
		closestObject.GetComponent(Objects).Taken = false;
	}
	
}

function DeadMom()
{
	if(ManType == "Mom" && ChildrenList.Count >0)
	{
		for(var Kids in ChildrenList)
		{
			Kids.GetComponent(Children).DeadMommy();
		}

	}
}



function KidDead(Kid : GameObject)
{
	for(var Kids in ChildrenList)
	{
		if(Kids == Kid)
		{
			ChildrenList.Remove(Kid);
		}
	}
}