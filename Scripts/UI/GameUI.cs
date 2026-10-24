using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    PanelRenderer renderer;
    GroupBox gameOverMenu;
    Button[] buttons;
    Label gameOverLabel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        renderer = GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        renderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        renderer.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement, int version)
    {
        if (rootElement == null) return;

        gameOverMenu = rootElement.Q<GroupBox>("GameOverMenu");
        buttons = gameOverMenu.Query<Button>().ToList().ToArray();
        buttons[0].clicked += GameMaster.Instance.StartNextLevel;
        buttons[1].clicked += GameMaster.Instance.RestartLevel;
        buttons[2].clicked += GameMaster.Instance.GoToMainMenu;
        gameOverLabel = gameOverMenu.Q<Label>("GameOverLabel");
    } 

    public void ToggleGameOverMenu(bool displayed, string text = "")
    {
        if (gameOverMenu != null)
            gameOverMenu.style.display = displayed ? DisplayStyle.Flex : DisplayStyle.None;

        if (displayed) gameOverLabel.text = text;
    }
}
