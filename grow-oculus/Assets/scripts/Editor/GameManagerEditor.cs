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
        if (GUILayout.Button("0E"))
        {
            gm.interactionLS = 0;
            gm.scenarioLS = 0;
            gm.currentCondition = "0E";
        }
        if (GUILayout.Button("1E"))
        {
            gm.interactionLS = 1;
            gm.scenarioLS = 0;
            gm.currentCondition = "1E";
        }
        if (GUILayout.Button("2E"))
        {
            gm.interactionLS = 2;
            gm.scenarioLS = 0;
            gm.currentCondition = "2E";
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(7);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("0C"))
        {
            gm.interactionLS = 0;
            gm.scenarioLS = 1;
            gm.currentCondition = "0C";
        }
        if (GUILayout.Button("1C"))
        {
            gm.interactionLS = 1;
            gm.scenarioLS = 1;
            gm.currentCondition = "1C";
        }
        if (GUILayout.Button("2C"))
        {
            gm.interactionLS = 2;
            gm.scenarioLS = 1;
            gm.currentCondition = "2C";
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Space(50);

        GUILayout.Label("Please enter your ID (First 2 letter of your father first name, first 2 letter of your mothers birth name, last two numbers of your birth year");
        gm.visiblePlayerName = GUILayout.TextField(gm.visiblePlayerName);

        GUILayout.Space(10);

        gm.visibleRightHanded = GUILayout.Toggle(gm.visibleRightHanded, "Do you throw with your right arm?", (GUIStyle)"Radio");

        GUILayout.Space(10);

        if (GUILayout.Button("Submit"))
        {
            gm.playerName = gm.visiblePlayerName;
            gm.visiblePlayerName = "";

            gm.rightHanded = gm.visibleRightHanded;
            gm.visibleRightHanded = false;
        }

    }
}
