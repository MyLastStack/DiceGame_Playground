using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepEvaluator : MonoBehaviour
{
    public static KeepEvaluator Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
