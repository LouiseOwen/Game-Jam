using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour {

    public GameObject player;

    private bool moving = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void LateUpdate ()
    {
		if (player.transform.position.x > transform.position.x) // once passes midpoint
        {
            moving = true;
        }
        // get end object to stop camera moving

        if (moving)
        {
            transform.position = new Vector3(player.transform.position.x, 4.0f, -10.0f);
        }

    }
}
