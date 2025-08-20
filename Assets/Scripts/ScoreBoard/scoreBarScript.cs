using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scoreBarScript : MonoBehaviour
{
    private new RectTransform transform;
    public TextMeshProUGUI text;
    public float min_width;
    public int score;
    private float tmp;
    void Awake()
    {
        transform = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmp = transform.sizeDelta.y;
        transform.sizeDelta = new Vector2(min_width + score * 0.5f, tmp);
        text.text = "" + score;
    }
}
