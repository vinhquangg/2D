using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            SceneManager.sceneLoaded += OnSceneLoad;

            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.Find("PlayerChangeSpawn");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player hoặc PlayerChangeSpawn trong scene.");
        }

        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}
