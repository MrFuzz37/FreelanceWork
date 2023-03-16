using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GoalEvent class holds a bunch of listeners that raise events when needed
/// </summary>
[CreateAssetMenu]
public class GoalEvent : ScriptableObject
{
	[Tooltip("List of all events that have been registered for a response")]
	[SerializeField] private List<Goal> _listeners = new List<Goal>();

	/// <summary>
	/// Initialise by clearing the list of listeners
	/// </summary>
	public void Init()
	{
		// because we don't create a copy of these events and the listeners are instances created at runtime
		// when closing the game you will have missing references in the list if you do not clear them
		_listeners.Clear();
	}

	/// <summary>
	/// Calls the response on all active listeners
	/// </summary>
	/// <param name="name">Name of the object, could be an enemy, location, etc.</param>
	public void Raise(string name)
	{
		for (int i = _listeners.Count - 1; i >= 0; i--)
			_listeners[i].Response.Invoke(name);
	}

	/// <summary>
	/// Register an event to have their response called 
	/// </summary>
	/// <param name="listener">The goal you want to register</param>
	public void RegisterListener(Goal listener)
	{
		// make sure we don't add the same listener more than once
		if (_listeners.Contains(listener)) { return; }

		_listeners.Add(listener); 
	}

	/// <summary>
	/// Unregister an event to not have their response called
	/// </summary>
	/// <param name="listener">The goal you want to unregister</param>
	public void UnregisterListener(Goal listener)
	{
		_listeners.Remove(listener); 
	}
}
