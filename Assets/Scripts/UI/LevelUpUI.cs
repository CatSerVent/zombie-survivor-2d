using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    [Serializable]
    public class OptionButton
    {
        public GameObject root;
        public Button button;
        public Text title;
        public Text sub;
    }

    public GameObject panel;
    public OptionButton[] buttons;
    public Button rerollButton;
    public Text rerollText;

    List<LevelUpOption> cur;
    Action<LevelUpOption> onPick;
    Action onReroll;

    public void Open(List<LevelUpOption> options, Action<LevelUpOption> onSelect, Action onRerollPressed, bool canReroll)
    {
        cur = options;
        onPick = onSelect;
        onReroll = onRerollPressed;
        if (panel) panel.SetActive(true);
        BindOptions(options);
        BindReroll(canReroll);
        Time.timeScale = 0f;
    }

    public void Refresh(List<LevelUpOption> options, bool canReroll)
    {
        cur = options;
        BindOptions(options);
        BindReroll(canReroll);
    }

    void BindOptions(List<LevelUpOption> options)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            bool active = i < options.Count;
            if (buttons[i].root) buttons[i].root.SetActive(active);
            if (!active) continue;
            var o = options[i];
            if (buttons[i].title) buttons[i].title.text = o.title;
            if (buttons[i].sub) buttons[i].sub.text = o.subtitle;
            int idx = i;
            buttons[i].button.onClick.RemoveAllListeners();
            buttons[i].button.onClick.AddListener(() => Select(idx));
        }
    }

    void BindReroll(bool canReroll)
    {
        if (rerollButton)
        {
            rerollButton.onClick.RemoveAllListeners();
            rerollButton.interactable = canReroll;
            if (rerollText) rerollText.text = canReroll ? "府费 (1/1)" : "府费 (0/1)";
            if (canReroll && onReroll != null) rerollButton.onClick.AddListener(() => onReroll());
        }
    }

    void Select(int idx)
    {
        if (panel) panel.SetActive(false);
        if (onPick != null && cur != null && idx >= 0 && idx < cur.Count) onPick(cur[idx]);
        Time.timeScale = 1f;
    }
}
