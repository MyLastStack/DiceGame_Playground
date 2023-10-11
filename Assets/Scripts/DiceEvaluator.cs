using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEvaluator : MonoBehaviour
{
    public static DiceEvaluator Instance;

    public bool lgStr, smStr, fHouse, fourK, threeK, twoP;

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
