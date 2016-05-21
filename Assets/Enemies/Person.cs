using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : PersonBase
{
    public GameObject WeaponInHand;
    public float Speed = 1.0f;
    public int SightRange = 40;
    private int SPOTDELAY = 3;
    public float SpotMeter = 0;
    public bool Spotted = false;
    private float Randomx = 0;
    public GameObject closestObject;
    public bool GotBench = false;
    public bool GotJob = true;

    public GameObject LastHit;
    public bool AtSpot = false;

    // ForMoms
    public bool pickupchildren;
    public GameObject ParkPosition;
    public bool CallChildren = false;
    public int ChildrenGot = 0;

    public Vector3 ArrivedPoint;
    public int ChildrenNeeded = 2;
    public List<GameObject> ChildrenList = new List<GameObject>();

    //is player in
    public GameObject RoadPoint;

    private float AttackRange = 10;
    public int AttackDamage = 10;

    public int CurrentRoadPoint = 0;

    public PersonType ManType = PersonType.Man;

    public bool StartRoadPos = true;
    public int StartRoadPosInt = 0;

    public bool Animations = false;
    public int TimeToPickChildren = 90;

    public bool Alive = true;


    // Use this for initialization
    public override void Start()
    {
        base.Start();

        TimeToPickChildren = 90;

        SpotMeter = 3;

        Randomx = Random.Range(0, 2);

        if (ManType == PersonType.Mom || ManType == PersonType.Man)
        {
            ObjectLoopsScript.NeedHouses.Add(this.gameObject);
        }

        if (ManType == PersonType.Man)
        {
            GotJob = true;
        }

        if (ManType == PersonType.Bagie)
        {
            GotJob = false;
        }

        if (ManType == PersonType.Mom)
        {
            GotJob = true;
            ObjectLoopsScript.Moms.Add(this.gameObject);
        }

        Stop = false;

        if (Animations == true)
        {
            this.GetComponent<Animation>().Play("MWalking0");
            this.GetComponent<Animation>()["MWalking0"].speed = 3;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Angle(Target.transform.position - this.transform.position, this.transform.forward);
        float targetDistance = Vector3.Distance(Target.transform.position, this.transform.position);
        var distanceFromHouse = 0.0f;
        var DistanceFromPark = 0.0f;

        if (ItsHouse != null)
        {
            distanceFromHouse = Vector3.Distance(ItsHouse.transform.position, this.transform.position);
        }

        //ForMom
        if (ManType == PersonType.Mom)
        {
            DistanceFromPark = Vector3.Distance(this.transform.position, ParkPosition.transform.position);

            if (ItsHouse != null)
            {
                if (ChildrenGot == ChildrenList.Count && State == PersonState.GoToHouse)
                {
                    //got to her house(origin point must be low or there can't find the piont to travel to(no error given)).
                    this.GetComponent<NavMeshAgent>().enabled = true;
                    this.GetComponent<NavMeshAgent>().SetDestination(ItsHouse.transform.position);
                    //Debug.Log("Goto house");
                }

                if (State == PersonState.GoToHouse && distanceFromHouse <= 2)
                {
                    State = PersonState.Nothing;
                }
            }
        }

        //Nocturnal or not Nocturnal
        Nocturnal = Randomx == 1;

        RaycastHit hit;
        var ray = Physics.Raycast(this.transform.position, Target.transform.position - this.transform.position, out hit);
        //var ObjectHit = hit.transform.gameObject;
        //when target is acquired
        if (ray == true)
        {
            if (hit.transform.name == Target.name && targetDistance <= SightRange)
            {
                if (angle < 50)
                {
                    Debug.DrawRay(this.transform.position, Target.transform.position - this.transform.position, Color.green);

                    if (SpotMeter < SPOTDELAY)
                    {
                        SpotMeter = SpotMeter + 1 * Time.deltaTime;
                        Spotted = true;
                    }

                    if (SpotMeter >= SPOTDELAY)
                    {
                        TargetAquired = true;
                        Alert = ALERTCONSTANT;
                        State = PersonState.SpottedPlayer;
                    }
                }
            }
        }
        //when target is NOT aquired
        if (ray == true)
        {
            if (hit.transform.name != Target.name || angle > 50 || targetDistance > SightRange)
            {
                TargetAquired = false;
                Debug.DrawRay(this.transform.position, Target.transform.position - this.transform.position, Color.red);
                Spotted = false;
            }
        }
        //when spotted counter is 0 then start subtracting alert counter, only once the alert counter is 0 then return to natual state.
        if (Spotted == false)
        {
            if (SpotMeter > 0)
            {
                SpotMeter = SpotMeter - 1 * Time.deltaTime;
            }
            if (SpotMeter <= 0 && Alert > 0)
            {
                Alert = Alert - 1 * Time.deltaTime;
            }
        }
        //once all spotting things are in order....
        if (TargetAquired == true)
        {
            if (Vector3.Distance(Target.transform.position, this.transform.position) > AttackRange && Alive == true)
            {
                this.GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
            }
            //If Man Prevention<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            if (Vector3.Distance(Target.transform.position, this.transform.position) <= AttackRange)
            {
                this.GetComponent<NavMeshAgent>().enabled = false;
                this.GetComponent<NavMeshAgent>().enabled = true;
            }

            //If barney is in range of attack
            if (Vector3.Distance(Target.transform.position, this.transform.position) <= AttackRange)
            {
                //attack
                Target.GetComponent<BarneyScript>().BarneyHealth = Target.GetComponent<BarneyScript>().BarneyHealth - (AttackDamage * Time.deltaTime);
                Target.GetComponent<BarneyScript>().HealthLost();
            }
        }
        if (Alert <= 0 && State == PersonState.SpottedPlayer)
        {
            State = PersonState.Nothing;
        }

        // Reset Point
        if (Alert > 0 && GotBench == true)
        {
            ResetBench();
            GotBench = false;
        }

        if (State == PersonState.LookForChildren && GotBench == true)
        {
            ResetBench();
            GotBench = false;
        }

        //If not in alert and, nothing is pending in state then......
        if (Alert <= 0 && State == PersonState.Nothing && ManType != PersonType.Walker)
        {
            ResetBench();
            ObjectLoopsScript.Bagie.Add(this.gameObject);
            State = PersonState.FindingWorkSpot;
        }
        //If this man is a walker and has not spotted anything the continue walking and start pos has been found
        //newRoadWork
        if (Alert <= 0 && State == PersonState.Nothing && ManType == PersonType.Walker)//&& StartRoadPos == false)
        {
            ObjectLoopsScript.Walkers.Add(this.gameObject);
            //State = "FindingWalkSpot";
            //Debug.Log("Walking called");
        }
        if (Alert <= 0 && State == PersonState.WalkSpotFound && ManType == PersonType.Walker && Alive == true)
        {
            this.GetComponent<NavMeshAgent>().SetDestination(RoadPoint.transform.position);
            ArrivedPoint = RoadPoint.transform.position;
        }
        //ForMom
        //Time Setting-----------------------------
        if (ManType == PersonType.Mom && SystemScript.GlobalTime > TimeToPickChildren && State != PersonState.LookForChildren && ChildrenGot < ChildrenList.Count && Alert <= 0)
        {
            State = PersonState.LookForChildren;
            this.GetComponent<NavMeshAgent>().SetDestination(ParkPosition.transform.position);
        }

        if (ManType == PersonType.Mom && SystemScript.GlobalTime > 0 && SystemScript.GlobalTime < TimeToPickChildren && State == PersonState.GoToHouse && Alert <= 0)
        {
            State = PersonState.Nothing;
        }
        //140
        //when bagie arrives at the bench
        if (State == PersonState.GoToBench || State == PersonState.GoToWork)
        {
            if (AtSpot == true)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, closestObject.transform.rotation, 5);
            }

            if (this.Enabled() && this.GetComponent<NavMeshAgent>().remainingDistance < 1)
            {
                AtSpot = true;
            }
            else
            {
                AtSpot = false;
            }

            if (this.transform.rotation == closestObject.transform.rotation)
            {
                State = PersonState.AtWorkOrBench;
            }
        }

        //When the walker arrives at the spot then find a new one.
        if (State == PersonState.WalkSpotFound)
        {
            if ((this.transform.position.x < ArrivedPoint.x + 2 && this.transform.position.x > ArrivedPoint.x - 2) && (this.transform.position.z < ArrivedPoint.z + 2 && this.transform.position.z > ArrivedPoint.z - 2))
            {
                State = PersonState.Nothing;

                if (StartRoadPos == true)
                {
                    StartRoadPos = false;
                }
            }
        }

        if (Health <= 0)
        {
            LastHit.GetComponent<BarneyScript>().Kills = LastHit.GetComponent<BarneyScript>().Kills + 1;
            LastHit.GetComponent<BarneyScript>().AddKill();
            Destroy(gameObject);
        }

        //ForMom
        //When the mom is the right distance away from the park. 
        if (DistanceFromPark < 25 && State == PersonState.LookForChildren && ChildrenGot < ChildrenList.Count)
        {
            CallChildren = true;
            Stop = true;
        }
        if (SystemScript.GlobalTime > TimeToPickChildren && ChildrenGot == ChildrenList.Count && Alert <= 0 && ManType == PersonType.Mom)
        {
            State = PersonState.GoToHouse;
            Stop = false;
        }
        //ForMom
        //call for children is the command
        //When calling fo children,the mom sets each childs runtomom counter, and call the function
        if (CallChildren == true && ChildrenGot < ChildrenList.Count)
        {
            foreach (var MyChildren in ChildrenList)
            {
                MyChildren.GetComponent<Children>().RunToMomNow();
            }

            CallChildren = false;
        }

        if (State == PersonState.Nothing && Stop == true)
        {
            Stop = false;
        }

        if (Alert > 0 && State != PersonState.Searching)
        {
            StartSearch();
        }
        if (Alert <= 0 && State == PersonState.Searching)
        {
            CancelMovement();
        }

        if (State == PersonState.Searching)
        {
            InSearch();
        }


        //stop and start the player
        if (Stop == true)
        {
            this.GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            this.GetComponent<NavMeshAgent>().enabled = true;
        }

        if(State == PersonState.Sleeping)
        {

        }
    }

    public void ResetBench()
    {
        if (closestObject != null)
        {
            closestObject.GetComponent<Objects>().Taken = false;
        }

    }

    public void DeadMom()
    {
        if (ManType == PersonType.Mom && ChildrenList.Count > 0)
        {
            foreach (var Kids in ChildrenList)
            {
                Kids.GetComponent<Children>().DeadMommy();
            }
        }
    }

    public void KidDead(GameObject Kid)
    {
        ChildrenList.Remove(Kid);
    }
}
