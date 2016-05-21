#pragma strict
import System.Collections.Generic;

var Bench = new List.<GameObject>(); 
var Bagie = new List.<GameObject>();

var PlayPlace = new List.<GameObject>();
var ChildrenP = new List.<GameObject>();
var Moms = new List.<GameObject>();
var ChildrenM = new List.<GameObject>();
var Houses = new List.<GameObject>();
var NeedHouses = new List.<GameObject>();
var Roads = new List.<GameObject>();
var RoadSorter = new List.<GameObject>();
var Walkers = new List.<GameObject>();

var RoadOrderInList : int = 0;
var BagieWorkPlace = new List.<GameObject>();

var SystemScript : SystemS;	
			
function Start () 
{
	SystemScript = this.GetComponent(SystemS);
}

function Update () 
{
	var closestObjectX : GameObject;

	//When Children are created and moms are available, set there moms
	if(ChildrenM.Count > 0 && Moms.Count > 0)
	{
		for(ChildM in ChildrenM )
		{
			for(var parents in Moms)
			{
				if(parents.GetComponent(Bagie_Script).ChildrenNeeded > 0 && ChildM.GetComponent(Children).HasMom == false)
				{
					parents.GetComponent(Bagie_Script).ChildrenNeeded = parents.GetComponent(Bagie_Script).ChildrenNeeded - 1;
					parents.GetComponent(Bagie_Script).ChildrenList.Add(ChildM);
					ChildM.GetComponent(Children).HasMom = true;
					ChildM.GetComponent(Children).ItsMom = parents;
					//Must change this line or will imediatly call for children
					//parents.GetComponent(Bagie_Script).CallChildren = true;
				}
			}	
		}

	}
	Moms.Clear();



	if(Bagie.Count > 0)
	{
		for(men in Bagie )
		{
			var distance : float = 99999;
			
			//for bagies that have no job
			if(men.GetComponent(Bagie_Script).GotJob == false)
			{
				for(var values in Bench)
				{
					if(Vector3.Distance(men.transform.position,values.transform.position) < distance && values.GetComponent(Objects).Taken == false && values.GetComponent(Objects).Type == "SitPlace")
					{
						distance = Vector3.Distance(men.transform.position,values.transform.position);
						closestObjectX = values;
					}
				}
				men.GetComponent.<NavMeshAgent>().SetDestination(closestObjectX.transform.position);
				closestObjectX.GetComponent(Objects).Taken = true;
				men.GetComponent(Bagie_Script).closestObject = closestObjectX;
				men.GetComponent(Bagie_Script).GotBench = true;
				men.GetComponent(Bagie_Script).State = "GoToBench";
				
			}
			if(men.GetComponent(Bagie_Script).GotJob == true)
			{
				for(var values in Bench)
				{
					if(Vector3.Distance(men.transform.position,values.transform.position) < distance && values.GetComponent(Objects).Taken == false && values.GetComponent(Objects).Type == "WorkPlace")
					{
						distance = Vector3.Distance(men.transform.position,values.transform.position);
						closestObjectX = values;
					}
				}
				men.GetComponent.<NavMeshAgent>().SetDestination(closestObjectX.transform.position);
				closestObjectX.GetComponent(Objects).Taken = true;
				men.GetComponent(Bagie_Script).closestObject = closestObjectX;
				men.GetComponent(Bagie_Script).GotBench = true;
				men.GetComponent(Bagie_Script).State = "GoToWork";
			}
			
		}
	Bagie.Clear();
	}
	
	
	
	var closestObjectY : GameObject;

	if(ChildrenP.Count > 0)
	{
		for(men in ChildrenP)
		{
			var distanceY : float = 99999;
			
			//for bagies that have no job
			if(men.GetComponent(Children).GotPlayPlace == false)
			{
				for(var values in PlayPlace)
				{

					if(Vector3.Distance(men.transform.position,values.transform.position) < distanceY && values.GetComponent(Objects).Taken == false && values.GetComponent(Objects).Type == "PlayPlace")
					{
						distanceY = Vector3.Distance(men.transform.position,values.transform.position);
						closestObjectY = values;
					}
				
				}
				men.GetComponent.<NavMeshAgent>().SetDestination(closestObjectY.transform.position);

				closestObjectY.GetComponent(Objects).Taken = true;
				men.GetComponent(Children).ClosestPlayPlace = closestObjectY;
				men.GetComponent(Children).GotPlayPlace = true;
				men.GetComponent(Children).State = "GoToPlay";
			}
		}
	
		
	ChildrenP.Clear();
	}
	
	if(Houses.Count > 0 && NeedHouses.Count > 0)
	{
		for(var house in Houses)
		{
			var HouseScript = house.GetComponent(House);
		
			for(var People in NeedHouses)
			{
				var Adults = People.GetComponent(Bagie_Script);
		
				if(HouseScript.MomSpaceTaken == false && Adults.HasHouse == false && Adults.ManType == "Mom")
				{
					HouseScript.MomObject = People;
					Adults.ItsHouse = house;
					Adults.HasHouse = true;
					HouseScript.MomSpaceTaken = true;
				}
				
				if(HouseScript.ManSpaceTaken == false && Adults.HasHouse == false && Adults.ManType == "Man")
				{
					HouseScript.ManObject = People;
					Adults.ItsHouse = house;
					Adults.HasHouse = true;
					HouseScript.ManSpaceTaken = true;
				}
			
			}
		}
	NeedHouses.Clear();
	}
	var newRoadPointNumber :int;

	if(Walkers.Count > 0 && Roads.Count > 0)
	{
		for(var walker in Walkers)
		{

			if(walker.GetComponent(Bagie_Script).StartRoadPos == false)
			{
				newRoadPointNumber = walker.GetComponent(Bagie_Script).CurrentRoadPoint +1;
				
				if(newRoadPointNumber >= Roads.Count)
				{
					newRoadPointNumber = 0;
				}
				
				
				walker.GetComponent(Bagie_Script).RoadPoint = Roads[newRoadPointNumber];
				walker.GetComponent(Bagie_Script).CurrentRoadPoint = newRoadPointNumber;
				walker.GetComponent(Bagie_Script).State = "WalkSpotFound";
			}

			if(walker.GetComponent(Bagie_Script).StartRoadPos == true && Roads.Count > walker.GetComponent(Bagie_Script).StartRoadPosInt)
			{
				walker.GetComponent(Bagie_Script).RoadPoint = Roads[walker.GetComponent(Bagie_Script).StartRoadPosInt];
				walker.GetComponent(Bagie_Script).CurrentRoadPoint = walker.GetComponent(Bagie_Script).StartRoadPosInt;
				walker.GetComponent(Bagie_Script).State = "WalkSpotFound";
				Debug.Log("Setetetete" + walker.GetComponent(Bagie_Script).StartRoadPosInt);
			}
		}
	Walkers.Clear();
	}


	if(RoadSorter.Count > 0)
	{
		for(var randomRoads in RoadSorter)
		{
			if(randomRoads.GetComponent(RoadPointScript).Ordering == RoadOrderInList)
			{
				Roads.Add(randomRoads);
				RoadOrderInList = RoadOrderInList +1;
			}
		}
	}
	//RoadSorter.Clear();
}
