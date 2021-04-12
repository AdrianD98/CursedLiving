using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManage : MonoBehaviour
{
    //uses unityEvents to make text boxes appear / dissapper

    public GameObject target;
    public float waitTime;
    public bool waitForInput;

    public UnityEvent manager = new UnityEvent();


    IEnumerator waitInput()
    {
        yield return new WaitForSeconds(waitTime);

        Time.timeScale = 0;

        
    }

    private void Start()
    {
        if (waitForInput) StartCoroutine(waitInput());    
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            {
                if (Input.GetKey(KeyCode.Q))
                {

                    Time.timeScale = 1;
                    manager.Invoke();
                    this.enabled = false;
                }
          }
        if (!target && !waitForInput) manager.Invoke();
    }
}
