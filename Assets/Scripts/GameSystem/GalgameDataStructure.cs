using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Galgame/Conversation")]
public class GalgameDataStructure : ScriptableObject
{
    public List<DialogueData> dialogueDataList;
    public List<Character> characterList;

    private Dictionary<int, DialogueData> DialogueDataDic;
    public Dictionary<int, DialogueData> dialogueDataDic => DialogueDataDic;

    private Dictionary<string, Character> CharacterDic;
    public Dictionary<string, Character> characterDic => CharacterDic;

    public void Initialization()
    {
        if (DialogueDataDic == null)
        {
            DialogueDataDic = new Dictionary<int, DialogueData>();

            foreach (var dialogueData in dialogueDataList)
            {
                if (!DialogueDataDic.ContainsKey(dialogueData.ID))
                {
                    DialogueDataDic.Add(dialogueData.ID, dialogueData);
                }
                else
                {
                    Debug.LogWarning($"Duplicate Dialogue ID {dialogueData.ID} detected in DialogueDataList.");
                }
            }
        }

        if (CharacterDic == null)
        {
            CharacterDic = new Dictionary<string, Character>();

            foreach (var character in characterList)
            {
                if (!CharacterDic.ContainsKey(character.Name))
                {
                    CharacterDic.Add(character.Name, character);
                }
                else
                {
                    Debug.LogWarning($"Duplicate Character Name {character.Name} detected in CharacterList.");
                }
            }
        }
    }
}

[System.Serializable]
public class DialogueData
{
    public int ID;
    public string CharacterName;
    public string Expression;
    public string Content;
    public List<OptionData> Options;
    public int NextDialogueID; // 新增下一个对话的ID编号
}

[System.Serializable]
public class OptionData
{
    public string Text;
    public int NextDialogueID;
}

[System.Serializable]
public class Character
{
    public string Name;
    public List<ExpressionData> Expressions;

    public Character(string name)
    {
        Name = name;
        Expressions = new List<ExpressionData>();
    }
}

[System.Serializable]
public class ExpressionData
{
    public string ExpressionName;
    public Sprite ExpressionSprite;
}
