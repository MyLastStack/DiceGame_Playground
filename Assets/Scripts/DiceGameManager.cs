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
            yield return new WaitForSeconds(0.5f);
            ClaimToggle(CountDiceValue);
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
        DiceEvaluator.Instance.DiceValueToKeep(CountDiceValue);

        int[] priority = new int[6];
        priority[0] = 0;
        priority[1] = 4;
        priority[2] = 1;
        priority[3] = 2;
        priority[4] = 5;
        priority[5] = 3;

        for (int index = 0; index < DiceEvaluator.Instance.category.Length; index++)
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
                        Debug.Log($"Highest: {index} - #{i}");
                    }
                }
            }
        }

        Debug.Log(highest);

        if (!GoalGUIManager.Instance.goalButtons[highest].m_goalClaimed)
        {
            GoalGUIManager.Instance.goalButtons[highest].Claim();
        }
    }

    void KeepRolls(int[] DiceValueCount)
    {

        // Toggle off anything over 1 for straights
        // If a zero is in the middle like 1 1 0 1 1 1
            // Toggle off the last number
    }
}
