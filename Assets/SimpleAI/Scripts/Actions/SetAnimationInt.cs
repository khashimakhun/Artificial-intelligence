using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEditor;

namespace Rapadura
{

    public class SetAnimationInt : SimpleAI_Action
    {

        [SerializeField]
        public string name = "Set Animation Int";
        public string integerName;
        public int integerValue;

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
            integerName = EditorGUILayout.TextField("Integer Name", integerName);
            integerValue = EditorGUILayout.IntField("Value", integerValue);

        }

        public override void fillData(JsonData json)
        {
            open = (bool)json["open"];
            integerName = (string)json["integerName"];
            integerValue = (int)json["integerValue"];
        }


        public override IEnumerator perform(SimpleAI _simpleAI)
        {

            if (_simpleAI.anim)
            {
                _simpleAI.anim.SetInteger(integerName, integerValue);

            }

            yield return true;


        }

    }

}