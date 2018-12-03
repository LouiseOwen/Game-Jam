using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
		
	}
	

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f) * Time.deltaTime);
	}

}
