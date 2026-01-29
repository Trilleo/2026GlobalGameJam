using UnityEngine;
using UnityEngine.UI;

public class AtlasManager : MonoBehaviour
{
    [Header("拖入三个面板")]
    public GameObject mainAtlasPanel;  // 主图鉴面板（有2个按钮）
    public GameObject maskPanel;       // 面具图鉴面板（6个按钮）
    public GameObject monsterPanel;    // 怪物图鉴面板（7个按钮）

    [Header("拖入主图鉴面板的2个按钮")]
    public Button toMaskButton;        // "面具图鉴"按钮
    public Button toMonsterButton;     // "怪物图鉴"按钮

    [Header("拖入返回按钮")]
    public Button maskBackButton;      // 面具面板的返回按钮
    public Button monsterBackButton;   // 怪物面板的返回按钮

    void Start()
    {
        // 开始时显示主图鉴面板
        ShowPanel(mainAtlasPanel);

        // 绑定按钮事件
        toMaskButton.onClick.AddListener(() => ShowPanel(maskPanel));
        toMonsterButton.onClick.AddListener(() => ShowPanel(monsterPanel));

        // 绑定返回按钮事件
        if (maskBackButton != null)
            maskBackButton.onClick.AddListener(() => ShowPanel(mainAtlasPanel));

        if (monsterBackButton != null)
            monsterBackButton.onClick.AddListener(() => ShowPanel(mainAtlasPanel));
    }

    // 显示指定面板，隐藏其他
    void ShowPanel(GameObject panelToShow)
    {
        mainAtlasPanel.SetActive(panelToShow == mainAtlasPanel);
        maskPanel.SetActive(panelToShow == maskPanel);
        monsterPanel.SetActive(panelToShow == monsterPanel);
    }
}