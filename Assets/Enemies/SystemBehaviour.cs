using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SystemBehaviour : MonoBehaviour
{
    public bool Big = true;

    public GameObject PlayerOneHealthText;
    public GameObject PlayerOneKillsText;
    public GameObject TimeText;
    public int WorldDiameter = 2000;
    //var SunConsttantRadius : int;
    //var DayTime : float = 0;
    //var SunY : float;
    //var InverseSun : boolean = false;
    public GameObject SunDay;
    public GameObject SunNight;
    public GameObject ObjectTerrain;
    public GameObject ParKPosition;

    public float GlobalTime = 90;
    public int SunSpeed = 20;

    public float WorldX;
    public float WorldZ;
    public float ParkXEnd;
    public float ParkZEnd;
    public float ParkXStart;
    public float ParkZStart;

    // Use this for initialization
    void Start()
    {
        //SunConsttantRadius = WorldDiameter/2;
        WorldX = ObjectTerrain.GetComponent<Terrain>().terrainData.size.x;
        WorldZ = ObjectTerrain.GetComponent<Terrain>().terrainData.size.z;

        ParkXEnd = ParKPosition.transform.position.x + 100;
        ParkZEnd = ParKPosition.transform.position.z + 100;
        ParkXStart = ParKPosition.transform.position.x - 100;
        ParkZStart = ParKPosition.transform.position.z - 100;
        GlobalTime = 90;
        SunSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {

        TimeText.GetComponent<Text>().text = "Time of day" + GlobalTime;
        GlobalTime = GlobalTime + SunSpeed * Time.deltaTime;

        if (GlobalTime > 360)
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
        SunDay.transform.RotateAround(Vector3.zero, Vector3.right, SunSpeed * Time.deltaTime);
        SunDay.transform.LookAt(Vector3.zero);
        SunNight.transform.RotateAround(Vector3.zero, Vector3.right, SunSpeed * Time.deltaTime);
        SunNight.transform.LookAt(Vector3.zero);
    }
}
