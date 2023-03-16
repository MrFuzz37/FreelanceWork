using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The story block class holds information about a set of goals and themes
/// </summary>
[CreateAssetMenu]
public class StoryBlock : ScriptableObject
{
    [Tooltip("The goals required for this story block to be considered complete")]
    [SerializeField] private Goal[] _questSuccessGoals;

    [Tooltip("The goals required for this story block to be considered failed")]
    [SerializeField] private Goal[] _questFailGoals;

    // These are the goals that can be modified as they are instance objects not the asset themselves
    [SerializeField] private List<Goal> _instanceSuccessQuestGoals = new List<Goal>();
    public List<Goal> QuestGoals => _instanceSuccessQuestGoals;

    [SerializeField] private List<Goal> _instanceFailQuestGoals = new List<Goal>();
    public List<Goal> QuestFailGoals => _instanceFailQuestGoals;

    // The active goals in this story block, use these when getting the active goal information
    private Goal _activeSuccessGoal;
    public Goal ActiveSuccessGoal => _activeSuccessGoal;

    private Goal _activeFailGoal;
    public Goal ActiveFailGoal => _activeFailGoal;

    // what goal we're up to if we have multiple
    private int successIndex = 0;
    private int failIndex = 0;

    // an event for when we have completed a story block
    public static UnityEvent onStoryComplete = new UnityEvent();
    public static UnityEvent onStoryFailure = new UnityEvent();

    public enum Type
	{
        Starter,
        Regular,
        Finale
	}

    [Tooltip("The type of story block this is")]
    public Type type;

    public enum Theme
    {
        Scifi,
        Fantasy,
        PostApocalyptic
    }

    [Tooltip("The theme/s this story block is connected to")]
    public Theme[] themes;

    /// <summary>
    /// Setup our goals by creating instances of them and set the active goal
    /// </summary>
    public void Init()
	{
		for (int i = 0; i < _questSuccessGoals.Length; i++)
		{
            // create an instance of each goal
            Goal instanceGoal = Instantiate(_questSuccessGoals[i]);

            // initialise the goal
            instanceGoal.Init();

            // add the instance goal to the list of goals
            _instanceSuccessQuestGoals.Add(instanceGoal);
		}
        for (int i = 0; i < _questFailGoals.Length; i++)
        {
            // create an instance of each goal
            Goal instanceGoal = Instantiate(_questFailGoals[i]);

            // initialise the goal
            instanceGoal.Init();

            // add the instance goal to the list of goals
            _instanceFailQuestGoals.Add(instanceGoal);
        }
        // set the active goal
        SetGoals();
	}

    /// <summary>
    /// Sets the active goal
    /// </summary>
    public void SetGoals()
	{
        if (_instanceSuccessQuestGoals.Count > successIndex)
        {
            // if we have another goal, it becomes the active goal
            _activeSuccessGoal = _instanceSuccessQuestGoals[successIndex];
            _activeSuccessGoal.SetToActive(this);
        }
        if (_instanceFailQuestGoals.Count > failIndex)
		{
            // if we have another goal, it becomes the active goal
            _activeFailGoal = _instanceFailQuestGoals[failIndex];
            _activeFailGoal.SetToActive(this);
        }
    }

    /// <summary>
    /// Run when one of the goals has been successfully completed
    /// </summary>
    public virtual void Success(Goal goal)
	{
        // determine the type of goal to run the correct response
        if (_instanceSuccessQuestGoals.Contains(goal)) { SuccessGoalComplete(); }
        else { FailGoalComplete(); }
	}

    /// <summary>
    /// Run when a success goal is completed
    /// </summary>
    private void SuccessGoalComplete()
	{
        successIndex++;

        if (_instanceSuccessQuestGoals.Count > successIndex)
        {
            // if there is another goal left in the list, it becomes the active goal
            SetGoals();
        }
        else
        {
            // otherwise this story is now complete
            onStoryComplete.Invoke();
        }
    }

    /// <summary>
    /// Run when a fail goal is completed
    /// </summary>
    public virtual void FailGoalComplete()
    {
        // increase index to go to the next goal
        failIndex++;

        if (_instanceFailQuestGoals.Count > failIndex)
        {
            // if there is another goal left in the list, it becomes the active goal
            SetGoals();
        }
        else
        {
            // otherwise this story has failed
            onStoryFailure.Invoke();
        }
    }
}
