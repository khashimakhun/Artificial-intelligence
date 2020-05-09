using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Rapadura
{
#if UNITY_EDITOR
    public class SimpleAILayout
    {

        public static Texture2D iconSpawn = Resources.Load("Icons/2dsimpleai-ai") as Texture2D;
        static Texture2D imgBanner = Resources.Load("Textures/2dsimpleai") as Texture2D;
        static Texture2D iconAI = Resources.Load("Icons/2dsimpleai-ai") as Texture2D;
        static Texture2D iconConfig = Resources.Load("Icons/2dsimpleai-config") as Texture2D;
        static Texture2D iconLanding = Resources.Load("Icons/2dsimpleai-landing") as Texture2D;

        public static void DrawBanner(Language.LangList selectedLanguage)
        {

            EditorGUILayout.Space();
            if (imgBanner)
            {
                Rect r;
                float ih = imgBanner.height;
                float iw = imgBanner.width;
                float result = ih / iw;
                float w = Screen.width;
                result = result * w;
                r = GUILayoutUtility.GetRect(ih, result);
                if (GUI.Button(r, new GUIContent(imgBanner, Language.GetRapaduraTooltip(selectedLanguage))))
                {
                    Application.OpenURL("http://rapadurastudio.com/");
                }
            }
        }


        #region AI

        public static void BehaviourButtons(SimpleAI _simpleAI)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            //TODO: Tradução
            EditorGUILayout.LabelField(new GUIContent("Behaviours", iconAI), EditorStyles.boldLabel);
            if (GUILayout.Button(((_simpleAI.showBehaviour) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showBehaviour = !_simpleAI.showBehaviour;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showBehaviour)
                showBehaviourArea(_simpleAI);
            EditorGUILayout.EndVertical();
        }


        public static void BehaviourButtons(AI _ai, SimpleAIConfig gameConfig)
        {
           
            EditorGUILayout.BeginVertical("Box");

            //TODO: Tradução
            EditorGUILayout.LabelField(new GUIContent("Behaviours", iconAI), EditorStyles.boldLabel);
            showBehaviourArea(_ai, gameConfig);
            EditorGUILayout.EndVertical();
        }


        public static void showBehaviourArea(SimpleAI _simpleAI)
        {

            EditorGUILayout.BeginVertical("Box");


            switch (_simpleAI.gameConfig.AIType)
            {
                case AIBase.AIType.platform:
                    {
                        AIDrawPlatformBehaviours(_simpleAI);
                        break;
                    }
                case AIBase.AIType.topdown:
                    {
                        AIDrawTopdownBehaviours(_simpleAI);
                        break;
                    }
                case AIBase.AIType.tactical:
                    {
                        AIDrawTacticalBehaviours(_simpleAI);
                        break;
                    }
                default:
                    {
                        EditorGUILayout.HelpBox(Language.alertGameMovimentType(_simpleAI.gameConfig.selectedLanguage), MessageType.Warning);
                        EditorGUILayout.Space();
                        break;
                    }
            }


            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        public static void showBehaviourArea(AI _ai, SimpleAIConfig gameConfig)
        {

            EditorGUILayout.BeginVertical("Box");


            switch (gameConfig.AIType)
            {
                case AIBase.AIType.platform:
                    {
                        AIDrawPlatformBehaviours(_ai, gameConfig);
                        break;
                    }
                case AIBase.AIType.topdown:
                    {
                        AIDrawTopdownBehaviours(_ai, gameConfig);
                        break;
                    }
                case AIBase.AIType.tactical:
                    {
                        AIDrawTacticalBehaviours(_ai, gameConfig);
                        break;
                    }
                default:
                    {
                        EditorGUILayout.HelpBox(Language.alertGameMovimentType(gameConfig.selectedLanguage), MessageType.Warning);
                        EditorGUILayout.Space();
                        break;
                    }
            }


            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        public static void AIDrawPlatformBehaviours(SimpleAI _simpleAI)
        {
            BehavioursTemplate(_simpleAI);
        }
        public static void AIDrawPlatformBehaviours(AI _ai, SimpleAIConfig gameConfig)
        {
            BehavioursTemplate(_ai, gameConfig);
        }


        public static void AIDrawTopdownBehaviours(SimpleAI _simpleAI)
        {
            AIDrawTopdownBehaviours(_simpleAI.ai, _simpleAI.gameConfig);
        }
        public static void AIDrawTopdownBehaviours(AI _ai, SimpleAIConfig gameConfig)
        {
            EditorGUILayout.HelpBox(Language.alertNotImplementedYet(gameConfig.selectedLanguage), MessageType.Warning);
            EditorGUILayout.Space();
        }


        public static void AIDrawTacticalBehaviours(SimpleAI _simpleAI)
        {
            AIDrawTacticalBehaviours(_simpleAI.ai, _simpleAI.gameConfig);
        }
        public static void AIDrawTacticalBehaviours(AI _ai, SimpleAIConfig gameConfig)
        {
            EditorGUILayout.HelpBox(Language.alertNotImplementedYet(gameConfig.selectedLanguage), MessageType.Warning);
            EditorGUILayout.Space();
        }





        public static void BehavioursTemplate(SimpleAI _simpleAI)
        {
            EditorGUILayout.Space();
            _simpleAI.aiJson = EditorGUILayout.ObjectField("AI asset", _simpleAI.aiJson, typeof(TextAsset), false) as TextAsset;
            EditorGUILayout.Space();

            BehavioursTemplate(_simpleAI.ai, _simpleAI.gameConfig);
        }
        public static void BehavioursTemplate(AI _ai, SimpleAIConfig gameConfig)
        {

            EditorGUILayout.LabelField(Language.numberOfBehaviours(gameConfig.selectedLanguage), EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                if (_ai.actions.Length > 0)
                {
                    Array.Resize<AIAction>(ref _ai.actions, _ai.actions.Length - 1);
                    _ai.save();
                }


            }

            GUIStyle BoxNumberCenterStyle;
            BoxNumberCenterStyle = new GUIStyle();
            BoxNumberCenterStyle.alignment = TextAnchor.MiddleCenter;
            BoxNumberCenterStyle.fontStyle = FontStyle.Bold;

            GUILayout.Box(_ai.actions.Length.ToString(), BoxNumberCenterStyle);

            if (GUILayout.Button("+"))
            {
                Array.Resize<AIAction>(ref _ai.actions, _ai.actions.Length + 1);
                _ai.actions[_ai.actions.Length - 1] = new AIAction();
                _ai.save();

                SimpleAIWindow.OpenWindow(_ai, _ai.actions.Length);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2f);

            DrawAIActions(_ai, gameConfig);
        }





        public static void DrawAIActions(AI _ai, SimpleAIConfig gameConfig)
        {
            
            for (int i = 0; i < _ai.actions.Length; i++)
            {
                try
                {

                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();

                    Rect r = EditorGUILayout.GetControlRect();
                    r.height = 22;
                    r.y += 1;

                    GUIStyle style = new GUIStyle(GUI.skin.GetStyle("HelpBox"));
                    style.richText = true;
                    style.normal.textColor = _ai.actions[i].color;
                    style.fontSize = 13;

                    EditorGUI.LabelField(r, "<b>"+_ai.actions[i].name+"</b>", style);
                    

                    if (GUILayout.Button("Edit", new GUILayoutOption[] { GUILayout.Width(50), GUILayout.Height(21) }))
                    {
                        SimpleAIWindow.OpenWindow(_ai, i);
                    }
                    
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    _ai.actions[i].relevance = EditorGUILayout.IntSlider("Relevance", _ai.actions[i].relevance, 1, 10);

                    EditorGUILayout.EndVertical();

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    continue;
                }
            }
        }

        #endregion

        #region Generals
        public static void generalsButtons(SimpleAI _simpleAI)
        {
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(Language.generalTitle(_simpleAI.gameConfig.selectedLanguage), iconConfig), EditorStyles.boldLabel);

            if (GUILayout.Button(((_simpleAI.showGenerals) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showGenerals = !_simpleAI.showGenerals;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showGenerals)
                SimpleAILayout.showGeneralsArea(_simpleAI.gameConfig);
            EditorGUILayout.EndVertical();
        }

        public static void showGeneralsArea(SimpleAIConfig gameConfig)
        {

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.HelpBox(Language.alertGeneralIsGlobal(gameConfig.selectedLanguage), MessageType.Warning);

            EditorGUILayout.Space();

            Language.LangList tmpLang = (Language.LangList)EditorGUILayout.EnumPopup(Language.languageLabel(gameConfig.selectedLanguage), gameConfig.selectedLanguage);
            if (tmpLang != Language.LangList.select)
                gameConfig.selectedLanguage = tmpLang;

            //EditorUtility.DisplayDialog("Place Selection On Surface?", "Are you sure you want to place", "Place", "Do Not Place")
            AIBase.AIType AITypeTemp = (AIBase.AIType)EditorGUILayout.EnumPopup(Language.gameMovimentTypeLabel(gameConfig.selectedLanguage), gameConfig.AIType);
            if (AITypeTemp != AIBase.AIType.nothingSelected)
                gameConfig.AIType = AITypeTemp;

            gameConfig.groundingDistance = EditorGUILayout.Slider(new GUIContent(Language.groundVerifierLabel(gameConfig.selectedLanguage), iconLanding, Language.groundVerifierTooltip(gameConfig.selectedLanguage)), gameConfig.groundingDistance, 0.01f, 1);


            gameConfig.groundLayer = EditorTools.LayerMaskField(Language.groundLayerLabel(gameConfig.selectedLanguage), gameConfig.groundLayer);

            gameConfig.platformLayer = EditorTools.LayerMaskField(Language.platformLayerLabel(gameConfig.selectedLanguage), gameConfig.platformLayer);

            //TODO: Tradução
            EditorGUILayout.HelpBox("Layer onde pode estar qualquer objeto, parede e etc que pode interromper a visão do AI alem do chão.", MessageType.Info);
            gameConfig.obistacleLayer = EditorTools.LayerMaskField("Obstacles layer", gameConfig.obistacleLayer);

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

    }

#endif
}