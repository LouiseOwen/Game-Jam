using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentOpen : MonoBehaviour {

    [SerializeField] private ParticleSystem m_PresentOpen;

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
            // would like to make box jump up and down
            Instantiate(m_PresentOpen, transform.position, Quaternion.identity);
            m_PresentOpen.Play();
            transform.parent.parent.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.1f, 0.0f);
            transform.gameObject.SetActive(false);

        }
    }

}
