using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The Goal class is the base class for all goals that requires being inherited from.
/// Contains all the general information and functionality needed for all goals.
/// </summary>
public abstract class Goal : ScriptableObject
{
    [Tooltip("How many are needed to complete the goal. e.g. How many of 'x' do I need to kill")]
    [SerializeField] private int _value;

    [Tooltip("The story block this goal is associated with.")]
    [SerializeField] private StoryBlock _parentQuest;

    [Tooltip("The current completion of the goal.")]
    [SerializeField] protected int current;

    [Tooltip("What event are we listening for.")]
    [SerializeField] private GoalEvent _event;

    [Tooltip("What do we do in response to the event.")]
    public UnityEvent<string> Response;

    public enum State
	{
        Inactive,
        Active,
        Complete,
        Failed
	}

    [Tooltip("The current state of the goal")]
    public State CurrentState { get; private set; }

    // general purpose event for when a goal has failed
    public static UnityEvent onGoalFail = new UnityEvent();

    /// <summary>
    /// Initialise the goal
    /// </summary>
    public void Init()
	{
        _event.Init();
	}

    /// <summary>
    /// Checks if the current goal has been completed
    /// </summary>
	protected void CheckGoalCompletion()
	{
        if (current >= _value) 
        {
            // if our current value is higher than the target value, the goal is complete
            CurrentState = State.Complete;

            // we can now unregister our event as this goal is now complete
             _event.UnregisterListener(this); 

            // tell the story block that our goal was a success
            _parentQuest.Success(this);
        }
	}

    /// <summary>
    /// Sets a goal to active
    /// </summary>
    /// <param name="parent">The story block with this goal</param>
    public void SetToActive(StoryBlock parent)
	{
        // our current state becomes active
        CurrentState = State.Active;

        // and we set which story block we belong to
        _parentQuest = parent;

        // register the event so we can have the response be called
        if (_event != null) { _event.RegisterListener(this); }
        else { Debug.LogError("An event has not been set! Please set one so this goal can function properly."); }
    }
}
