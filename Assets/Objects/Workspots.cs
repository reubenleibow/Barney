using UnityEngine;
using System.Collections;

public class WorkSpots : MonoBehaviour
{
    public bool Taken = false;
    public GameObject System;

    // Use this for initialization
    void Start()
    {
        System.GetComponent<ObjectLoop>().BagieWorkPlace.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
