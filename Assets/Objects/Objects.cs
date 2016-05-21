using UnityEngine;

public class Objects : MonoBehaviour
{
    public GameObject System;
    public bool Taken = false;
    public string Type = "SitPlace";

    // Use this for initialization
    void Start()
    {
        if (Type == "SitPlace" || Type == "WorkPlace")
        {
            System.GetComponent<ObjectLoop>().Bench.Add(this.gameObject);
        }

        if (Type == "PlayPlace")
        {
            System.GetComponent<ObjectLoop>().PlayPlace.Add(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
