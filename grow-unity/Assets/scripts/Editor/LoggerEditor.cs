using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Logger))]
public class LoggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Path for Logs"))
        {
            Logger logger = (Logger)target;
            logger.SetPath(EditorUtility.OpenFolderPanel("Select path to save logs", "", ""));
        }
    }
}
