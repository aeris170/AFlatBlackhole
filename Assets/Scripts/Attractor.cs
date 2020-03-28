using UnityEngine;

public class Attractor : MonoBehaviour
{
    private Collider ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(ownCollider.enabled)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Eatable");
            foreach (GameObject o in objects)
            {
                Vector3 topOfHole = new Vector3(transform.position.x, ownCollider.bounds.max.y, transform.position.z);
                Vector3 objectToBlackhole = topOfHole - o.transform.position;
                if (Vector3.Distance(topOfHole, o.transform.position) < o.GetComponent<EatableObject>().AttractionDistance)
                {
                    o.GetComponent<Rigidbody>().AddForce(10 * objectToBlackhole, ForceMode.Force);
                }
            }
        }
    }
}
