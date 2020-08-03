using UnityEditor;
using UnityEngine;

namespace MetaGoo.Editor
{
    [CustomEditor(typeof(GooController))]
    public class GooControllerEditor : UnityEditor.Editor
    {
        private GooController Goo => (GooController) target;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Remove Biomass"))
            {
                Goo.RemoveBallExternal();
            }
            
            if (GUILayout.Button("Add Biomass"))
            {
                Goo.SpawnBallExternal();
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}