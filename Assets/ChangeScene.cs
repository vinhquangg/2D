using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CompareTag("Outside"))
                SceneLoader.instance.LoadScene(SceneName.BossFight2);
        }

    }
}
