using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentGenerator))]
public class EnvironmentGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnvironmentGenerator eG = (EnvironmentGenerator)target;

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Generate Environment"))
        {
            eG.GenerateEnvironment();
        }
        if(GUILayout.Button("Clear"))
        {
            eG.Clear();
        }
        GUILayout.EndHorizontal();
    }
}
