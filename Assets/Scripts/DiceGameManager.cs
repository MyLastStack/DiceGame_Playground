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

    public DiceEvaluator DiceEvaluator;

    public static DiceGameManager Instance;

    public int rollCount = 0;
    public int score = 0;
    public int rollsLeft = 0;
    private int rollsMax = 1;

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
                Debug.Log(Dicelist[d].newValue);
                CountDiceValue[Dicelist[d - 1].newValue]++;
                continue;
            }
            else
            {
                Dicelist[d].RollToRandomSide();
            }

            Debug.Log(Dicelist[d].newValue);
            // Record the dice count
            CountDiceValue[Dicelist[d - 1].newValue]++;

            yield return new WaitForSeconds(0.125f);
        }

        for (int d = 0; d < CountDiceValue.Length; d++)
        {
            Debug.Log(CountDiceValue[d]);
        }

        isRolling = false;
        GoalGUIManager.Instance.ReleaseButtons();
        rollCount += 1;
        StatsGUI.Instance.UpdateStatsGUI();
    }

    void CheckRollsLeft()
    {
        rollsLeft -= 1;
        if (rollsLeft < 0)
        {
            foreach (var d in KeepDiceButtons)
            {
                d.ResetDice();
            }
            rollsLeft = rollsMax;
        }
    }
}
