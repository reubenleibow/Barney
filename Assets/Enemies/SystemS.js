#pragma strict
var Big : boolean = true;

var PlayerOneHealthText : GameObject;
var PlayerOneKillsText : GameObject;
var TimeText : GameObject;
var WorldDiameter : int = 2000;
//var SunConsttantRadius : int;
//var DayTime : float = 0;
//var SunY : float;
//var InverseSun : boolean = false;
var SunDay : GameObject;
var SunNight : GameObject;
var ObjectTerrain : GameObject;
var ParKPosition : GameObject;

var GlobalTime : float = 90;
var SunSpeed : int = 20;

var WorldX : int;
var WorldZ : int;
var ParkXEnd : int;
var ParkZEnd : int;
var ParkXStart : int;
var ParkZStart : int;



function Start () 
{
	//SunConsttantRadius = WorldDiameter/2;
	WorldX = ObjectTerrain.GetComponent.<Terrain>().terrainData.size.x;
	WorldZ = ObjectTerrain.GetComponent.<Terrain>().terrainData.size.z;
	
	ParkXEnd = ParKPosition.transform.position.x + 100;
	ParkZEnd = ParKPosition.transform.position.z + 100;
	ParkXStart = ParKPosition.transform.position.x - 100;
	ParkZStart = ParKPosition.transform.position.z - 100;
	GlobalTime = 90;
	SunSpeed = 1;
}

function Update () 
{
	TimeText.GetComponent(UI.Text).text = "Time of day" + GlobalTime;
	GlobalTime = GlobalTime + SunSpeed * Time.deltaTime;
	
	if(GlobalTime > 360)
	{
		GlobalTime = 0;
	}
	
	//if(GlobalTime > )
	
	//if(DayTime > WorldDiameter)
	//{
	//	InverseSun = true;
	//}
	
	//if(DayTime < 0)
	//{
	//	InverseSun = false;
	//}
	
	
	//if(InverseSun == false)
	//{
	//	DayTime = DayTime + 1*Time.deltaTime;
		//SunY = SunConsttantRadius * Mathf.Sin(Mathf.Acos((SunConsttantRadius -DayTime) / SunConsttantRadius));
	//}
	
	//if(InverseSun == true)
	//{
	//	DayTime = DayTime - 1*Time.deltaTime;
		//SunY = -(SunConsttantRadius * Mathf.Sin(Mathf.Acos((SunConsttantRadius -DayTime) / SunConsttantRadius)));
	//}
	
	
	//Sun.transform.position.x = DayTime - SunXOffset;
	//Sun.transform.position.y = SunY;
	SunDay.transform.RotateAround(Vector3.zero,Vector3.right, SunSpeed * Time.deltaTime);
	SunDay.transform.LookAt(Vector3.zero);
	SunNight.transform.RotateAround(Vector3.zero,Vector3.right, SunSpeed * Time.deltaTime);
	SunNight.transform.LookAt(Vector3.zero);
}