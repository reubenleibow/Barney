using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectLoop : MonoBehaviour
{

    public List<GameObject> Bench = new List<GameObject>();
    public List<GameObject> Bagie = new List<GameObject>();
    public List<GameObject> PlayPlace = new List<GameObject>();
    public List<GameObject> ChildrenP = new List<GameObject>();
    public List<GameObject> Moms = new List<GameObject>();
    public List<GameObject> ChildrenM = new List<GameObject>();
    public List<GameObject> Houses = new List<GameObject>();
    public List<GameObject> NeedHouses = new List<GameObject>();
    public List<GameObject> Roads = new List<GameObject>();
    public List<GameObject> RoadSorter = new List<GameObject>();
    public List<GameObject> Walkers = new List<GameObject>();
    public int RoadOrderInList = 0;
    public List<GameObject> BagieWorkPlace = new List<GameObject>();
    public SystemS SystemScript;

    // Use this for initialization
    void Start()
    {
        SystemScript = this.GetComponent<SystemS>();
    }

    // Update is called once per frame
    void Update()
    {


        //When Children are created and moms are available, set there moms
        if (ChildrenM.Count > 0 && Moms.Count > 0)
        {
            foreach (var ChildM in ChildrenM)
            {
                foreach (var parents in Moms)
                {
                    if (parents.GetComponent<Bagie_Script>().ChildrenNeeded > 0 && ChildM.GetComponent<Children>().HasMom == false)
                    {
                        parents.GetComponent<Bagie_Script>().ChildrenNeeded = parents.GetComponent<Bagie_Script>().ChildrenNeeded - 1;
                        parents.GetComponent<Bagie_Script>().ChildrenList.Add(ChildM);
                        ChildM.GetComponent<Children>().HasMom = true;
                        ChildM.GetComponent<Children>().ItsMom = parents;
                        //Must change this line or will imediatly call for children
                        //parents.GetComponent<Bagie_Script>().CallChildren = true;
                    }
                }
            }

        }
        Moms.Clear();

        if (Bagie.Count > 0)
        {
            GameObject closestObjectX = null;
            foreach (var men in Bagie)
            {
                var distance = float.MaxValue;

                //for bagies that have no job
                if (men.GetComponent<Bagie_Script>().GotJob == false)
                {
                    foreach (var values in Bench)
                    {
                        if (Vector3.Distance(men.transform.position, values.transform.position) < distance && values.GetComponent<Objects>().Taken == false && values.GetComponent<Objects>().Type == "SitPlace")
                        {
                            distance = Vector3.Distance(men.transform.position, values.transform.position);
                            closestObjectX = values;
                        }
                    }
                    men.GetComponent<NavMeshAgent>().SetDestination(closestObjectX.transform.position);
                    closestObjectX.GetComponent<Objects>().Taken = true;
                    men.GetComponent<Bagie_Script>().closestObject = closestObjectX;
                    men.GetComponent<Bagie_Script>().GotBench = true;
                    men.GetComponent<Bagie_Script>().State = "GoToBench";
                }
                if (men.GetComponent<Bagie_Script>().GotJob == true)
                {
                    foreach (var values in Bench)
                    {
                        if (Vector3.Distance(men.transform.position, values.transform.position) < distance && values.GetComponent<Objects>().Taken == false && values.GetComponent<Objects>().Type == "WorkPlace")
                        {
                            distance = Vector3.Distance(men.transform.position, values.transform.position);
                            closestObjectX = values;
                        }
                    }
                    men.GetComponent<NavMeshAgent>().SetDestination(closestObjectX.transform.position);
                    closestObjectX.GetComponent<Objects>().Taken = true;
                    men.GetComponent<Bagie_Script>().closestObject = closestObjectX;
                    men.GetComponent<Bagie_Script>().GotBench = true;
                    men.GetComponent<Bagie_Script>().State = "GoToWork";
                }
            }
            Bagie.Clear();
        }

        if (ChildrenP.Count > 0)
        {
            GameObject closestObjectY = null;
            foreach (var men in ChildrenP)
            {
                var distanceY = float.MaxValue;

                //for bagies that have no job
                if (men.GetComponent<Children>().GotPlayPlace == false)
                {
                    foreach (var values in PlayPlace)
                    {

                        if (Vector3.Distance(men.transform.position, values.transform.position) < distanceY && values.GetComponent<Objects>().Taken == false && values.GetComponent<Objects>().Type == "PlayPlace")
                        {
                            distanceY = Vector3.Distance(men.transform.position, values.transform.position);
                            closestObjectY = values;
                        }

                    }
                    men.GetComponent<NavMeshAgent>().SetDestination(closestObjectY.transform.position);

                    closestObjectY.GetComponent<Objects>().Taken = true;
                    men.GetComponent<Children>().ClosestPlayPlace = closestObjectY;
                    men.GetComponent<Children>().GotPlayPlace = true;
                    men.GetComponent<Children>().State = "GoToPlay";
                }
            }


            ChildrenP.Clear();
        }

        if (Houses.Count > 0 && NeedHouses.Count > 0)
        {
            foreach (var house in Houses)
            {
                var HouseScript = house.GetComponent<House>();

                foreach (var People in NeedHouses)
                {
                    var Adults = People.GetComponent<Bagie_Script>();

                    if (HouseScript.MomSpaceTaken == false && Adults.HasHouse == false && Adults.ManType == "Mom")
                    {
                        HouseScript.MomObject = People;
                        Adults.ItsHouse = house;
                        Adults.HasHouse = true;
                        HouseScript.MomSpaceTaken = true;
                    }

                    if (HouseScript.ManSpaceTaken == false && Adults.HasHouse == false && Adults.ManType == "Man")
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
        var newRoadPointNumber = 0;

        if (Walkers.Count > 0 && Roads.Count > 0)
        {
            foreach (var walker in Walkers)
            {

                if (walker.GetComponent<Bagie_Script>().StartRoadPos == false)
                {
                    newRoadPointNumber = walker.GetComponent<Bagie_Script>().CurrentRoadPoint + 1;

                    if (newRoadPointNumber >= Roads.Count)
                    {
                        newRoadPointNumber = 0;
                    }


                    walker.GetComponent<Bagie_Script>().RoadPoint = Roads[newRoadPointNumber];
                    walker.GetComponent<Bagie_Script>().CurrentRoadPoint = newRoadPointNumber;
                    walker.GetComponent<Bagie_Script>().State = "WalkSpotFound";
                }

                if (walker.GetComponent<Bagie_Script>().StartRoadPos == true && Roads.Count > walker.GetComponent<Bagie_Script>().StartRoadPosInt)
                {
                    walker.GetComponent<Bagie_Script>().RoadPoint = Roads[walker.GetComponent<Bagie_Script>().StartRoadPosInt];
                    walker.GetComponent<Bagie_Script>().CurrentRoadPoint = walker.GetComponent<Bagie_Script>().StartRoadPosInt;
                    walker.GetComponent<Bagie_Script>().State = "WalkSpotFound";
                    Debug.Log("Setetetete" + walker.GetComponent<Bagie_Script>().StartRoadPosInt);
                }
            }
            Walkers.Clear();
        }


        if (RoadSorter.Count > 0)
        {
            foreach (var randomRoads in RoadSorter)
            {
                if (randomRoads.GetComponent<RoadPointScript>().Ordering == RoadOrderInList)
                {
                    Roads.Add(randomRoads);
                    RoadOrderInList = RoadOrderInList + 1;
                }
            }
        }
        //RoadSorter.Clear();
    }
}
