using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very simple kill quest template
/// </summary>
[CreateAssetMenu]
public class KillGoal : Goal
{
	[Tooltip("The name of the enemy required to complete the quest")]
    [SerializeField] private string _enemyName;

	/// <summary>
	/// Determines whether we have killed the correct enemy and updates the quest accordingly
	/// </summary>
	/// <param name="name">Name of the enemy we killed</param>
	public void OnKill(string name)
	{
		// Ensure the quest is active before doing any modifications
		if (CurrentState != State.Active) { return; }

        if (_enemyName == name)
		{
			// If the enemy we killed matches the quest requirement, increase the kill count
            current++;

			// Check if the goal is now complete
            CheckGoalCompletion();
		}
	}
}
