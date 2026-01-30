using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInfoManager : MonoBehaviour
{
    private static PlayerInfoManager instance;
    public static PlayerInfoManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance=FindObjectOfType<PlayerInfoManager>();
                if (instance == null)
                {
                    Debug.Log("No PlayerInfoManager!");
                }
            }
            return instance;
        }
    }

    public UnityEngine.UI.Image bloodback;
    public UnityEngine.UI.Image bloodbarquick;
    public UnityEngine.UI.Image bloodbarslow;
    public TextMeshProUGUI bloodtext;
    [SerializeField] private float fadetime;
    public UnityEngine.UI.Image SkillIcon;
    public UnityEngine.UI.Image SkillCoolDownImage;
    public List<Sprite> skillicons;

    float saveaimtime;

    private float maxsizex;

    void Start()
    {
        maxsizex = bloodback.rectTransform.sizeDelta.x;
        bloodbarquick.rectTransform.sizeDelta = new Vector2(maxsizex, bloodbarquick.rectTransform.sizeDelta.y);
        bloodbarslow.rectTransform.sizeDelta = new Vector2(maxsizex, bloodbarslow.rectTransform.sizeDelta.y);
        bloodtext.text = "100/100";

        SkillCoolDownImage.gameObject.SetActive(false);
    }


    void Update()
    {
        float sizex=GameDataManager.Instance.health / 100f * maxsizex;
        if(sizex != bloodbarquick.rectTransform.sizeDelta.x)
        {
            UpdateBloodBar();
        }

        SkillIcon.sprite=skillicons[(int)GameDataManager.Instance.playerType-1];
    }

    public void SkillCoolDown(float time)
    {
        SkillCoolDownImage.gameObject.SetActive(true);
        saveaimtime = time;
        SkillCoolDownImage.fillAmount = 1;
        StartCoroutine(Cooldown(time));
    }

    IEnumerator Cooldown(float aimtime)
    {
        while (saveaimtime > 0)
        {
            saveaimtime-=Time.deltaTime;
            SkillCoolDownImage.fillAmount = Mathf.Lerp(0, 1, saveaimtime / aimtime);
            yield return null;
        }
        SkillCoolDownImage.gameObject.SetActive(false);
        saveaimtime = 0;
        SkillCoolDownImage.fillAmount = 0;
    }

    void UpdateBloodBar()
    {
        bloodbarquick.rectTransform.sizeDelta = new Vector2(GameDataManager.Instance.health / 100f * maxsizex, bloodbarquick.rectTransform.sizeDelta.y);
        bloodtext.text=$"{GameDataManager.Instance.health}/100";
        StartCoroutine(Updatebloodbar());
    }

    IEnumerator Updatebloodbar()
    {
        float nowsizex=bloodbarslow.rectTransform.sizeDelta.x;
        float time = 0;
        while(time<fadetime)
        {
            time+=Time.deltaTime;
            float percent = time / fadetime;
            float sizex = Mathf.Lerp(nowsizex, bloodbarquick.rectTransform.sizeDelta.x, percent);
            bloodbarslow.rectTransform.sizeDelta = new Vector2(sizex, bloodbarslow.rectTransform.sizeDelta.y);
            yield return null;
        }
        bloodbarslow.rectTransform.sizeDelta = new Vector2(GameDataManager.Instance.health / 100f * maxsizex, bloodbarslow.rectTransform.sizeDelta.y);
        yield break;
    }
}
