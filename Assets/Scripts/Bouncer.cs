using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour {

    Dictionary<Player, Vector3> gambs = new Dictionary<Player, Vector3>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag  == "Player")
        {
        
            Debug.Log("??");
            Player p = other.gameObject. GetComponent<Player>();
            if (!gambs.ContainsKey(p))
                gambs.Add(p,Vector3.zero);


            Vector3 vel = p.GetVelocityRB();
            vel = -vel;
            vel.y = -vel.y;

            p.bouncingPending = !p.bouncingPending;

            //gambs[p] = vel;
            //p.SetVelocity(vel);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.tag == "Player")
        //{

        //    Player p = other.GetComponent<Player>();
           
          
        //    p.SetVelocity(gambs[p]);
        //}
    }


}
