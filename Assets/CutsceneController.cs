using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; 

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance { get; private set; }

    public GameObject player;
    public GameObject enemy;
    public GameObject menu; 

    [Header("Timeline")]
    public PlayableDirector director;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (director != null && director.state == PlayState.Playing)
            director.Stop();
        player.SetActive(false);
        enemy.SetActive(false);
    }
    public void PlayCutscene()
    {
        player.SetActive(true);
        enemy.SetActive(true);
        menu.SetActive(false);
        if (director != null)
        {
            director.Play(); 
        }
    }

    public void OnPlayerReachedDoor()
    {
        SceneLoader.instance.LoadScene(SceneName.SampleScene);
    }
}
