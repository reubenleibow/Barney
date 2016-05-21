using UnityEngine;

public abstract class PersonBase : MonoBehaviour
{
    private const float NearDistance = 5.0f;
    private const float AtDistance = 1.0f;

    protected const int ALERTCONSTANT = 2;
    protected const int SPOTTEDCONSTANT = 2;

    protected SystemBehaviour SystemScript;
    protected ObjectLoop ObjectLoopsScript;

    public GameObject Target;
    public bool TargetAquired = false;

    public float Alert = 0;

    public bool Stop = false;
    public PersonState State = PersonState.Nothing;
    public bool Nocturnal = false;
    public PersonGender Gender = PersonGender.Male;
    public PersonType PersonType = PersonType.Walker;
    public float Health = 100;
    public bool IsAlive
    {
        get { return Health > 0; }
    }

    // Movement Stuff
    public MovementType MovementType = MovementType.Stopped;
    public MovementState MovementState
    {
        get
        {
            var nav = this.GetComponent<NavMeshAgent>();

            if (!nav.enabled)
            {
                return MovementState.NotMoving;
            }

            var destination = nav.destination;
            var distance = Vector3.Distance(destination, this.transform.position);
            if (distance <= NearDistance)
            {
                return MovementState.NearDestination;
            }
            if (distance <= AtDistance)
            {
                return MovementState.AtDestination;
            }

            return MovementState.Moving;
        }
    }

    // Mom Stuff
    public bool AccountedFor = false;
    public bool HasMom
    {
        get { return ItsMom != null; }
    }
    public GameObject ItsMom;
    public bool IsNearMom
    {
        get
        {
            if (!HasMom)
            {
                return false;
            }

            var distance = Vector3.Distance(this.transform.position, ItsMom.transform.position);
            return distance <= NearDistance;
        }
    }

    public bool HasHouse = false;
    public GameObject ItsHouse;

    // Use this for initialization
    public virtual void Start()
    {
        var SYSTEM = GameObject.Find("System");
        SystemScript = SYSTEM.GetComponent<SystemBehaviour>();
        ObjectLoopsScript = SYSTEM.GetComponent<ObjectLoop>();
    }

    public void Kill()
    {
        Health = 0;
        Stop = true;

        if (ItsMom != null)
        {
            ItsMom.GetComponent<Person>().KidDead(this.gameObject);
            ItsMom = null;
        }

        SystemScript.GetComponent<ObjectLoop>().RemovePerson(gameObject,PersonType);

    }

    protected void StartWander()
    {
        var point = new Vector3(
            Random.Range(SystemScript.ParkXStart, SystemScript.ParkXEnd),
            0,
            Random.Range(SystemScript.ParkZStart, SystemScript.ParkZEnd));

        StartMovement(point, MovementType.Wandering);
    }

    public void RunToMomNow()
    {
        // keep finding path to mom.
        if (Alert <= 0 && HasMom)
        {
            StartMovement(ItsMom.transform.position, MovementType.RunToMom);
        }
    }

    public void GoHome()
    {
        // keep finding path to mom.
        if (Alert <= 0 && ItsHouse)
        {
            StartMovement(ItsHouse.transform.position, MovementType.GoHome);
            State = PersonState.GoingHome;
        }
    }

    public void StopMovement()
    {
        MovementType = MovementType.Stopped;

        var nav = this.GetComponent<NavMeshAgent>();
        nav.enabled = false;
    }

    private void StartMovement(Vector3 point, MovementType type)
    {
        if (IsAlive)
        {
            MovementType = type;

            var nav = this.GetComponent<NavMeshAgent>();
            nav.enabled = true;
            nav.SetDestination(point);
        }
    }

    protected void UpdateMovement()
    {
        if (MovementType == MovementType.Wandering)
        {
            if (MovementState == MovementState.AtDestination)
            {
                StartWander();
            }
        }
        else if (MovementType == MovementType.RunToMom)
        {
            if (IsNearMom)
            {
                if (AccountedFor == false)
                {
                    AccountedFor = true;
                    ItsMom.GetComponent<Person>().ChildrenGot += 1;
                    State = PersonState.GoingHome;
                }
                if (AccountedFor && MovementState != MovementState.NotMoving)
                {
                    RunToMomNow();
                }
            }
        }
    }

    private Vector3 point = new Vector3(0, 0, 0);
    private float distance;
    protected void StartSearch()
    {
        point = Target.transform.position;
        distance = Vector3.Distance(this.transform.position, point);

        this.GetComponent<NavMeshAgent>().SetDestination(point);
        State = PersonState.Searching;
    }

    protected void InSearch()
    {
       // Debug.Log(distance);
        distance = Vector3.Distance(this.transform.position, point);
        if (distance <= 2)
        {
           // Debug.Log("SetPoint");
            point = Target.transform.position;
           

            this.GetComponent<NavMeshAgent>().SetDestination(point);
        }
    }

    protected void CancelMovement()
    {
        State = PersonState.Nothing;
    }

    protected void RunInTerror(GameObject barney, GameObject person)
    {
        var Origin = barney.transform.position - person.transform.position;
        var newPosx = person.transform.position.x - Origin.x;
        var newPosz = person.transform.position.z - Origin.z;
        var zDisplacement = barney.transform.position.z - person.transform.position.z;
        var xDisplacement = barney.transform.position.x - person.transform.position.x;
        var endX = SystemScript.EndOfWorldX;
        var endZ = SystemScript.EndOfWorldZ;

        if (newPosx < 0)
        {
            newPosx = 0;
            newPosz -= (zDisplacement / Mathf.Abs(zDisplacement)) * 50;
        }

        if (newPosz < 0)
        {
            newPosz = 0;
            newPosx -= (xDisplacement / Mathf.Abs(xDisplacement)) * 50;
        }

        if (newPosx > endX)
        {
            newPosx = 0;
            newPosz -= (zDisplacement / Mathf.Abs(zDisplacement)) * 50;
        }

        if (newPosz > endZ)
        {
            newPosz = 0;
            newPosx -= (xDisplacement / Mathf.Abs(xDisplacement)) * 50;
        }
        Debug.Log(newPosx + "," + newPosz);
        person.PathfindTo(newPosx, newPosz);
    }

    public float distanceFromHouse;
    protected void ToHouse (GameObject person, GameObject house,PersonType personType)
    {
        distanceFromHouse = Vector3.Distance(person.transform.position, house.transform.position);

        if(distanceFromHouse <= 2 && personType == PersonType.Child)
        {
            person.GetComponent<Children>().State = PersonState.Sleeping;
        }

        if (distanceFromHouse <= 2 && personType != PersonType.Child)
        {
            person.GetComponent<Person>().State = PersonState.Sleeping;
        }
    }
}
