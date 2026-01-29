using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // 按钮引用
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button galleryButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    // 界面面板引用
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject galleryPanel;
    [SerializeField] private GameObject settingsPanel;

    // 存档管理器
    [SerializeField] private SaveManager saveManager;

    private void Start()
    {
        // 绑定按钮事件
        startButton.onClick.AddListener(OnStartClicked);
        tutorialButton.onClick.AddListener(OnTutorialClicked);
        galleryButton.onClick.AddListener(OnGalleryClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        // 初始化面板为关闭状态
        tutorialPanel.SetActive(false);
        galleryPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    // 开始游戏按钮点击事件
    private void OnStartClicked()
    {
        bool hasSave = saveManager.HasSaveData();
        if (hasSave)
        {
           
            StartNewGame();
        }
        else
        {
            StartNewGame();
        }
    }

    // 新游戏
    private void StartNewGame()
    {
        
        SceneManager.LoadScene("OpeningCutscene"); 
    }

    // 继续游戏
    private void ContinueGame()
    {
        saveManager.LoadGame();
        SceneManager.LoadScene(saveManager.CurrentScene);
    }

    // 游戏说明按钮点击事件
    private void OnTutorialClicked()
    {
        tutorialPanel.SetActive(true);
    }

    // 图鉴按钮点击事件
    private void OnGalleryClicked()
    {
        galleryPanel.SetActive(true);
    }

    // 游戏设置按钮点击事件
    private void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // 退出游戏按钮点击事件
    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 关闭面板（可在子面板内调用此方法）
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        if (panel == settingsPanel)
        {
            Time.timeScale = 1f; // 恢复游戏时间
        }
    }
}