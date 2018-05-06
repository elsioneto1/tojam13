using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdEntity : MonoBehaviour {

    public static float vectorPower = 1;
    public static float speed;

    public GameObject[] objectsNearby;
    public GameObject[] registeredColliders;
    //public CrowdGravity gravityComponent;
    public Vector3 movementVec;
    private SpriteRenderer rendererComp;

	// Use this for initialization
	void Start () {

        objectsNearby = new GameObject[10]; // max of 10 objects
        registeredColliders = new GameObject[10];
        rendererComp = GetComponent<SpriteRenderer>();
        rendererComp.sprite = SpritePool.S_INSTANCE.pool[Mathf.RoundToInt(Random.Range(0, SpritePool.S_INSTANCE.pool.Length-1))];
    }
	
	// Update is called once per frame
	public void CustomUpdate () {
        if (gameObject.activeSelf)
            transform.position -= movementVec * Time.deltaTime;
	}

    public void ProcessMovementVector(GameObject gravityComponent)
    {

        if (!gameObject.activeSelf)
            return;
        Vector3 entitityResultant = Vector3.zero;
        Vector3 managerResultant = Vector3.zero;
        bool insideCollider = false;

        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (objectsNearby[i] != null)
            { 
                entitityResultant += ( transform.position - objectsNearby[i].transform.position ).normalized ;  
            }
            if (registeredColliders[i] != null)
            {
                managerResultant += (transform.position - registeredColliders[i].transform.position).normalized;
                insideCollider = true;
                
            }
        }
        float compensation = insideCollider ? 0.09f : 0;
        managerResultant *= (CrowdManager.vectorPower + compensation);

        if (managerResultant == Vector3.zero && gravityComponent != null)
        {
            managerResultant += (gravityComponent.transform.position - transform.position).normalized;
            managerResultant *= CrowdGravity.vectorPower;
        }
        
        

        entitityResultant.Normalize();

        movementVec = -(entitityResultant + managerResultant).normalized ;
        movementVec.y = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        bool checkForCollider = false;
        if (other.gameObject.tag == "CrowdManager")
        {
            checkForCollider = true;
        }
        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (!checkForCollider)
            {
                if (objectsNearby[i] == null)
                {
                    objectsNearby[i] = other.gameObject;
                    break;
                }
            }
            else
            {
                if (registeredColliders[i] == null)
                {
                    registeredColliders[i] = other.gameObject;
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool checkForCollider = false;
        if (other.gameObject.tag == "CrowdManager")
        {
            checkForCollider = true;
        }
        for (int i = 0; i < objectsNearby.Length; i++)
        {
            if (!checkForCollider)
            {
                if (objectsNearby[i] == other.gameObject)
                {
                    objectsNearby[i] = null;
                    break;
                }
            }
            else
            {
                if (registeredColliders[i] == other.gameObject)
                {
                    registeredColliders[i] = null;
                    break;
                }
            }
        }
    }

}
