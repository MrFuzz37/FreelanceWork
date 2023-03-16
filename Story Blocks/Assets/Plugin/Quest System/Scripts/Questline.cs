using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Questline : MonoBehaviour
{
    [SerializeField] private QuestManager _questManager;
    [Tooltip("The story blocks chosen to form this questline")]
    [SerializeField] private List<StoryBlock> _storyBlocks = new List<StoryBlock>();
    [Tooltip("How many quests are required to complete this questline")]
    [SerializeField] private int _questsToComplete = 4;

    // what story block we're currently at in the questline
    private int _index = 0;

    void Start()
    {
        // just a reminder that this requires a reference to the quest manager
        if (_questManager == null)
            Debug.LogError("The variable QuestManager is not set!");

        // subscribe to the event 
        StoryBlock.onStoryComplete.AddListener(SelectNewStoryBlock);

        // Grab a starting story block.
        // NOTE: Can be deleted later if you don't want to start off with any quests, it's just for demonstration purposes
        AddStoryBlock(StoryBlock.Theme.Fantasy, StoryBlock.Type.Starter);
    }

    /// <summary>
    /// Adds a new story block to the questline
    /// </summary>
    private void AddStoryBlock(StoryBlock.Theme theme, StoryBlock.Type type)
	{
        StoryBlock newStoryBlock = Instantiate(_questManager.SelectStoryBlock(theme, type));
        newStoryBlock.Init();
        _storyBlocks.Add(newStoryBlock);
    }

    /// <summary>
    /// Selects a new story block to add to the questline
    /// </summary>
    private void SelectNewStoryBlock()
	{
        // Increase our index to indicate we've completed a quest
        _index++;

        // If we only have 1 left to complete
        if (_index == _questsToComplete - 1)
		{
            //we look for a finale block with the same theme as the starting
            AddStoryBlock(_storyBlocks[0].themes[0], StoryBlock.Type.Finale);
        }
        // otherwise if we've completed enough quests, gain the rewards and clear the current questline
        else if (_index >= _questsToComplete)
		{
            // gain rewards


            _storyBlocks.Clear();

            // Refresh the available story blocks
            _questManager.RefreshQuestList();
		}
        // if none of the above are true, we find a regular story block
		else
		{
            AddStoryBlock(_storyBlocks[0].themes[0], StoryBlock.Type.Regular);
        }
	}
}
