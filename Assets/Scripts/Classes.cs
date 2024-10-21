using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Classes : MonoBehaviour
{

}

[System.Serializable]
public class WeaponClass
{
    [Tooltip("贴图")]
    public Sprite spr;
    [Tooltip("后坐力")]
    public float recoil;
    [Tooltip("子弹速度")]
    public float bulletSpd;
    [Tooltip("子弹种类")]
    public int type;
    [Tooltip("枪口位置")]
    public Vector2 firePos;
    [Tooltip("开火特效")]
    public GameObject fireEff;
    [Tooltip("伤害")]
    public float damage;
    [Tooltip("伤害类型 (还没做)")]
    public int damageType;
    [Tooltip("发射时间：射速=60秒/发射时间")]
    public float firing_time;
    [Tooltip("换弹时间 (弹夹式：总时间)(泵动式：装一颗子弹的时间)")]
    public float reloading_time;
    [Tooltip("弹夹容量")]
    public int bullet_count;
    [Tooltip("全自动？")]
    public bool automatic;
    [Tooltip("拉栓时间")]
    public float arm_time;
    [Tooltip("当前弹夹容量（没事别动，这只是运行内存）")]
    public int mag_c;
    [Tooltip("换弹类型（0，弹夹式）（1，泵动式）")]
    public int reload_type;
    [Tooltip("开火音效")]
    public AudioClip fire_audio;
    [Tooltip("随即弹道速度")]
    public float spd_offset;
    [Tooltip("随机弹道角度")]
    public float ang_offset;
    [Tooltip("枪口上台角度（radians）")]
    public float ang_rec;
    [Tooltip("枪口上抬恢复速度")]
    public float rec_acc;
    //[HideInInspector]
    //blaa
    [Tooltip("要延迟开火就勾选这个")]
    public bool delay_fire = false;
        [Tooltip("蓄力/延迟时间")]
        [ShowIf("delay_fire", true)][SerializeField] public float time = 0f;
        [Tooltip("要蓄力就勾选这个")]
        [ShowIf("delay_fire", true)][SerializeField] public bool hold_to_fire = false;
    [Tooltip("双持武器")]
    public bool duo_hold = false;
        [Tooltip("第二武器位置")]
        [ShowIf("duo_hold", true)][SerializeField] public Vector2 Sec_pos = Vector2.zero;
        
}
/* Title : Attribute for show a field if other field is true or false.
 * Author : Anth
*/


/// <summary>
/// Atttribute for show a field if other field is true or false.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public sealed class ShowIfAttribute : PropertyAttribute
{
    public string ConditionalSourceField;
    public bool expectedValue;
    public bool HideInInspector;

    /// <summary>
    /// Create the attribute for show a field x if field y is true or false.
    /// </summary>
    /// <param name="ConditionalSourceField">name of field y type boolean </param>
    /// <param name="expectedValue"> what value should have the field y for show the field x</param>
    /// <param name="HideInInspector"> if should hide in the inspector or only disable</param>
    public ShowIfAttribute(string ConditionalSourceField, bool expectedValue, bool HideInInspector = false)
    {
        this.ConditionalSourceField = ConditionalSourceField;
        this.expectedValue = expectedValue;
        this.HideInInspector = HideInInspector;
    }
}


[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
#if UNITY_EDITOR
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
        bool enabled = GetConditionalSourceField(property, condHAtt);
        GUI.enabled = enabled;

        // if is enable draw the label
        if (enabled)
            EditorGUI.PropertyField(position, property, label, true);
        // if is not enabled but we want not hide it, then draw it disabled
        else if (!condHAtt.HideInInspector)
            EditorGUI.PropertyField(position, property, label, false);
        // else hide it ,dont draw it
        else return;
#endif
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
#if UNITY_EDITOR
        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
        bool enabled = GetConditionalSourceField(property, condHAtt);

        // if is enable draw the label
        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        // if is not enabled but we want not hide it, then draw it disabled
        else
        {
            if (!condHAtt.HideInInspector)
                return EditorGUI.GetPropertyHeight(property, label, false);
            // else hide it
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
#else
        return 0f;
#endif
    }

    /// <summary>
    /// Get if the conditional what expected is true.
    /// </summary>
    /// <param name="property"> is used for get the value of the property and check if return enable true or false </param>
    /// <param name="condHAtt"> is the attribute what contains the values what we need </param>
    /// <returns> only if the field y is same to the value expected return true</returns>
    private bool GetConditionalSourceField(SerializedProperty property, ShowIfAttribute condHAtt)
    {
#if UNITY_EDITOR
        bool enabled = false;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
            if (enabled == condHAtt.expectedValue) enabled = true;
            else enabled = false;
        }
        else
        {
            string warning = "ConditionalHideAttribute: 未找到布尔字段 [" + condHAtt.ConditionalSourceField + "] 在 " + property.propertyPath;
            warning += " 确保正确指定条件字段的名称。";
            Debug.LogWarning(warning);
        }

        return enabled;
#else
        return false;
#endif
    }
}
//What ever classes goes after this thing
