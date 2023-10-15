using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DiceEvaluator : MonoBehaviour
{
    public static DiceEvaluator Instance;

    public int[] dVH;

    public bool[] category = new bool[6];
    public bool[] priority = new bool[6];
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
        for (int i = 0; i < DiceValueCount.Length; i++)
        {
            dVH[i] = DiceValueCount[i];
        }

        category[3] = LargeStraightCheck();
        category[2] = SmallStraightCheck();
        category[5] = FullHouseCheck();
        category[1] = FourKindCheck();
        category[0] = ThreeKindCheck();
        category[4] = TwoPairCheck();
    }
    
    #region Checks
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

        if (maxValue == 4) smstr = true;

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

            if (three && pair) break;
        }

        return three && pair;
    }
    private bool FourKindCheck()
    {
        bool fourk = false;

        for (int i = 0; i < dVH.Length; i++ )
        {
            if (dVH[i] >= 4)
            {
                fourk = true;
                break;
            }
        }

        return fourk;
    }
    private bool ThreeKindCheck()
    {
        bool threek = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] >= 3)
            {
                threek = true;
                break;
            }
        }

        return threek;
    }
    private bool TwoPairCheck()
    {
        bool firstP = false, secondP = false;

        for(int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] >= 2)
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
    #endregion

    public void DiceValueToKeep(int[] DiceValueCount)
    {
        for (int i = 0; i < DiceValueCount.Length; i++)
        {
            dVH[i] = DiceValueCount[i];
        }

        priority[0] = OneOffThreeKindAndTwoPair();
        priority[1] = OneOffThreeKindAndTwoPair();
        priority[2] = OneOffFourKind();
        priority[3] = OneOffSmlStr();
        priority[4] = OneOffFullHouse();
        priority[5] = OneOffLrgStr();

        for (int i = 0; i < priority.Length; i++)
        {
            if (!priority[i]) continue;
            for (int d = 0; d < priority.Length; d++)
            {
                if (!priority[d]) continue; else if (d >= i) continue;

                if (priority[d] && d > i)
                {
                    priority[i] = false;
                }
            }
        }
    }

    #region OneOffs
    private bool OneOffLrgStr()
    {
        bool lrgstr = false;
        int amountZero = 0;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 0)
            {
                amountZero++;
            }
        }

        if (amountZero <= 2)
        {
            lrgstr = true;
        }

        return lrgstr;
    }
    private bool OneOffSmlStr()
    {
        bool smlstr = false;
        int amountZero = 0;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] == 0)
            {
                amountZero++;
            }
        }

        if (amountZero <= 3)
        {
            smlstr = true;
        }

        return smlstr;
    }
    private bool OneOffFullHouse()
    {
        bool chk1 = false;
        bool chk2 = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] >= 2 && chk1)
            {
                chk2 = true;
            }
            else
            {
                chk1 = true;
            }
            if (chk1 && chk2) break;
        }

        return chk1 && chk2;
    }

    private bool OneOffFourKind()
    {
        bool check = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] >= 3)
            {
                check = true;
                break;
            }
        }

        return check;
    }
    private bool OneOffThreeKindAndTwoPair()
    {
        bool check = false;

        for (int i = 0; i < dVH.Length; i++)
        {
            if (dVH[i] >= 2)
            {
                check = true;
                break;
            }
        }

        return check;
    }
    #endregion
}
