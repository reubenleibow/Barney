#pragma strict
var SystemObject : GameObject;
var Ordering : int;

function Start () 
{
	SystemObject = gameObject.Find("System");
	//SystemObject.GetComponent(ObjectLoop).Roads.Add(this.gameObject);
	SystemObject.GetComponent(ObjectLoop).RoadSorter.Add(this.gameObject);
}

function Update () 
{
	
}


// the roads aad to a list then that list is sorted out into another mlist that is used for the walkers.