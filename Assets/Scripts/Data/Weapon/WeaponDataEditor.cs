#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponData))]
[CanEditMultipleObjects]
public class WeaponDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;
        WeaponType currentType = (WeaponType)serializedObject.FindProperty("loaiVuKhi").enumValueIndex;

        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;
            string pName = property.name;

            if (currentType != WeaponType.vuKhiTamXa)
            {
                if (pName == "bulletPrefab" || pName == "tocDoBayCuaDan" ||
                    pName == "xuyenThau" || pName == "soLuongDan" || pName == "gocToaDan")
                {
                    continue;
                }
            }
            if (currentType == WeaponType.vuKhiTamXa)
            {
                if (pName == "doDaiVuKhi" || pName == "overshoot" ||
                    pName == "gocChem" || pName == "tocDoXoay" || pName == "tocDoDam")
                {
                    continue;
                }
            }
            EditorGUILayout.PropertyField(property, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif