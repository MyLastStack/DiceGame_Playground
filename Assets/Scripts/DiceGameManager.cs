using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceGameManager : MonoBehaviour
{
    public Dice[] Dicelist;
    public DiceButton[] KeepDiceButtons;

    public int[] CountDiceValue;

    public bool isRolling;

    public static DiceGameManager Instance;

    public int rollCount = 0;
    public int score = 0;
    public int rollsLeft = 0;
    private int rollsMax = 1;

    public int highest = 99;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            rollCount = 0;
            score = 0;
            rollsLeft = 2;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Roll()
    {
        if (!isRolling)
        {
            StartCoroutine(RollAllDice());
        }
    }

    IEnumerator RollAllDice()
    {
        isRolling = true;
        CheckRollsLeft();
        GoalGUIManager.Instance.ProtectButtons();

        // Reset Count Value
        CountDiceValue = new int[6];

        for (int d = 0; d < Dicelist.Length; d++)
        {
            
            if (KeepDiceButtons[d].m_keepDice) 
            {
                CountDiceValue[Dicelist[d].newValue - 1]++;
                continue;
            }
            else
            {
                Dicelist[d].RollToRandomSide();
            }

            // Record the dice count
            CountDiceValue[Dicelist[d].newValue - 1]++;

            yield return new WaitForSeconds(0.125f);
        }

        Debug.Log(string.Join("; ", CountDiceValue));

        isRolling = false;
        GoalGUIManager.Instance.ReleaseButtons();

        // Dice Evaluator Action
        DiceEvaluator.Instance.DiceValueOnHand(CountDiceValue);
        for (int index = 0; index < DiceEvaluator.Instance.category.Length; index++)
        {
            if (!DiceEvaluator.Instance.category[index])
            {
                GoalGUIManager.Instance.ProtectSpecificButtons(index);
            }
            else
            {
                GoalGUIManager.Instance.ReleaseSpecificButtons(index);
            }
        }

        if (rollsLeft == 0)
        {
            yield return new WaitForSeconds(0.25f);
            ClaimToggle(CountDiceValue);
        }
        else if (rollsLeft == rollsMax)
        {
            yield return new WaitForSeconds(0.125f);
            KeepRolls(CountDiceValue);
        }

        rollCount += 1;
        StatsGUI.Instance.UpdateStatsGUI();
    }

    void CheckRollsLeft()
    {
        rollsLeft -= 1;
        highest = 99;
        if (rollsLeft < 0)
        {
            foreach (var d in KeepDiceButtons)
            {
                d.ResetDice();
            }
            rollsLeft = rollsMax;
        }
    }

    void ClaimToggle(int[] DiceValueCount)
    {
        int[] priority = new int[6];
        priority[0] = 0;
        priority[1] = 4;
        priority[2] = 1;
        priority[3] = 2;
        priority[4] = 5;
        priority[5] = 3;

        for (int index = 0; index < DiceValueCount.Length; index++)
        {
            #region other method
            //if (!DiceEvaluator.Instance.category[index])
            //{
            //    GoalGUIManager.Instance.ProtectSpecificButtons(index);
            //}
            //else
            //{
            //    GoalGUIManager.Instance.ReleaseSpecificButtons(index);
            //    for (int i = 0; i < priority.Length; i++)
            //    {
            //        Debug.Log($"Combine{index} - Placement{i}");
            //        if (index == priority[i])
            //        {
            //            highest = index;
            //            Debug.Log($"Highest: {index} - #{i}");
            //        }
            //    }
            //}
            #endregion

            if (GoalGUIManager.Instance.goalButtons[index].GetComponent<Button>().interactable)
            {
                for (int i = 0; i < priority.Length; i++)
                {
                    if (index == priority[i])
                    {
                        highest = index;
                    }
                }
            }
            else
            {
                continue;
            }
        }

        if (highest != 99)
        {
            if (!GoalGUIManager.Instance.goalButtons[highest].m_goalClaimed)
            {
                GoalGUIManager.Instance.goalButtons[highest].Claim();
            }
        }
    }

    void KeepRolls(int[] DiceValueCount)
    {
        DiceEvaluator.Instance.DiceValueToKeep(DiceValueCount);

        int chosenPrio = 99;

        for (int i = 0; i < DiceEvaluator.Instance.priority.Length; i++)
        {
            if (DiceEvaluator.Instance.priority[i])
            {
                chosenPrio = i;
            }
        }

        Debug.Log(chosenPrio);

        switch (chosenPrio)
        {
            case 0:
                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 2)
                    {
                        for (int d = 0; d < Dicelist.Length; d++)
                        {
                            if (Dicelist[d].newValue - 1 == i)
                            {
                                KeepDiceButtons[d].ToggleDice();
                            }
                        }
                        break;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 2)
                    {
                        for (int d = 0; d < Dicelist.Length; d++)
                        {
                            if (Dicelist[d].newValue - 1 == i)
                            {
                                KeepDiceButtons[d].ToggleDice();
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 1)
                    {
                        for (int d = 0; d < Dicelist.Length; d++)
                        {
                            if (Dicelist[d].newValue - 1 == i)
                            {
                                KeepDiceButtons[d].ToggleDice();
                            }
                        }
                        break;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 3)
                    {
                        for (int d = 0; d < Dicelist.Length; d++)
                        {
                            if (Dicelist[d].newValue - 1 == i)
                            {
                                KeepDiceButtons[d].ToggleDice();
                            }
                        }
                        break;
                    }
                }
                break;
            case 3:
                int iStart = 99; // Default 99
                int iStartUp = iStart; // Default 99

                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 1 && iStart == 99)
                    {
                        iStart = i;
                        iStartUp = iStart;
                    }
                    else if (DiceValueCount[i] == 0 && iStart != 99)
                    {
                        iStart = 99;
                        iStartUp = iStart;
                    }
                    else if (DiceValueCount[i] >= 1 && iStart != 99)
                    {
                        iStartUp++;
                    }
                }

                while (iStartUp != iStart - 1)
                {
                    for (int d = 0; d < Dicelist.Length; d++)
                    {
                        if (Dicelist[d].newValue - 1 == iStartUp)
                        {
                            KeepDiceButtons[d].ToggleDice();
                            break;
                        }
                    }

                    iStartUp--;
                }

                break;
            case 4:
                int pt1 = 99;
                int pt2 = 99;
                int pt3 = 99;

                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 3)
                    {
                        pt1 = i;
                        break;
                    }
                }

                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] >= 2)
                    {
                        if (pt2 != 99)
                        {
                            pt3 = i;
                            break;
                        }
                        else
                        {
                            pt2 = i;
                        }
                    }
                }

                if (pt1 != 99)
                {
                    for (int d = 0; d < Dicelist.Length; d++)
                    {
                        if (Dicelist[d].newValue - 1 == pt1)
                        {
                            KeepDiceButtons[d].ToggleDice();
                        }
                    }
                }
                else if (pt1 == 99 && pt2 != 99 && pt3 != 99)
                {
                    for (int d = 0; d < Dicelist.Length; d++)
                    {
                        if (Dicelist[d].newValue - 1 == pt2 || Dicelist[d].newValue - 1 == pt3)
                        {
                            KeepDiceButtons[d].ToggleDice();
                        }
                    }
                }

                break;
            case 5:
                int highestCount = 0;
                int indexFound = 99;

                for (int i = 0; i < DiceValueCount.Length; i++)
                {
                    if (DiceValueCount[i] == 2)
                    {
                        highestCount = 2;
                        indexFound = i;
                        break;
                    }
                }

                if (indexFound != 99) // if there is a value over 1
                {
                    if (highestCount >= 2)
                    {
                        for (int d = 0; d < Dicelist.Length; d++)
                        {
                            if (Dicelist[d].newValue - 1 == indexFound)
                            {
                                if (highestCount < 2)
                                {
                                    break;
                                }
                                else
                                {
                                    highestCount--;
                                }
                            }

                            KeepDiceButtons[d].ToggleDice();
                        }
                    }
                }
                else // If there is a gap but no values over 1
                {
                    int keptstr = 0;
                    for (int i = 0; i < DiceValueCount.Length; i++)
                    {
                        if (keptstr != 4 && DiceValueCount[i] > 0)
                        {
                            for (int d = 0; d < Dicelist.Length; d++)
                            {
                                if (Dicelist[d].newValue - 1 == i)
                                {
                                    KeepDiceButtons[d].ToggleDice();
                                    break;
                                }
                                else continue;
                            }
                            keptstr++;
                        }
                        else break;
                    }
                }
                break;
            default:
                break;
        }
    }
}
