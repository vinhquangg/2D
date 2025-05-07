using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    protected NPCStateMachine npcStateMachine;
    public bool isPlayerInRange;
    protected Animator anim;
    public Rigidbody2D rb { get; private set; }

    public GameObject pointA;
    public GameObject pointB;
    public float moveSpeed;
    public SpriteRenderer spriteRenderer;
    public Transform currentPoint { get; set; }

    public DialogueData dialogueData;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText, nameText;
    public Image portraitImage;
    public TextMeshProUGUI yesText;  
    public Button yesButton;  

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    protected virtual void Start()
    {
        npcStateMachine = GetComponent<NPCStateMachine>();
        if (npcStateMachine == null)
        {
            Debug.LogError("NPCStateMachine component not found on BaseNPC.");
            return;
        }
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPoint = pointA.transform;

        dialoguePanel.SetActive(false);
        yesText.gameObject.SetActive(false);  
        yesButton.gameObject.SetActive(false); 
        yesButton.onClick.AddListener(OnYesButtonClicked); 
    }

    protected virtual void Update()
    {
        if (npcStateMachine != null)
        {
            npcStateMachine.Update();
        }

        OpenShopUI();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    protected virtual void OpenShopUI()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    public virtual void Flip(Transform targetPoint)
    {
        if (targetPoint == null) return;

        Vector3 scale = transform.localScale;

        if (targetPoint.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    public virtual void Interact()
    {
        if (dialogueData == null || isDialogueActive)
        {
            return;
        }

        StartDialogue();  
    }

    public bool CanInteract()
    {
        return !isDialogueActive;  
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);  
        portraitImage.sprite = dialogueData.npcPortrait;  

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());  
        Time.timeScale = 0f;
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText(""); 

        // Gõ từng chữ trong mỗi dòng hội thoại
        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }

        isTyping = false;

        // Kiểm tra nếu dòng hội thoại đã hoàn tất và tự động chuyển sang dòng tiếp theo nếu được cài đặt
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay); 
            NextLine();
        }
        else
        {

            ShowYesText();
            
        }
    }

    public void ShowYesText()
    {
        yesText.gameObject.SetActive(true);  
        yesButton.gameObject.SetActive(true);  
        yesText.SetText("Yes");  
    }

    // Sự kiện khi người chơi nhấn "Yes"
    private void OnYesButtonClicked()
    {
        EndDialogue();  
        ShopUIController.instance.OpenShopUI();  
        yesText.gameObject.SetActive(false); 
        yesButton.gameObject.SetActive(false); 
    }

    // Kết thúc hội thoại và quay lại trạng thái ban đầu
    public void EndDialogue()
    {
        StopAllCoroutines(); 
        isDialogueActive = false;  
        dialogueText.SetText("");  
        dialoguePanel.SetActive(false);  
        npcStateMachine.SwitchState(new NPCIdleState(npcStateMachine));  
        Time.timeScale = 1f;
    }

}
