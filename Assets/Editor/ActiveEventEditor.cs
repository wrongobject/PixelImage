using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ActiveEvent))]
public class ActiveEventEditor : Editor
{
    ActiveEvent activeEvent;

    private void OnEnable()
    {
        activeEvent = target as ActiveEvent;
    }
    public override void OnInspectorGUI()
    {
        activeEvent.target = (GameObject)EditorGUILayout.ObjectField("Target",activeEvent.target,typeof(GameObject),true);
    }
}
