using UnityEngine;
using System.Collections;

public class Children : PersonBase
{
    public float TargetRange = 40;
    public int SightAngle = 60;
    public float Spotted = 0;
    public float Terra = 2;
    public int TERRACONSTANT = 2;
    public int RunDistance = 3;
    public bool GotPlayPlace = false;
    public GameObject ClosestPlayPlace;
    public bool CarringBarney = false;
    public GameObject BarneyCarried;

    public string TargetType = "Nothing";

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        ObjectLoopsScript.ChildrenM.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        var angle = Vector3.Angle(Target.transform.position - this.transform.position, this.transform.forward);
        var targetDistance = Vector3.Distance(this.transform.position, Target.transform.position);
        RaycastHit hit;
        var Xdif = Target.transform.position.x - this.transform.position.x;
        var Zdif = Target.transform.position.z - this.transform.position.z;



        var ray = Physics.Raycast(this.transform.position, Target.transform.position - this.transform.position, out hit);

        //if player is in line of sight and if the angle/range is right and is big the add to terra/spotted (barney not being carried)
        //Prevent messing up "hit"
        if (CarringBarney == false && ray == true)
        {
            if (hit.transform.name == Target.name && targetDistance <= TargetRange && angle <= SightAngle)
            {
                Debug.DrawRay(this.transform.position, Target.transform.position - this.transform.position, Color.magenta);


                if (Spotted <= SPOTTEDCONSTANT)
                {
                    Spotted = Spotted + 1 * Time.deltaTime;
                }
                if (Spotted >= SPOTTEDCONSTANT)
                {
                    Alert = ALERTCONSTANT;
                    TargetAquired = true;

                    //find path to the baby barney
                    if (SystemScript.Big == false && Terra <= 0 && CarringBarney == false)
                    {
                        this.GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
                        TargetType = "SmallBarney";
                    }
                    if (SystemScript.Big == true)
                    {
                        TargetType = "BigBarney";
                        Terra = TERRACONSTANT;
                    }
                }
            }
        }

        //Run away because of terra
        if (Terra > 0)
        {
            this.GetComponent<NavMeshAgent>().SetDestination(new Vector3(Xdif * -RunDistance, this.transform.position.y, Zdif * -RunDistance));
            //Debug.Log("Tera");
        }


        //PickUpBarney
        if (TargetAquired == true)
        {
            var distance = Vector3.Distance(this.transform.position, Target.transform.position);

            if (Target.GetComponent<BarneyScript>().BarneySize == false && Terra <= 0 && distance < 1)
            {
                Target.GetComponent<BarneyScript>().carried = true;
                CarringBarney = true;
                BarneyCarried = Target;
                StartWander();
            }
        }
        // when have baby barney the run around the park
        if (CarringBarney == true)
        {
            BarneyCarried.transform.position = this.transform.position;

            if (Alert <= 0)
            {
                UpdateMovement();
            }

            if (BarneyCarried.GetComponent<BarneyScript>().BarneySize == true)
            {
                CarringBarney = false;
            }
        }

        //When not in sight: terra goes down first then alert

        //If it is in sight Or being carried
        var HitRayPosible = false;
        if (ray == true)
        {
            if (CarringBarney == false && hit.transform.name != Target.name)
            {
                HitRayPosible = true;
            }
            if (CarringBarney == false && hit.transform.name == Target.name)
            {
                HitRayPosible = false;
            }
        }
        if (HitRayPosible == true || targetDistance > TargetRange || angle > SightAngle || CarringBarney == true)
        {
            if (Spotted > 0)
            {
                Spotted = Spotted - 1 * Time.deltaTime;
            }

            if (Alert > 0 && Terra <= 0)
            {
                Alert = Alert - 1 * Time.deltaTime;
            }
            if (Alert > 0)
            {
                State = PersonState.SpottedPlayer;
            }
            if (Terra > 0)
            {
                Terra = Terra - 1 * Time.deltaTime;
                State = PersonState.Terror;
            }
        }

        if(State == PersonState.Terror)
        {
            RunInTerror(Target,gameObject);
        }

        if (Spotted <= SPOTTEDCONSTANT)
        {
            TargetAquired = false;
        }

        if (Alert <= 0 && State == PersonState.SpottedPlayer)
        {
            ObjectLoopsScript.ChildrenP.Add(this.gameObject);
            State = PersonState.Nothing;
        }

        if (Alert > 0 && GotPlayPlace == true)
        {
            ResetPlayPlace();
            GotPlayPlace = false;
        }

        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        if(State == PersonState.GoingHome)
        {
            ToHouse(this.gameObject,ItsHouse, PersonType);
        }

        if(ItsMom != null && ItsHouse == null)
        {
            ItsHouse = ItsMom.GetComponent<Person>().ItsHouse;
        }

        UpdateMovement();
    }

    void ResetPlayPlace()
    {
        ClosestPlayPlace.GetComponent<Objects>().Taken = false;
    }

    public void DeadMommy()
    {
        //ItsMom = null;
        ObjectLoopsScript.ChildrenM.Add(this.gameObject);
    }
}
