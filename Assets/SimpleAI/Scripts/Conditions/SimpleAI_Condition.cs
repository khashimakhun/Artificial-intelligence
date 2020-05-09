using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;

namespace Rapadura
{

    [System.Serializable]
    public abstract class SimpleAI_Condition : ICloneable
    {


        public static Dictionary<string, SimpleAI_Condition> typesList = new Dictionary<string, SimpleAI_Condition> {
            {
                "Detection", new Detection()
            }
        };

        public bool open = true;

        Texture2D removeIcon = Resources.Load("Icons/remove") as Texture2D;
        Texture2D copyIcon = Resources.Load("Icons/copy") as Texture2D;
        Texture2D closeIcon = Resources.Load("Icons/close") as Texture2D;
        Texture2D openIcon = Resources.Load("Icons/open") as Texture2D;

        public abstract void drawGUI(AI _ai, int actionID, int conditionID);
        public abstract Texture2D BtnIcon();
        public abstract void fillData(JsonData json);
        public abstract bool isSatisfied();

        public virtual void gizmos(SimpleAI simpleAI, Transform transform, Color color) { }
        public virtual void selectedGizmos(SimpleAI simpleAI, Transform transform, Color color) { }
        public virtual void start(SimpleAI _simpleAI, Transform transform) { }
        public virtual void update(SimpleAI _simpleAI, Transform transform) { }

        public abstract string getName();

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void drawConditionHeader(string title, Texture2D icon, AI _ai, int actionID, int conditionID)
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(title, icon));

            //TODO: Implementar o botão de copiar
            /*if (GUILayout.Button(new GUIContent(copyIcon), new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(15) }))
            {
                _ai.save();
            }*/

            if (_ai.actions[actionID].conditions[conditionID].open)
            {
                if (GUILayout.Button(new GUIContent(closeIcon), new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(15) }))
                {
                    _ai.actions[actionID].conditions[conditionID].open = false;
                    _ai.save();
                }
            }
            else
            {
                if (GUILayout.Button(new GUIContent(openIcon), new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(15) }))
                {
                    _ai.actions[actionID].conditions[conditionID].open = true;
                    _ai.save();
                }
            }


            if (GUILayout.Button(new GUIContent(removeIcon), new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(15) }))
            {
                var conditionsTemp = new List<SimpleAI_Condition>(_ai.actions[actionID].conditions);
                conditionsTemp.Remove(_ai.actions[actionID].conditions[conditionID]);
                _ai.actions[actionID].conditions = conditionsTemp.ToArray();

                _ai.save();
            }

            EditorGUILayout.EndHorizontal();

        }

    }

}