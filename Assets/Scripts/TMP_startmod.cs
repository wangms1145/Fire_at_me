using UnityEngine;
using TMPro;

public class FadeInOutText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float waitBeforeAppear = 1f;   // 前摇等待
    public float fadeInDuration = 2f;     // 淡入时间
    public float fadeOutDuration = 2f;    // 淡出时间
    public float flashSpeed = 2f;         // 后续闪烁速度

    private float timer = 0f;
    private bool fadeInDone = false;
    private bool fadeOutDone = false;

    void Update()
    {
        if (tmpText == null) return;

        timer += Time.deltaTime;

        if (timer < waitBeforeAppear)
        {
            // 前摇等待
            SetAlpha(0f);
        }
        else if (!fadeInDone)
        {
            // 淡入
            float alpha = (timer - waitBeforeAppear) / fadeInDuration;
            SetAlpha(alpha);
            if (alpha >= 1f) fadeInDone = true;
        }
        else if (!fadeOutDone)
        {
            // 淡出
            float alpha = 1f - ((timer - waitBeforeAppear - fadeInDuration) / fadeOutDuration);
            SetAlpha(alpha);
            if (alpha <= 0f)
            {
                fadeOutDone = true;
                timer = waitBeforeAppear; // 可以重置循环
            }
        }
        else
        {
            // 循环闪烁
            float alpha = 0.5f + 0.5f * Mathf.Sin((timer - waitBeforeAppear) * flashSpeed * Mathf.PI * 2f);
            SetAlpha(alpha);
        }
    }

    void SetAlpha(float a)
    {
        Color c = tmpText.color;
        c.a = Mathf.Clamp01(a);
        tmpText.color = c;
    }
}
