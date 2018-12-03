using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    private const float k_TimerMaxium = 300.0f;
    private float m_Timer = k_TimerMaxium;

    [SerializeField] private TextMeshProUGUI m_TEXTTimer;

	// Use this for initialization
	void Start ()
    {
        m_Timer = k_TimerMaxium;
        SetTimerText();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_Timer -= Time.deltaTime;
        SetTimerText();
	}

    void SetTimerText()
    {
        m_TEXTTimer.text = Mathf.Round(m_Timer).ToString();
    }

}
