using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using LitJson;
using System;

namespace Rapadura
{

    [System.Serializable]
    public class AIAction
    {
        //TODO: Implementar relevancia
        public int relevance = 10;

        public AIBase.MomentAction moment = AIBase.MomentAction.idle;
        public bool open = true;
        public string name = "New Behaviour";
        public Color32 color = Color.black;
        public bool isAndGroup = true;
        public bool prioritary = false;
        public bool continuosRun = false;

        [SerializeField]
        public SimpleAI_Condition[] conditions = { };
        public SimpleAI_Action[] actions = { };

    }
    
    public class AI
    {

        string path;
        JsonData json;

        [SerializeField]
        public AIAction[] actions = { };


        public AI(string p)
        {
            path = p;
        }



        public void save()
        {
            json = JsonMapper.ToJson(this);
            File.WriteAllText(path, json.ToString());
            AssetDatabase.Refresh();
        }

        public void load(TextAsset jFile)
        {
            json = JsonMapper.ToObject(jFile.ToString());

            laodActions();
        }

        public void load()
        {
            string jsonString = File.ReadAllText(path);
            if (!jsonString.Equals(""))
            {
                json = JsonMapper.ToObject(jsonString);
            }

            laodActions();

        }


        void laodActions()
        {
            for (int i = 0; i < json["actions"].Count; i++)
            {
                addAction(json["actions"][i]);
            }
        }

        void addAction(JsonData action)
        {
            Array.Resize<AIAction>(ref actions, actions.Length + 1);

            int actionID = actions.Length - 1;
            actions[actionID] = new AIAction();

            actions[actionID].relevance = (int)action["relevance"];
            actions[actionID].name = action["name"].ToString();
            actions[actionID].open = (bool)action["open"];
            actions[actionID].color = new Color((int)action["color"]["r"] / 255f, (int)action["color"]["g"] / 255f, (int)action["color"]["b"] / 255f, (int)action["color"]["a"] / 255f);
            actions[actionID].isAndGroup = (bool)action["isAndGroup"];
            actions[actionID].prioritary = (bool)action["prioritary"];
            actions[actionID].continuosRun = (bool)action["continuosRun"];

            addConditions(action["conditions"], actionID);
            addActionExecutions(action["actions"], actionID);
        }



        void addConditions(JsonData conditions, int actionID)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                Array.Resize<SimpleAI_Condition>(ref actions[actionID].conditions, actions[actionID].conditions.Length + 1);
                actions[actionID].conditions[actions[actionID].conditions.Length - 1] = (SimpleAI_Condition) SimpleAI_Condition.typesList[conditions[i]["name"].ToString()].Clone();

                actions[actionID].conditions[actions[actionID].conditions.Length - 1].fillData(conditions[i]);
            }
        }


        void addActionExecutions(JsonData actionExecution, int actionID)
        {
            for (int i = 0; i < actionExecution.Count; i++)
            {
                Array.Resize<SimpleAI_Action>(ref actions[actionID].actions, actions[actionID].actions.Length + 1);
                actions[actionID].actions[actions[actionID].actions.Length - 1] = (SimpleAI_Action)SimpleAI_Action.actionsList[actionExecution[i]["name"].ToString()].Clone();

                actions[actionID].actions[actions[actionID].actions.Length - 1].fillData(actionExecution[i]);
            }
        }


        [MenuItem("Assets/Create/Simple AI/AI")]
        public static void create()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Your AI", "ActorName.json", "json", "Please select file name to save AI to:");

            AI _ai = new AI(path);

            Array.Resize<AIAction>(ref _ai.actions, _ai.actions.Length + 1);
            _ai.actions[_ai.actions.Length - 1] = new AIAction();

            Array.Resize<AIAction>(ref _ai.actions, _ai.actions.Length + 1);
            _ai.actions[_ai.actions.Length - 1] = new AIAction();


            Array.Resize<SimpleAI_Condition>(ref _ai.actions[0].conditions, _ai.actions[0].conditions.Length + 1);
            _ai.actions[0].conditions[0] = new Detection();

            _ai.save();

        }

        [MenuItem("Assets/Create/Simple AI/Load AI")]
        public static void loadAI()
        {

            string path = Application.dataPath + "/ActorName.json";

            AI _ai = new AI(path);
            _ai.load();

        }

    }

}
