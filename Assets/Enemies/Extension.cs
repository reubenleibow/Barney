using UnityEngine;

public static class Extensions
{
    public static bool Enabled(this Component Person)
    {
        return Person.GetComponent<NavMeshAgent>().enabled;
    }

    public static GameObject Clone(this GameObject old, Vector3 position)
    {
        return (GameObject)Object.Instantiate(old, position, Quaternion.identity);
    }

    public static GameObject Scale(this GameObject old, Vector3 scale)
    {
        old.transform.localScale = scale;
        return old;
    }

    public static GameObject Scale(this GameObject old, float scale)
    {
        return old.Scale(new Vector3(scale, scale, scale));
    }

    public static GameObject PathfindTo(this GameObject kids, Vector3 destination)
    {
        kids.GetComponent<NavMeshAgent>().SetDestination(destination);
        return kids;
    }

    public static GameObject PathfindTo(this GameObject kids, float x, float z)
    {
        kids.PathfindTo(new Vector3(x, 0, z));
        return kids;
    }

    public static GameObject PathfindTo(this GameObject kids, GameObject dest)
    {
        kids.PathfindTo(dest.transform.position);
        return kids;
    }

    public static bool PathfindingEnabled(this GameObject kids)
    {
        return kids.GetComponent<NavMeshAgent>().enabled;
    }
}