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

    public new void OnYesButtonClicked()  
    {
        base.OnYesButtonClicked();  
        ShopUIController.instance.OpenShopUI();
    }
}

