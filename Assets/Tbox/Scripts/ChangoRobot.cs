using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ChangoRobot : MonoBehaviour
{

    public bool conversationHasStarted = false;
    public bool conversationHasEnded = false;

    public NPCConversation myConversation;

    public void StartConversation()
    {
        if (!conversationHasStarted)
        {
            conversationHasStarted = true;
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
