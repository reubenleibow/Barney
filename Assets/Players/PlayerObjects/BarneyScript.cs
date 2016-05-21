using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarneyScript : MonoBehaviour
{

    public bool moving = false;
    public string movingState = "Idle";
    public bool falling = true;
    public SystemBehaviour SystemScript;
    public ObjectLoop ObjectLoopS;
    public float BarneyHealth = 100;
    public float AttackRange = 10;
    public float AttackDamage = 10;
    public int Kills = 0;
    public float totalRotation = 0;
    public bool PlayingAttackAnimation = false;
    public bool BarneySize = true;
    public bool carried = false;
    //gameObjects
    public GameObject playerCamera;
    public GameObject MainSystemS;
    public GameObject ActionCamera;
    public GameObject Kill;
    // playervariables
    public float FSpeed = 0.5f;
    public float BSpeed = 0.2f;
    public bool timeStart = false;
    public float aTime = 0;
    public float timeConstant = 1.5f;

    // Use this for initialization
    void Start()
    {

        SystemScript = MainSystemS.GetComponent<SystemBehaviour>();
        ObjectLoopS = MainSystemS.GetComponent<ObjectLoop>();
        HealthLost();
        AddKill();
    }

    // Update is called once per frame
    void Update()
    {
        //Setting size of barney------------------------
        if (SystemScript.Big == true)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }


        if (Input.GetKey("q"))
        {
            SystemScript.Big = true;
            BarneySize = true;
        }

        if (Input.GetKey("e"))
        {
            SystemScript.Big = false;
            BarneySize = false;
        }


        //Setting size of barney------------------------
        //Death of barney ---------------------------------------

        if (BarneyHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        //Death of barney ---------------------------------------

        bool ray = false;
        RaycastHit hit = new RaycastHit();
        if (Input.GetKeyDown("r"))
        {
            ray = Physics.Raycast(this.transform.position, this.transform.forward, out hit, AttackRange);
        }

        if (ray == true)
        {

            GameObject chosenMan;
            float angle;

            if (hit.transform.tag == "Man")
            {
                chosenMan = hit.collider.gameObject;
                angle = Vector3.Angle(this.transform.position - chosenMan.transform.position, chosenMan.transform.forward);
                Debug.Log("chosenMan" + chosenMan);
                chosenMan.GetComponent<Person>().Alive = false;
                Debug.Log("Set Tag MAN");

                if (chosenMan.GetComponent<Person>().ManType == PersonType.Mom && chosenMan.GetComponent<Person>().ChildrenList.Count > 0)
                {
                    chosenMan.GetComponent<Person>().DeadMom();
                }

                if (angle > 120)
                {
                    Debug.Log("Reached");
                    this.GetComponent<Animation>().Play("AttackAnimation0");
                    PlayingAttackAnimation = true;
                    chosenMan.GetComponent<Person>().Stop = true;
                    Debug.Log(Kill);
                    Kill = chosenMan;

                    //ObjectLoopS.Bagie.Remove(Kill);
                }
                else
                {
                    chosenMan.GetComponent<Person>().Health = chosenMan.GetComponent<Person>().Health - AttackDamage;
                    chosenMan.GetComponent<Person>().LastHit = this.gameObject;
                }
            }

            if (hit.transform.tag == "Child")
            {
                chosenMan = hit.collider.gameObject;
                angle = Vector3.Angle(this.transform.position - chosenMan.transform.position, chosenMan.transform.forward);
                Debug.Log(angle);
                chosenMan.GetComponent<Children>().Kill();
                //Debug.Log("Set Tag");

                if (angle > 120)
                {
                    this.GetComponent<Animation>().Play("AttackAnimation0");
                    PlayingAttackAnimation = true;
                    chosenMan.GetComponent<Children>().StopMovement();
                    Kill = chosenMan;
                }
                else
                {
                    chosenMan.GetComponent<Children>().Health = chosenMan.GetComponent<Children>().Health - AttackDamage;
                }
            }
        }

        if (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("a"))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        if (PlayingAttackAnimation == false)
        {
            if (Input.GetKey("w"))
            {
                movingState = "forward";

                this.transform.Translate(Vector3.forward * FSpeed);
            }

            if (Input.GetKey("s"))
            {
                movingState = "backward";

                this.transform.Translate(Vector3.back * BSpeed);
            }

            if (Input.GetKey("a"))
            {
                movingState = "left";

                this.transform.Translate(Vector3.left * BSpeed);
            }

            if (Input.GetKey("d"))
            {
                movingState = "right";

                this.transform.Translate(Vector3.right * BSpeed);
            }
        }

        if (PlayingAttackAnimation == false)
        {
            if (moving == true)
            {
                if (Input.GetKey("w"))
                {
                    this.GetComponent<Animation>().Play("Walking0");
                    this.GetComponent<Animation>()["Walking0"].speed = 2.0f;
                }

                if (Input.GetKey("s"))
                {
                    this.GetComponent<Animation>().Play("Walking1");
                    this.GetComponent<Animation>()["Walking1"].speed = 1.2f;

                }

                //if(movingState == "right")
                //{
                //	if(!Input.GetKey("w") && !Input.GetKey("s"))
                //	{
                //		this.GetComponent.<Animation>().Play("Walking2");
                //		this.GetComponent.<Animation>()["Walking2"].speed = 2;
                //		
                //	}
                //	else
                //	{
                //		this.transform.Translate(Vector3.right * BSpeed);
                //	}
                //	
                //	if(this.GetComponent.<Animation>()["Walking2"].time > 1)
                //	{
                //		this.transform.Translate(Vector3.right * BSpeed);
                //	}
                //	
                //}
                //
                //if(movingState == "left")
                //{
                //	if(!Input.GetKey("w") && !Input.GetKey("s"))
                //	{
                //		this.GetComponent.<Animation>().Play("Walking3");
                //		this.GetComponent.<Animation>()["Walking3"].speed = 2;
                //	}
                //	else
                //	{
                //		this.transform.Translate(Vector3.left * BSpeed);
                //	}
                //	
                //	if(this.GetComponent.<Animation>()["Walking3"].time > 1)
                //	{
                //		this.transform.Translate(Vector3.left * BSpeed);
                //	}
                //	
                //}
            }
            else
            {
                this.GetComponent<Animation>().Play("Idle0");
            }
        }

        if (PlayingAttackAnimation == true)
        {
            ActionCamera.GetComponent<Camera>().enabled = true;
            playerCamera.GetComponent<Camera>().enabled = false;
        }
        else
        {
            ActionCamera.GetComponent<Camera>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = true;
        }

        if (PlayingAttackAnimation == true && timeStart == false)
        {
            timeStart = true;
            this.transform.position = Kill.GetComponent<AIShare>().BehindePos.transform.position;
        }
        if (aTime <= timeConstant && timeStart == true)
        {
            aTime = aTime + 1 * Time.deltaTime;
        }
        if (aTime >= timeConstant)
        {
            Destroy(Kill);
            Kills++;
            AddKill();
            Debug.Log("destroy" + Kill);
            timeStart = false;
            aTime = 0;
            PlayingAttackAnimation = false;

        }

        //mouse movement----------------------------------------------
        if (PlayingAttackAnimation == false)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;

            var h = Input.GetAxis("Mouse X");
            var v = Input.GetAxis("Mouse Y");

            if (totalRotation < 60 && -v > 0)
            {
                playerCamera.transform.Rotate(-v, 0, 0);
                totalRotation = totalRotation - v;
            }

            if (totalRotation > -60 && -v < 0)
            {
                playerCamera.transform.Rotate(-v, 0, 0);
                totalRotation = totalRotation - v;
            }

            this.transform.Rotate(0, h, 0);
        }
        //falling = true;


    }

    public void HealthLost()
    {
        SystemScript.PlayerOneHealthText.GetComponent<Text>().text = "Health" + BarneyHealth;
    }

    public void AddKill()
    {
        SystemScript.PlayerOneKillsText.GetComponent<Text>().text = "Kills: " + Kills;
    }

}
