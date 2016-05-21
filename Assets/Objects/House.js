#pragma strict
var ObjectLoopScript : ObjectLoop;
var MomSpaceTaken : boolean = false;
var ManSpaceTaken : boolean = false;
var MomObject : GameObject;
var ManObject : GameObject;


function Start () 
{
	ObjectLoopScript = gameObject.Find("System").GetComponent(ObjectLoop);
	ObjectLoopScript.Houses.Add(this.gameObject);
}

function Update () {

}