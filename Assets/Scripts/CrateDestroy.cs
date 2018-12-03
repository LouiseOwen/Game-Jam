using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroy : MonoBehaviour {

    [SerializeField] private ParticleSystem m_CreateDestroy;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Crate"))
        {
            Instantiate(m_CreateDestroy);
            Destroy(other.gameObject);
        }
    }

}
