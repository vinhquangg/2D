using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NPCStateMachine npcStateMachine;
    public bool isPlayerInRange;
    public GameObject pointA;
    public GameObject pointB;
    public float patrolSpeed = 2f; 
    private Vector3 targetPosition;

    private void Start()
    {
        npcStateMachine = GetComponent<NPCStateMachine>();
        if (npcStateMachine == null)
        {
            Debug.LogError("NPCStateMachine component not found on NPCController.");
            return;
        }

        targetPosition = pointA.transform.position;
    }

    void Update()
    {
        if (npcStateMachine != null)
        {
            npcStateMachine.Update();
        }

        Patrol();
        OpenShopUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void OpenShopUI()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShopUIController.instance.OpenShopUI();
        }
    }

    public void CloseShopUI()
    {
        if (isPlayerInRange)
        {
            ShopUIController.instance.CloseShopUI();
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }
    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = (targetPosition == pointA.transform.position) ? pointB.transform.position : pointA.transform.position;
        }
    }
}
