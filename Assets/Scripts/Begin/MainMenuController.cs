using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // 主菜单按钮
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button galleryButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    // 面板（在编辑器中拖拽赋值）
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject galleryPanel;
    [SerializeField] private GameObject settingsPanel;

    // 面板的关闭按钮
    [SerializeField] private Button tutorialCloseButton;
    [SerializeField] private Button galleryCloseButton;
    [SerializeField] private Button settingsCloseButton;

    void Start()
    {
        // 绑定主菜单按钮事件
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (tutorialButton != null) tutorialButton.onClick.AddListener(ShowTutorialPanel);
        if (galleryButton != null) galleryButton.onClick.AddListener(ShowGalleryPanel);
        if (settingsButton != null) settingsButton.onClick.AddListener(ShowSettingsPanel);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        // 绑定面板关闭按钮事件
        if (tutorialCloseButton != null) tutorialCloseButton.onClick.AddListener(HideTutorialPanel);
        if (galleryCloseButton != null) galleryCloseButton.onClick.AddListener(HideGalleryPanel);
        if (settingsCloseButton != null) settingsCloseButton.onClick.AddListener(HideSettingsPanel);

        // 初始化：隐藏所有面板
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (galleryPanel != null) galleryPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // 开始游戏
    void StartGame()
    {
        SceneManager.LoadScene("TestLevel1");
    }

    // 显示教程面板
    void ShowTutorialPanel()
    {
        HideAllPanels(); 
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    // 显示图鉴面板
    void ShowGalleryPanel()
    {
        HideAllPanels();
        if (galleryPanel != null)
        {
            galleryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // 显示设置面板
    void ShowSettingsPanel()
    {
        HideAllPanels();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // 隐藏教程面板
    void HideTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
    }

    // 隐藏图鉴面板
    void HideGalleryPanel()
    {
        if (galleryPanel != null)
        {
            galleryPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // 隐藏设置面板
    void HideSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // 隐藏所有面板
    void HideAllPanels()
    {
        HideTutorialPanel();
        HideGalleryPanel();
        HideSettingsPanel();
    }

    // 退出游戏
    void QuitGame()
    {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}