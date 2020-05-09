using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEditor;

namespace Rapadura
{

    public class Idle : SimpleAI_Action
    {

        [SerializeField]
        public string name = "Idle";

        public override string getName()
        {
            return name;
        }

        public override Texture2D BtnIcon()
        {
            return Resources.Load("Icons/2dsimpleai-ai") as Texture2D;
        }

        public override void drawGUI(AI _ai, int actionID, int actionScriptID)
        {

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(name);
            
        }
        
        public override void fillData(JsonData json)
        {
            open = (bool)json["open"];
        }


        public override IEnumerator perform(SimpleAI _simpleAI)
        {

            yield return new WaitForSeconds(2);


        }

    }

}