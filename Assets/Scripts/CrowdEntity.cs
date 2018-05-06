using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdEntity : MonoBehaviour {

    public static float vectorPower = 1;
    public static float speed;

    public GameObject[] objectsNearby;
    public CrowdManager crowdManagerComp;
    public Vector3 movementVec;
	// Use this for initialization
	void Start () {

        objectsNearby = new GameObject[10]; // max of 10 objects

    }
	
	// Update is called once per frame
	public void CustomUpdate () {
        if (gameObject.activeSelf)
        transform.position -= movementVec * Time.deltaTime;
	}

    public void ProcessMovementVector(GameObject crowdManager)
    {

        if (!gameObject.activeSelf)
            return;
        Vector3 entitityResultant = Vector3.zero;
        Vector3 managerResultant = Vector3.zero;
        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (objectsNearby[i] != null)
            {
                if (objectsNearby[i].tag == "CrowdEntitity")
                {
                    entitityResultant += ( transform.position - objectsNearby[i].transform.position ).normalized ;
                }
               
            }
           

        }

        bool tooCloseFromCenter = false;
        if (crowdManagerComp != null)
        {
            managerResultant += (transform.position - crowdManagerComp.transform.position).normalized;
        }
        else 
        {

            managerResultant = (crowdManager.transform.position - transform.position).normalized;
        }

        if (crowdManagerComp != null && crowdManagerComp.sphereCollider)
        {
            if ( (Vector3.Distance(crowdManagerComp.transform.position,transform.position)/ crowdManagerComp.sphereCollider.radius) < 0.99f)
            {
                tooCloseFromCenter = true;
            }
        }
        entitityResultant.Normalize();
        float compensation = tooCloseFromCenter ? 0.5f : 0;

        managerResultant *= (CrowdManager.vectorPower  +compensation);
        movementVec = -(entitityResultant + managerResultant).normalized ;
        movementVec.y = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CrowdManager")
        {
            crowdManagerComp = other.gameObject.GetComponent<CrowdManager>();
            return;
        }
        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (objectsNearby[i] == null)
            {
                objectsNearby[i] = other.gameObject;
               
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CrowdManager")
        {
            crowdManagerComp = null;
            return;
        }
        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (objectsNearby[i] == other.gameObject)
            {
                objectsNearby[i] = null;
                break;
            }
        }
    }

}
