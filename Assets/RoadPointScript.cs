using UnityEngine;
using System.Collections;

public class RoadPointScript : MonoBehaviour
{
    public GameObject SystemObject;
    public int Ordering;

    // Use this for initialization
    void Start()
    {
        SystemObject = GameObject.Find("System");
        //SystemObject.GetComponent(ObjectLoop).Roads.Add(this.gameObject);
        SystemObject.GetComponent<ObjectLoop>().RoadSorter.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
