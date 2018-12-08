using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestroy : MonoBehaviour {

    [SerializeField] private ParticleSystem m_CrateDestroy;

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
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(m_CrateDestroy, transform.position, Quaternion.identity);
            m_CrateDestroy.Play();
            Destroy(transform.parent.parent.gameObject); // parent.parent is baaaddd
            
        }
    }

}
