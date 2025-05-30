﻿using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject playerInputPrefab;
    public GameObject pauseGamePrefab;
    public GameObject playerUI;
    public GameObject shopUI;  

    private GameObject pauseGameInstance;
    private GameObject playerUIInstance;
    private GameObject shopInstance;  
    private bool isPaused;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (FindObjectOfType<PlayerInputHandler>() == null)
        {
            var input = Instantiate(playerInputPrefab);
            DontDestroyOnLoad(input);
        }

        if (FindObjectOfType<MenuController>() == null)
        {
            pauseGameInstance = Instantiate(pauseGamePrefab);
            DontDestroyOnLoad(pauseGameInstance);
        }

        if (GameObject.FindGameObjectWithTag("PlayerUI") == null)
        {
            playerUIInstance = Instantiate(playerUI);
            DontDestroyOnLoad(playerUIInstance);
        }

        if(GameObject.FindGameObjectWithTag("ShopUI") == null)
        {
            shopInstance = Instantiate(shopUI);
            DontDestroyOnLoad(shopInstance);
        }
        else
        {
            pauseGameInstance = FindObjectOfType<MenuController>().gameObject;
        }
    }

    private void Start()
    {
        if (pauseGameInstance != null)
        {
            pauseGameInstance.SetActive(false);
        }
        if (shopInstance != null)
        {
            shopInstance.SetActive(false);
        }

        PlayerInputHandler.instance.uiAction.Pause.performed += OnPausePerformed;

    }


    private void OnDestroy()
    {
        if (PlayerInputHandler.instance != null)
            PlayerInputHandler.instance.uiAction.Pause.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseGameInstance != null)
            pauseGameInstance.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;
    }

    public void HidePlayerUI()
    {
        if (playerUIInstance != null)
        {
            playerUIInstance.SetActive(false);
        }
    }

    public void ShowPlayerUI()
    {
        if (playerUIInstance != null)
        {
            playerUIInstance.SetActive(true);
        }
    }

    public void OpenShopUI()
    {
        if (shopInstance == null)
        {
            shopInstance = Instantiate(shopUI);
            DontDestroyOnLoad(shopInstance);
        }

        shopInstance.SetActive(true); 
    }

    public void CloseShopUI()
    {
        if (shopInstance != null)
        {
            shopInstance.SetActive(false);
        }
    }

}
