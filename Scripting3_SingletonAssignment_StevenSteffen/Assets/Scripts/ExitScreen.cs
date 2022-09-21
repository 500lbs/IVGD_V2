using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
        DestroyObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Exit"))
        {
            Destroy(obj);
        }
    }
}
