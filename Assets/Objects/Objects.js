#pragma strict
var System : GameObject;
var Taken : boolean = false;
var Type : String = "SitPlace";

function Start () 
{
	if(Type == "SitPlace" || Type == "WorkPlace" )
	{
		System.GetComponent(ObjectLoop).Bench.Add(this.gameObject);
	}
	
	if(Type == "PlayPlace")
	{
		System.GetComponent(ObjectLoop).PlayPlace.Add(this.gameObject);

	}
}

function Update () 
{
	
}