using UnityEngine;

public class House : MonoBehaviour
{
    public ObjectLoop ObjectLoopScript;
    public bool MomSpaceTaken = false;
    public bool ManSpaceTaken = false;
    public GameObject MomObject;
    public GameObject ManObject;

    // Use this for initialization
    void Start()
    {
        ObjectLoopScript = GameObject.Find("System").GetComponent<ObjectLoop>();
        ObjectLoopScript.Houses.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
