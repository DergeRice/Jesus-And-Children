using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInteract : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.instance.isPossibleCreate =false;
    }
    
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if(GameManager.instance != null) GameManager.instance.SetIsPossible();
    }
}
