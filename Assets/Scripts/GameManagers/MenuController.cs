using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    #region Singleton

    public static MenuController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Menu Controller found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject world;
    public GameObject mainMenu;

    public GameObject titleScreen;
    public Button playButton;
    public Button controlButton;
    public AudioSource titleTrack;

    public GameObject controlMenu;
    public TextMeshProUGUI controlText;
    public Image controlImage;
    public Button closeButton;

    void Start()
    {
        ArrangeMenu();
        world.SetActive(false);
        mainMenu.SetActive(true);
        titleTrack.Play();
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        world.SetActive(true);
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        titleTrack.Stop();
        GameController.instance.StartGame();
    }

    private void ArrangeMenu()
    {
        ArrangeTitleScreen();
        ArrangeControlScreen();
    }

    private void ArrangeTitleScreen()
    {
        RectTransform parentPanel = titleScreen.GetComponent<RectTransform>();
        RectTransform pButton = playButton.GetComponent<RectTransform>();
        RectTransform cButton = controlButton.GetComponent<RectTransform>();
        pButton.localPosition = new Vector3(0, 0, 0);
        pButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentPanel.rect.height/12);
        pButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentPanel.rect.width/8);
        cButton.localPosition = new Vector3(0, -parentPanel.rect.height/12, 0);
        cButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentPanel.rect.height / 12);
        cButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentPanel.rect.width / 8);
        titleScreen.SetActive(true);
    }

    private void ArrangeControlScreen()
    {
        RectTransform parentPanel = titleScreen.GetComponent<RectTransform>();
        RectTransform controlWindow = controlMenu.GetComponent<RectTransform>();
        RectTransform text = controlText.GetComponent<RectTransform>();
        RectTransform image = controlImage.GetComponent<RectTransform>();
        RectTransform exit = closeButton.GetComponent<RectTransform>();
        controlWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentPanel.rect.height);
        controlWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentPanel.rect.height);
        text.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.rect.height / 2);
        text.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3*image.rect.width / 4);
        exit.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, image.rect.height/8);
        exit.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, image.rect.width/8);
        exit.localPosition = new Vector3(image.rect.height/2-exit.rect.height, image.rect.width/2-exit.rect.width, 0);

        controlMenu.SetActive(false);
    }

    public void CloseTitleMenu()
    {
        titleScreen.SetActive(false);
    }

    public void OpenTitleMenu()
    {
        titleScreen.SetActive(true);
    }

    public void CloseControlMenu()
    {
        controlMenu.SetActive(false);
    }

    public void OpenControlMenu()
    {
        controlMenu.SetActive(true);
    }
}

