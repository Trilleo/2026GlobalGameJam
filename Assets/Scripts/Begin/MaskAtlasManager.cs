// MaskAtlasManager.cs - 挂在面具面板上
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskAtlasManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject maskListPanel;      // 6个按钮的面板
    public GameObject maskDetailPanel;    // 详情面板

    [Header("详情页UI")]
    public Text maskNameText;
    public Text normalAttackText;
    public Text skillText;
    public Text specialText;

    [Header("6个面具按钮")]
    public List<MaskButtonInfo> maskButtons = new List<MaskButtonInfo>();

    // 定义一个类来存储按钮数据
    [System.Serializable]
    public class MaskButtonInfo
    {
        public Button button;            // 按钮组件
        public string maskName;          // 面具名称
        public string normalAttack;      // 普通攻击
        public string skill;             // 技能
        public string special;           // 特殊效果
    }

    void Start()
    {
        // 隐藏详情面板
        maskDetailPanel.SetActive(false);

        // 给每个按钮绑定事件
        for (int i = 0; i < maskButtons.Count; i++)
        {
            int index = i;  // 重要：保存索引

            maskButtons[i].button.onClick.AddListener(() =>
            {
                // 直接使用按钮对应的数据
                ShowMaskDetail(index);
            });
        }
    }

    // 显示面具详情
    void ShowMaskDetail(int index)
    {
        if (index < 0 || index >= maskButtons.Count) return;

        MaskButtonInfo info = maskButtons[index];

        // 更新UI
        maskNameText.text = info.maskName;
        normalAttackText.text = $"普通攻击：\n{info.normalAttack}";
        skillText.text = $"技能：\n{info.skill}";
        specialText.text = $"特殊：\n{info.special}";

        // 切换面板
        maskListPanel.SetActive(false);
        maskDetailPanel.SetActive(true);
    }

    // 返回按钮调用
    public void BackToList()
    {
        maskDetailPanel.SetActive(false);
        maskListPanel.SetActive(true);
    }
}