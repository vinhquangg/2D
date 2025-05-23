//using System.Collections;
//using UnityEngine;

//public class WinConditionChecker : MonoBehaviour
//{
//    [SerializeField] private GameObject winPanel;
//    [SerializeField] private float delayBeforeShow = 2f;
//    [SerializeField] private LayerMask bossLayer;

//    private bool hasTriggered = false;

//    private void Update()
//    {
//        if (hasTriggered) return;

//        GameObject boss = FindBossByLayer();

//        if (boss == null)
//        {
//            hasTriggered = true;
//            StartCoroutine(DelayedShowWinPanel());
//        }
//    }


//    private GameObject FindBossByLayer()
//    {
//        GameObject[] allObjects = FindObjectsOfType<GameObject>();
//        foreach (var obj in allObjects)
//        {
//            if (obj.layer == LayerMask.NameToLayer("Boss"))
//            {
//                return obj.activeInHierarchy ? obj : null;
//            }
//        }
//        return null;
//    }


//    private IEnumerator DelayedShowWinPanel()
//    {
//        yield return new WaitForSecondsRealtime(delayBeforeShow);
//        if (winPanel != null)
//        {
//            winPanel.SetActive(true);
//            Time.timeScale = 0f;
//        }
//    }
//}
