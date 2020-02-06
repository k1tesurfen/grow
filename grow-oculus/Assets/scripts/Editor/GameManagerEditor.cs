using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gm = (GameManager)target;

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Story"))
        {
            gm.storyTime = true;
            gm.currentGroup = "Story";
        }
        if (GUILayout.Button("NON-Story"))
        {
            gm.storyTime = false;
            gm.currentGroup = "No Story";
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("E1"))
        {
            gm.interactionLS = 0;
            gm.scenarioLS = 0;
            gm.currentCondition = "E1";
        }
        if (GUILayout.Button("E2"))
        {
            gm.interactionLS = 1;
            gm.scenarioLS = 0;
            gm.currentCondition = "E2";
        }
        if (GUILayout.Button("E3"))
        {
            gm.interactionLS = 2;
            gm.scenarioLS = 0;
            gm.currentCondition = "E3";
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(7);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("C1"))
        {
            gm.interactionLS = 0;
            gm.scenarioLS = 1;
            gm.currentCondition = "C1";
        }
        if (GUILayout.Button("C2"))
        {
            gm.interactionLS = 1;
            gm.scenarioLS = 1;
            gm.currentCondition = "C2";
        }
        if (GUILayout.Button("C3"))
        {
            gm.interactionLS = 2;
            gm.scenarioLS = 1;
            gm.currentCondition = "C3";
        }
        GUILayout.EndHorizontal();

    }
}
