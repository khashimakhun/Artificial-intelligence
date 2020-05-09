using System.Collections;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rapadura
{


#if UNITY_EDITOR

    public class SimpleAIWindow : EditorWindow
    {
        static SimpleAIConfig gameConfig;
        static AI _ai;
        static int behaviour = -1;
        Vector2 conditionsScroll = Vector2.zero;
        Vector2 buildMenuScrollPosition2 = Vector2.zero;

        private enum addMoment
        {
            condition, action
        }

        private enum windowMoments
        {
            main, second, error
        }

        static windowMoments moment = windowMoments.main;
        static addMoment secondScreenMoment;

        Color BGSctionColor = new Color(38f / 255f, 38f / 255f, 38f / 255f, 1f);
        Color BG_ErrorColor = new Color(100f / 255f, 38f / 255f, 38f / 255f, 1f);
        Color DivisionColor = new Color(38f / 255f, 38f / 255f, 38f / 255f, 1f);
        Color BG_SecondScreenColor = new Color(38f / 255f, 100f / 255f, 38f / 255f, 1f);

        Rect HeaderSection;
        Texture2D HeaderSectionTexture;
        int headerPadding = 10;

        Rect ConditionalSection;
        Texture2D ConditionalSectionTexture;

        Rect ActionSection;
        Texture2D ActionSectionTexture;

        Rect ErrorSection;
        Rect ErrorIconSection;
        Texture2D ErrorSectionTexture;
        Texture2D ErrorIcon;
        float iconsize = 80;

        static Vector2 windowSize = new Vector2(850, 450);

        #region secondScreen

        Rect SecondScreen;
        Texture2D SecondScreenTexture;

        GUIStyle BorderLassButton;

        Rect backButton;
        float backIconSize = 40;
        float secondScreenIcons = 60;

        Texture2D SecondScreenIconTexture;

        GUISkin secondScreenLayout;

        Rect optionsArea;
        Vector2 optionsAreaScroll = Vector2.zero;

        #endregion


        [MenuItem("Window/SimpleAI")]
        public static void OpenWindow()
        {
            _ai = null;
            behaviour = -1;

            gameConfig = Resources.Load("Configs/GeneralConfigs") as SimpleAIConfig;

            SimpleAIWindow window = (SimpleAIWindow)GetWindow(typeof(SimpleAIWindow));
            window.minSize = windowSize;
            window.Show();
        }


        public static void OpenWindow(AI ai, int behaviour_id)
        {
            _ai = ai;
            behaviour = behaviour_id;
            moment = windowMoments.main;

            gameConfig = Resources.Load("Configs/GeneralConfigs") as SimpleAIConfig;

            SimpleAIWindow window = (SimpleAIWindow)GetWindow(typeof(SimpleAIWindow));
            window.minSize = windowSize;
            window.Show();
        }

        void OnEnable()
        {
            BorderLassButton = new GUIStyle();
            BorderLassButton.border.top = 0;
            gameConfig = Resources.Load("Configs/GeneralConfigs") as SimpleAIConfig;

            secondScreenLayout = Resources.Load<GUISkin>("GUIStyle/FontsSkin");
        }


        //TODO: Aperfeiçoar validação de AI
        void ValidateSelection()
        {

            bool isAAssetObject = false;

            //TODO: COrrigir edição de arquivo direto dos assets
            /*var path = "";
            var obj = Selection.activeObject;
            if (obj == null) path = "Assets";
            else path = AssetDatabase.GetAssetPath(obj.GetGetSingleton()ID());
            if (path.Length > 0)
            {
                if (Directory.Exists(path))
                {
                    Debug.Log("Folder");
                }
                else
                {
                    try
                    {
                        Debug.Log(obj.GetType());
                        //isAAssetObject = true;
                         
                    }catch
                    {
                        isAAssetObject = false;
                    }
                }
            }
            else
            {
                Debug.Log("Not in assets folder");
            }*/



            if (!isAAssetObject && Selection.gameObjects.Length < 1)
            {
                _ai = null;
                behaviour = -1;
                moment = windowMoments.error;
                return;
            }
            else
            {

                if (!isAAssetObject)
                {
                    SimpleAI _simpleAI = Selection.gameObjects[0].GetComponent<SimpleAI>();
                    if (_simpleAI)
                    {
                        _ai = _simpleAI.ai;
                    }
                }


                if (_ai != null)
                {

                    if (moment != windowMoments.second)
                        moment = windowMoments.main;

                    if (_ai.actions.Length > 0 && behaviour < 0)
                    {
                        behaviour = 0;
                    }
                    else if (_ai.actions.Length - 1 < behaviour)
                    {
                        behaviour -= 1;
                    }
                }
                else
                {
                    _ai = null;
                    behaviour = -1;
                    moment = windowMoments.error;
                }
            }

        }

        void initTextures()
        {
            HeaderSectionTexture = new Texture2D(1, 1);
            HeaderSectionTexture.SetPixel(0, 0, BGSctionColor);
            HeaderSectionTexture.Apply();

            ConditionalSectionTexture = new Texture2D(1, 1);
            ConditionalSectionTexture.SetPixel(0, 0, BGSctionColor);
            ConditionalSectionTexture.Apply();

            ActionSectionTexture = new Texture2D(1, 1);
            ActionSectionTexture.SetPixel(0, 0, BGSctionColor);
            ActionSectionTexture.Apply();

            ErrorSectionTexture = new Texture2D(1, 1);
            ErrorSectionTexture.SetPixel(0, 0, BG_ErrorColor);
            ErrorSectionTexture.Apply();

            SecondScreenTexture = new Texture2D(1, 1);
            SecondScreenTexture.SetPixel(0, 0, BG_SecondScreenColor);
            SecondScreenTexture.Apply();

            SecondScreenIconTexture = new Texture2D(1, 1);
            SecondScreenIconTexture.SetPixel(0, 0, Color.white);
            SecondScreenIconTexture.Apply();

            ErrorIcon = Resources.Load<Texture2D>("Icons/no-ai-selected");

        }



        private void OnGUI()
        {
            initTextures();
            ValidateSelection();


            if (moment == windowMoments.error || behaviour < 0) //(moment == windowMoments.error || !_ai || behaviour < 0)
            {
                DrawSelectionError();
                return;
            }
            else if (moment == windowMoments.main)
            {
                DrawLayout();
                DrawHeader();
                DrawConditions();
                DrawActions();
            }
            else if (moment == windowMoments.second)
            {
                DrawSecondScreen();
            }


        }

        private void DrawSelectionError()
        {
            ErrorSection.x = 0;
            ErrorSection.y = 0;
            ErrorSection.width = Screen.width;
            ErrorSection.height = Screen.height;

            ErrorIconSection.x = (ErrorSection.x + ErrorSection.width / 2f) - iconsize / 2;
            ErrorIconSection.y = (ErrorSection.y + ErrorSection.height / 2f) - iconsize / 2;
            ErrorIconSection.width = iconsize;
            ErrorIconSection.height = iconsize;


            GUI.DrawTexture(ErrorSection, ErrorSectionTexture);
            GUI.DrawTexture(ErrorIconSection, ErrorIcon);

            GUILayout.BeginArea(ErrorSection);


            //TODO: traducao
            EditorGUILayout.HelpBox("Select a behaviour of SimpleAI.", MessageType.Warning);

            GUILayout.EndArea();
        }

        private void DrawSecondScreen()
        {
            SecondScreen.x = 0;
            SecondScreen.y = 0;
            SecondScreen.width = Screen.width;
            SecondScreen.height = Screen.height;

            backButton.x = 5;
            backButton.y = 5;
            backButton.width = backIconSize;
            backButton.height = backIconSize;

            optionsArea.x = 10;
            optionsArea.y = 70;
            optionsArea.width = SecondScreen.width - 20;
            optionsArea.height = SecondScreen.height - optionsArea.y - 35;

            GUI.DrawTexture(SecondScreen, SecondScreenTexture);
            GUI.DrawTexture(backButton, SecondScreenIconTexture);

            //Remover
            GUI.DrawTexture(optionsArea, SecondScreenIconTexture);

            GUILayout.BeginArea(SecondScreen);

            if (GUI.Button(backButton, "", BorderLassButton))
            {
                moment = windowMoments.main;
            }

            if (secondScreenMoment == addMoment.condition)
            {
                secondScreenConditions();
            }
            else
            {
                secondScreenActions();
            }
            

            
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            GUILayout.EndArea();

        }


        void secondScreenConditions()
        {
            //TODO: Tradução
            EditorGUILayout.LabelField("Select condition to add", secondScreenLayout.GetStyle("secondHeader"));

            GUILayout.BeginArea(optionsArea);
            optionsAreaScroll = GUILayout.BeginScrollView(optionsAreaScroll);
            EditorGUILayout.BeginVertical();

            foreach (var type in SimpleAI_Condition.typesList)
            {
                if (GUILayout.Button(type.Key))
                {
                    Array.Resize<SimpleAI_Condition>(ref _ai.actions[behaviour].conditions, _ai.actions[behaviour].conditions.Length + 1);
                    _ai.actions[behaviour].conditions[_ai.actions[behaviour].conditions.Length - 1] = (SimpleAI_Condition)type.Value.Clone();
                    _ai.save();
                    moment = windowMoments.main;
                }
            }

            EditorGUILayout.EndVertical();
        }

        void secondScreenActions()
        {
            //TODO: Tradução
            EditorGUILayout.LabelField("Select action to add", secondScreenLayout.GetStyle("secondHeader"));

            GUILayout.BeginArea(optionsArea);
            optionsAreaScroll = GUILayout.BeginScrollView(optionsAreaScroll);
            EditorGUILayout.BeginVertical();

            foreach (var type in SimpleAI_Action.actionsList)
            {
                if (GUILayout.Button(type.Key))
                {
                    Array.Resize<SimpleAI_Action>(ref _ai.actions[behaviour].actions, _ai.actions[behaviour].actions.Length + 1);
                    _ai.actions[behaviour].actions[_ai.actions[behaviour].actions.Length - 1] = (SimpleAI_Action)type.Value.Clone();
                    _ai.save();
                    moment = windowMoments.main;
                }
            }

            EditorGUILayout.EndVertical();
        }


        void DrawLayout()
        {
            HeaderSection.x = 0;
            HeaderSection.y = 0;
            HeaderSection.width = Screen.width - 70;
            HeaderSection.height = 70;

            ConditionalSection.x = 0;
            ConditionalSection.y = HeaderSection.height + 1;
            ConditionalSection.width = Screen.width / 3;
            ConditionalSection.height = Screen.height - (HeaderSection.height + 23);

            ActionSection.x = (Screen.width / 3) + 1;
            ActionSection.y = HeaderSection.height + 1;
            ActionSection.width = ((Screen.width / 3) * 2) + 1;
            ActionSection.height = Screen.height - (HeaderSection.height + 23);

            GUI.DrawTexture(HeaderSection, HeaderSectionTexture);
            GUI.DrawTexture(ConditionalSection, ConditionalSectionTexture);
            GUI.DrawTexture(ActionSection, ActionSectionTexture);
        }

        void DrawHeader()
        {


            Rect HeaderSectionContent = HeaderSection;
            GUI.color = Color.white;

            HeaderSectionContent.y += headerPadding;
            HeaderSectionContent.x += headerPadding;
            HeaderSectionContent.width -= headerPadding * 2;
            HeaderSectionContent.height -= headerPadding;


            Rect saveButtonR = new Rect();
            saveButtonR.y = 0;
            saveButtonR.x = HeaderSection.width;
            saveButtonR.width = 70;
            saveButtonR.height = HeaderSection.height;

            if (GUI.Button(saveButtonR, "Save"))
            {
                _ai.save();
            }

            GUILayout.BeginArea(HeaderSection);

            GUILayout.BeginArea(HeaderSectionContent);

            //Action name
            EditorGUILayout.BeginHorizontal();
            WindowHelpers.drawLabel(Language.nameOfAction(gameConfig.selectedLanguage));
            string wNameHelper = EditorGUILayout.TextField(_ai.actions[behaviour].name);
            if (wNameHelper != _ai.actions[behaviour].name)
            {
                _ai.actions[behaviour].name = wNameHelper;
                _ai.save();
            }
            EditorGUILayout.EndHorizontal();

            //Color picker
            EditorGUILayout.BeginHorizontal();
            WindowHelpers.drawLabel(Language.colorOfAction(gameConfig.selectedLanguage));
            Color wColorHelper = EditorGUILayout.ColorField(_ai.actions[behaviour].color);
            if (wColorHelper != _ai.actions[behaviour].color)
            {
                _ai.actions[behaviour].color = wColorHelper;
                _ai.save();
            }
            EditorGUILayout.EndHorizontal();

            //TODO: Mudar lingua
            EditorGUILayout.BeginHorizontal();
            WindowHelpers.drawLabel("Kind of condition group: ");
            
            if (!_ai.actions[behaviour].isAndGroup)
            {
                if (GUILayout.Button("AND", GUILayout.Width(50)))
                {
                    _ai.actions[behaviour].isAndGroup = true;
                    _ai.save();
                }

                GUILayout.Button("OR", WindowHelpers.pressedButton(), GUILayout.Width(50));
            }
            else
            {
                GUILayout.Button("AND", WindowHelpers.pressedButton(), GUILayout.Width(50));
                if (GUILayout.Button("OR", GUILayout.Width(50)))
                {
                    _ai.actions[behaviour].isAndGroup = false;
                    _ai.save();
                }
            }

            WindowHelpers.drawLabel("    Can it break another behaviour: ", 230);

            bool wPrioritaryHelper = EditorGUILayout.Toggle(_ai.actions[behaviour].prioritary, GUILayout.Width(30));
            if (wPrioritaryHelper != _ai.actions[behaviour].prioritary)
            {
                _ai.actions[behaviour].prioritary = wPrioritaryHelper;
                _ai.save();
            }

            WindowHelpers.drawLabel("  Run until be break: ", 140);

            bool wContinuosRunHelper = EditorGUILayout.Toggle(_ai.actions[behaviour].continuosRun, GUILayout.Width(30));
            if (wContinuosRunHelper != _ai.actions[behaviour].continuosRun)
            {
                _ai.actions[behaviour].continuosRun = wContinuosRunHelper;
                _ai.save();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();


            GUILayout.EndArea();

            GUILayout.EndArea();


        }

        void DrawConditions()
        {
            GUILayout.BeginArea(ConditionalSection);
            conditionsScroll = GUILayout.BeginScrollView(conditionsScroll);
            GUILayout.BeginVertical();


            if (_ai.actions[behaviour].conditions.Length < 1)
            {
                //TODO: Tradução
                EditorGUILayout.HelpBox("No conditions!", MessageType.Warning);
            }
            else
            {
                for (int i = 0; i < _ai.actions[behaviour].conditions.Length; i++)
                {
                    try
                    {

                        EditorGUILayout.BeginVertical("box");

                        _ai.actions[behaviour].conditions[i].drawConditionHeader(_ai.actions[behaviour].conditions[i].getName(), _ai.actions[behaviour].conditions[i].BtnIcon(), _ai, behaviour, i);

                        if (!_ai.actions[behaviour].conditions[i].open)
                        {
                            EditorGUILayout.EndVertical();
                            continue;
                        }

                        _ai.actions[behaviour].conditions[i].drawGUI(_ai, behaviour, i);


                        EditorGUILayout.EndVertical();

                        if (_ai.actions[behaviour].conditions.Length > 1 && i+1 < _ai.actions[behaviour].conditions.Length)
                        {
                            EditorGUILayout.LabelField("-------- " + ((_ai.actions[behaviour].isAndGroup) ? "AND" : "OR") + " --------", secondScreenLayout.GetStyle("conditionDivision"), new GUILayoutOption[] {GUILayout.Height(8) });
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                        continue;
                    }
                }
            }


            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            WindowHelpers.drawLabel("Add condition");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                secondScreenMoment = addMoment.condition;
                moment = windowMoments.second;
            }

            EditorGUILayout.EndHorizontal();


            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }


        void DrawActions()
        {
            GUILayout.BeginArea(ActionSection);
            buildMenuScrollPosition2 = GUILayout.BeginScrollView(buildMenuScrollPosition2);
            GUILayout.BeginVertical();


            if (_ai.actions[behaviour].actions.Length < 1)
            {
                //TODO: Tradução
                EditorGUILayout.HelpBox("No actions!", MessageType.Warning);
            }
            else
            {
                for (int i = 0; i < _ai.actions[behaviour].actions.Length; i++)
                {
                    try
                    {
                        if (i % 2 == 0)
                            EditorGUILayout.BeginVertical("box");
                        else
                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);


                        _ai.actions[behaviour].actions[i].drawActionHeader(_ai.actions[behaviour].actions[i].getName(), _ai.actions[behaviour].actions[i].BtnIcon(), _ai, behaviour, i);

                        if (!_ai.actions[behaviour].actions[i].open)
                        {
                            EditorGUILayout.EndVertical();
                            continue;
                        }
                        

                        _ai.actions[behaviour].actions[i].drawGUI(_ai, behaviour, i);
                        EditorGUILayout.Space();

                        EditorGUILayout.EndVertical();


                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                        continue;
                    }
                }
            }

            


            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            WindowHelpers.drawLabel("Add action");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                secondScreenMoment = addMoment.action;
                moment = windowMoments.second;
            }

            EditorGUILayout.EndHorizontal();
            

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }


    }
#endif
}
