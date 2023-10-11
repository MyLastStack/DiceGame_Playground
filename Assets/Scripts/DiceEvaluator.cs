using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DiceEvaluator : MonoBehaviour
{
    public static DiceEvaluator Instance;

    public int[] dVH;

    public bool lgStr, smStr, fHouse, fourK, threeK, twoP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dVH = new int[6];
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

    public void DiceValueOnHand(int[] DiceValueCount)
    {
        dVH = DiceValueCount;
    }

    private bool LargeStraightCheck()
    {
        bool lrgstr = false;

        int currentCount = 0;
        int maxValue = 0;
        
        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 0)
            {
                currentCount = 0;
            }
            else
            {
                currentCount++;
            }

            if (currentCount > maxValue)
            {
                maxValue = currentCount;
            }
        }

        if (maxValue == 5)
        {
            lrgstr = true;
        }

        return lrgstr;
    }

    private bool SmallStraightCheck()
    {
        bool smstr = false;

        int currentCount = 0;
        int maxValue = 0;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 0)
            {
                currentCount = 0;
            }
            else
            {
                currentCount++;
            }

            if (currentCount > maxValue)
            {
                maxValue = currentCount;
            }
        }

        if (maxValue == 4)
        {
            smstr = true;
        }

        return smstr;
    }
    private bool FullHouseCheck()
    {
        bool three = false, pair = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 3)
            {
                three = true;
            }
            if (dVH[i] == 2)
            {
                pair = true;
            }

            if (three && pair)
            {
                break;
            }
        }

        return three && pair;
    }
    private bool FourKind()
    {
        bool fourk = false;

        for (int i = 0; i < dVH.Length; i++ )
        {
            if (dVH[i] == 4)
            {
                fourk = true;
                break;
            }
        }

        return fourk;
    }
    private bool ThreeKind()
    {
        bool threek = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 3)
            {
                threek = true;
                break;
            }
        }

        return threek;
    }
    private bool TwoPair()
    {
        bool firstP = false, secondP = false;

        for(int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 2)
            {
                if (firstP)
                {
                    secondP = true;
                    break;
                }
                else
                {
                    firstP = true;
                }
            }
        }

        return firstP && secondP;
    }
}
