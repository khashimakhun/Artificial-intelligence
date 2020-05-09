using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Rapadura
{

    //[CreateAssetMenu(menuName = "Simple AI/AI Config")]
    public class SimpleAIConfig : ScriptableObject
    {
        public float groundingDistance = 0.1f;
        public LayerMask groundLayer;
        public LayerMask platformLayer;
        public AIBase.AIType AIType = AIBase.AIType.platform;
        public Language.LangList selectedLanguage = Language.LangList.english;
        public LayerMask obistacleLayer;

    }

    [CustomEditor(typeof(SimpleAIConfig))]
    public class SimpleAIConfig_editor : Editor
    {

        SimpleAIConfig gameConfig;

        void OnEnable()
        {
            gameConfig = (SimpleAIConfig)target;
        }

        public override void OnInspectorGUI()
        {
            SimpleAILayout.DrawBanner(gameConfig.selectedLanguage);
            EditorGUILayout.Space();

            SimpleAILayout.showGeneralsArea(gameConfig);
            EditorUtility.SetDirty(gameConfig);
        }
    }
}