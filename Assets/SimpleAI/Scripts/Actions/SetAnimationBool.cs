using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEditor;

namespace Rapadura
{

    public class SetAnimationBool : SimpleAI_Action
    {

        [SerializeField]
        public string name = "Set Animation Bool";
        public string boolName;
        public bool boolValue;

        public override string getName()
        {
            return name;
        }

        public override Texture2D BtnIcon()
        {
            return Resources.Load("Icons/2dsimpleai-play") as Texture2D;
        }

        public override void drawGUI(AI _ai, int actionID, int actionScriptID)
        {

            EditorGUILayout.Space();
            boolName = EditorGUILayout.TextField("Boolean Name", boolName);
            boolValue = EditorGUILayout.Toggle("Value", boolValue);

        }

        public override void fillData(JsonData json)
        {
            open = (bool)json["open"];
            boolName = (string)json["boolName"];
            boolValue = (bool)json["boolValue"];
        }


        public override IEnumerator perform(SimpleAI _simpleAI)
        {

            if (_simpleAI.anim)
            {
                _simpleAI.anim.SetBool(boolName, boolValue);

            }

            yield return true;


        }

    }

}