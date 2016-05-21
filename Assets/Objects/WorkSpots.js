#pragma strict
var Taken : boolean = false;
var System : GameObject;

function Start () 
{
	System.GetComponent(ObjectLoop).BagieWorkPlace.Add(this.gameObject);
}