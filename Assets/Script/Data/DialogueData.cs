using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Dialogue/NPCDialogue")]
public class DialogueData : ScriptableObject
{
    public string       npcName;
    public Sprite       npcPortrait;
    public string[]     dialogueLines;
    public bool[]       autoProgressLines;
    public float        autoProgressDelay = 1.5f;
    public float        typingSpeed = 0.05f;
    public AudioClip    voiceSound;
    public float        voicePitch = 1f;
}
