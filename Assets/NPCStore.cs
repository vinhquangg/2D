using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStore : BaseNPC
{
    protected override void Start()
    {
        base.Start(); 
    }

    protected override void Update()
    {
        base.Update(); 
        OpenShopUI();  
    }

    private void OpenShopUI()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))  
        {
            base.Interact(); 
        }
    }

    protected override void OnDialogueComplete()  
    {
        base.OnDialogueComplete();

        ShopUIController.instance.OpenShopUI();
    }
}

