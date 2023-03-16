using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The quest manager handles handles all the quests in the game, refining the available pool based on the world state
/// </summary>
public class QuestManager : MonoBehaviour
{
    [Tooltip("The pool of all available story blocks in the game")]
    [SerializeField] private List<StoryBlock> _storyBlocks = new List<StoryBlock>();

    private readonly List<StoryBlock> _refinedStoryBlocks = new List<StoryBlock>();
    private readonly List<StoryBlock> _removedStoryBlocks = new List<StoryBlock>();

    // the current world state data
    private StoryBlock.Type currentType;
    private StoryBlock.Theme currentTheme;

    /// <summary>
    /// Finds a story block that matches the required parameters
    /// </summary>
    /// <param name="themeToFind">The theme of the story block we want</param>
    /// <param name="typeOfBlock">The type of story block we are looking for</param>
    /// <returns>The chosen story block based on the parameters passed in</returns>
	public StoryBlock SelectStoryBlock(StoryBlock.Theme themeToFind, StoryBlock.Type typeOfBlock)
    {
        // Before selecting a block we need to update the world state
        SetCurrentWorldState(themeToFind, typeOfBlock);

        // grab a random story block based on what's in the refined list of story blocks
        StoryBlock selectedStoryBlock = _refinedStoryBlocks[Random.Range(0, _refinedStoryBlocks.Count)];

        // add it to a removed list and remove it from the main list as we don't want this story block again this questline
        _removedStoryBlocks.Add(selectedStoryBlock);
        _storyBlocks.Remove(selectedStoryBlock);

        // return the story block that was chosen
        return selectedStoryBlock;
    }

    /// <summary>
    /// Set some data based on the world state
    /// </summary>
    /// <param name="themeToFind">The theme of the story block we want</param>
    /// <param name="typeOfBlock">The type of story block we are looking for</param>
    private void SetCurrentWorldState(StoryBlock.Theme themeToFind, StoryBlock.Type typeOfBlock)
	{
        // set all relevant data
        currentType = typeOfBlock;
        currentTheme = themeToFind;

        // refine the list immediately after the current world state changes
        RefineStoryBlocks();
	}

    /// <summary>
    /// Refines the list of blocks to choose from based on the world state
    /// </summary>
	private void RefineStoryBlocks()
	{
        // start with a fresh clean list
        _refinedStoryBlocks.Clear();

        // Create a list that will hold all the type of story blocks we want
        List<StoryBlock> singleBlockTypes = new List<StoryBlock>();

        foreach (StoryBlock storyBlock in _storyBlocks)
        {
            // Look through all story blocks and add any blocks of the type we're looking for to the type List
            if (storyBlock.type == currentType)
                singleBlockTypes.Add(storyBlock);
        }

        foreach (StoryBlock block in singleBlockTypes)
        {
            // since only regular blocks have more than one theme, check if we are a regular block 
            if (block.type == StoryBlock.Type.Regular)
            {
                // Look through the themes for matching themes
                for (int i = 0; i < block.themes.Length; i++)
                {
                    // If we find a match add it to the refined list
                    if (block.themes[0] == currentTheme) 
                    {
                        _refinedStoryBlocks.Add(block);
                        break;
                    }
                }
            }
            else
            {
                // otherwise we can just check if our singular theme matches the theme we want and add it to the refined list
                if (block.themes[0] == currentTheme) { _refinedStoryBlocks.Add(block); }
            }
        }
    }

    /// <summary>
    /// Add all removed story blocks back into the quest pool
    /// </summary>
    public void RefreshQuestList()
	{
        foreach (StoryBlock block in _removedStoryBlocks)
		{
            _storyBlocks.Add(block);
		}

        _removedStoryBlocks.Clear();
	}
}
