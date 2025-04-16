using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Color;

public class Color_Identifier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Color DetectSpriteColor(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                // 获取所有必要参数
                Sprite sprite = sr.sprite;
                Texture2D texture = sprite.texture;
                Transform targetTransform = hit.transform;
                
                // 获取物体的实际缩放比例（考虑层级缩放）
                Vector3 lossyScale = targetTransform.lossyScale;
                Vector2 effectiveScale = new Vector2(
                    Mathf.Abs(lossyScale.x),
                    Mathf.Abs(lossyScale.y)
                );

                // 核心坐标转换流程
                Vector2 worldPoint = hit.point;
                Vector2 localPos = targetTransform.InverseTransformPoint(worldPoint);

                // 消除缩放影响（将坐标还原到原始sprite尺寸空间）
                Vector2 unscaledLocalPos = new Vector2(
                    localPos.x / effectiveScale.x,
                    localPos.y / effectiveScale.y
                );

                // 获取sprite的像素基准参数
                float pixelsPerUnit = sprite.pixelsPerUnit;
                Rect spriteRect = sprite.rect;
                Rect textureRect = sprite.textureRect;

                // 计算sprite的实际边界（考虑pivot和缩放）
                Vector2 spriteSizePixels = new Vector2(spriteRect.width, spriteRect.height);
                Vector2 pivotOffsetPixels = new Vector2(
                    sprite.pivot.x - spriteRect.width * 0.5f,
                    sprite.pivot.y - spriteRect.height * 0.5f
                );

                // 转换为以sprite左下角为原点的像素坐标
                Vector2 pixelPos = new Vector2(
                    (unscaledLocalPos.x * pixelsPerUnit) + spriteSizePixels.x * 0.5f + pivotOffsetPixels.x,
                    (unscaledLocalPos.y * pixelsPerUnit) + spriteSizePixels.y * 0.5f + pivotOffsetPixels.y
                );

                // 转换为图集坐标
                int atlasX = Mathf.FloorToInt(pixelPos.x + textureRect.x);
                int atlasY = Mathf.FloorToInt(pixelPos.y + textureRect.y);

                // 边界保护
                atlasX = Mathf.Clamp(atlasX, (int)textureRect.xMin, (int)textureRect.xMax - 1);
                atlasY = Mathf.Clamp(atlasY, (int)textureRect.yMin, (int)textureRect.yMax - 1);

                // 调试输出
                Debug.Log(string.Format(
                    "转换流程:\n" +
                    "World: {0}\n" +
                    "Local: {1}\n" +
                    "Unscaled: {2}\n" +
                    "Pixel: ({3},{4})\n" +
                    "Atlas: ({5},{6})",
                    worldPoint.ToString("F4"),
                    localPos.ToString("F4"),
                    unscaledLocalPos.ToString("F4"),
                    pixelPos.x.ToString("F1"),
                    pixelPos.y.ToString("F1"),
                    atlasX,
                    atlasY
                ));
                Color ans = texture.GetPixel(atlasX, atlasY);
                if(ans.a == 0){
                    ans.a = 1;
                }
                Debug.Log(ans.ToString());

                return ans;
            }
        }
        return Color.clear;
    }
}
