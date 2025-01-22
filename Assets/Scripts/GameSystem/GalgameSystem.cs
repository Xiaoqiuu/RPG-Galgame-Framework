using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalgameSystem : MonoBehaviour
{
    public GalgameDataStructure dialogueDataListSO;

    // UI Elements
    public Image characterImage;
    public Text dialogueContentText;
    public GameObject galConversation;
    public GameObject choicePanel; // 新增选项面板
    public Button optionButton1;
    public Button optionButton2;
    public Text optionText1;
    public Text optionText2;

    // 动态角色贴图字典
    private Dictionary<string, Dictionary<int, Sprite>> characterSpriteDic = new Dictionary<string, Dictionary<int, Sprite>>();

    public int currentDialogueID = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool spaceKeyEnabled = true; // 控制空格键是否启用

    void Start()
    {
        StartConversation();
    }

    void Update()
    {
        if (galConversation.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 如果正在显示特效，直接显示完整文本
                StopCoroutine(typingCoroutine);
                CompleteDialogueText();
            }
            else if (!spaceKeyEnabled && choicePanel.activeSelf)
            {
                // 如果选项面板激活，按下空格选择第一个选项
                optionButton1.onClick.Invoke();
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    public void ShowDialogue(int dialogueID)
    {
        galConversation.SetActive(true);
        currentDialogueID = dialogueID;
        if (dialogueDataListSO.dialogueDataDic.TryGetValue(dialogueID, out var dialogueData))
        {
            // 启动文字逐字显示协程
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeDialogueText(dialogueData.Content, dialogueData));

            // 刷新角色贴图
            RefreshCharacterImage(dialogueData.CharacterName, dialogueData.Expression);

            // 暂时隐藏选项面板
            choicePanel.SetActive(false);

            // 更新选项面板
            UpdateOptions(dialogueData);
        }
        else
        {
            EndConversation();
        }
    }

    public void ShowNextDialogue()
    {
        if (dialogueDataListSO.dialogueDataDic.TryGetValue(currentDialogueID, out var dialogueData))
        {
            if (dialogueData.Options.Count == 0)
            {
                if (dialogueData.NextDialogueID != -1)
                {
                    ShowDialogue(dialogueData.NextDialogueID);
                }
                else
                {
                    EndConversation();
                }
            }
        }
    }

    public void StartConversation()
    {
        galConversation.SetActive(true);
        dialogueDataListSO.Initialization();
        InitializeCharacterSprites();

        if (dialogueDataListSO.dialogueDataDic.ContainsKey(0))
        {
            ShowDialogue(0);
        }
    }

    public void EndConversation()
    {
        galConversation.SetActive(false);
        choicePanel.SetActive(false); // 确保选项面板也被关闭
    }

    private void InitializeCharacterSprites()
    {
        foreach (var character in dialogueDataListSO.characterList)
        {
            if (!characterSpriteDic.ContainsKey(character.Name))
            {
                characterSpriteDic[character.Name] = new Dictionary<int, Sprite>();
                foreach (var expression in character.Expressions)
                {
                    int expressionHash = expression.ExpressionName.GetHashCode();
                    characterSpriteDic[character.Name][expressionHash] = expression.ExpressionSprite;
                }
            }
        }
    }

    private void RefreshCharacterImage(string characterName, string expressionName)
    {
        if (characterSpriteDic.TryGetValue(characterName, out var expressionDic))
        {
            int expressionHash = expressionName.GetHashCode();
            if (expressionDic.TryGetValue(expressionHash, out var sprite))
            {
                characterImage.sprite = sprite; // 更新Source Image
            }
            else
            {
                Debug.LogWarning($"Expression {expressionName} not found for character {characterName}.");
            }
        }
        else
        {
            Debug.LogWarning($"Character {characterName} not found in characterSpriteDic.");
        }
    }

    private IEnumerator TypeDialogueText(string content, DialogueData dialogueData)
    {
        isTyping = true;
        dialogueContentText.text = "";
        foreach (char letter in content)
        {
            dialogueContentText.text += letter;
            yield return new WaitForSeconds(0.05f); // 控制每个字出现的间隔时间
        }
        isTyping = false;

        // 检查是否有选项
        if (dialogueData.Options.Count > 0)
        {
            choicePanel.SetActive(true);
            spaceKeyEnabled = false; // 禁用空格键
        }
        else
        {
            choicePanel.SetActive(false);
        }
    }

    private void CompleteDialogueText()
    {
        if (dialogueDataListSO.dialogueDataDic.TryGetValue(currentDialogueID, out var dialogueData))
        {
            dialogueContentText.text = dialogueData.Content;
            isTyping = false;

            // 检查是否有选项
            if (dialogueData.Options.Count > 0)
            {
                choicePanel.SetActive(true);
                spaceKeyEnabled = false; // 禁用空格键
            }
            else
            {
                choicePanel.SetActive(false);
            }
        }
    }

    private void UpdateOptions(DialogueData dialogueData)
    {
        choicePanel.SetActive(false); // 初始化状态
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);

        if (dialogueData.Options.Count > 0)
        {
            choicePanel.SetActive(true);

            // 配置第一个选项
            optionButton1.gameObject.SetActive(true);
            optionText1.text = dialogueData.Options[0].Text;
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => HandleOptionSelection(dialogueData.Options[0].NextDialogueID));

            // 配置第二个选项（如果存在）
            if (dialogueData.Options.Count > 1)
            {
                optionButton2.gameObject.SetActive(true);
                optionText2.text = dialogueData.Options[1].Text;
                optionButton2.onClick.RemoveAllListeners();
                optionButton2.onClick.AddListener(() => HandleOptionSelection(dialogueData.Options[1].NextDialogueID));
            }
        }
    }

    private void HandleOptionSelection(int nextDialogueID)
    {
        if (nextDialogueID == -1)
        {
            EndConversation();
        }
        else
        {
            ShowDialogue(nextDialogueID);
        }
    }
}