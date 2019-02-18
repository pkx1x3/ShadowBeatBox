﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI state machine for Shadowboxing
/// Uses enum for state machines along with switch statement
/// </summary>
[RequireComponent(typeof(UnitHealth))]
public class SimplestAI : MonoBehaviour
{
    MoveLoader parsedList;
    MoveContainer moveContainer;
    Animator animator;
    MoveList activeMoveList;

    public const string path = "MoveDatabase";

    int stateDuration = 0;
    int lastBeatCounter;

    int activeMoveIndex = 4;

    public int randomMoveList;


    /// <summary>
    /// Start function, initializes state as well as starts state machine
    /// </summary>
    private void Start()
    {
        parsedList = GetComponent<MoveLoader>();
        animator = GetComponent<Animator>();
        moveContainer = MoveContainer.Load(path);
        lastBeatCounter = Global.counterBPM;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Global.counterBPM != lastBeatCounter && stateDuration <= 0)
        {
            UpdateState();
            
            lastBeatCounter = Global.counterBPM;
        }
        else if (Global.counterBPM != lastBeatCounter)
        {
            stateDuration -= 1;
            lastBeatCounter = Global.counterBPM;
        }
    }

    /// <summary>
    /// Handles the state change
    /// </summary>
    private void UpdateState()
    {
        
        if (activeMoveIndex >= 4)
        {
            
            randomMoveList = Random.Range(0, moveContainer.moves.Count);
            activeMoveList = moveContainer.moves[randomMoveList];
            activeMoveIndex = 0;

            EnemyActions(activeMoveList.moves.Split(',')[activeMoveIndex]);
            stateDuration = int.Parse(activeMoveList.duration.Split(',')[activeMoveIndex]);
        }
        else
        {
            EnemyActions(activeMoveList.moves.Split(',')[activeMoveIndex]);

            stateDuration = int.Parse(activeMoveList.duration.Split(',')[activeMoveIndex]);
        }

        activeMoveIndex = activeMoveIndex + 1;

    }

    /// <summary>
    /// Enemy Action state machine
    /// </summary>
    private void EnemyActions(string animationTransition)
    {
		if(!GetComponent<UnitHealth>().KnockedDown)
		{
			animator.Play(animationTransition, 0, 0f);
		}
	}
}

public class Global
{
    public static bool switchStateBPM = false;
    public static int counterBPM = 0;
}