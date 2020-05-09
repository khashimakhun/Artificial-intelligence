using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEditor;

namespace Rapadura
{

    public class Move : SimpleAI_Action
    {

        public enum eDirection
        {
            right, left, random
        }

        [SerializeField]
        public string name = "Move";
        public eDirection direction = eDirection.right;
        public double distance = 0.1f;

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


            //TODO: Tradução
            direction = (eDirection)EditorGUILayout.EnumPopup("Direction", direction);
            float distanceHelper = EditorGUILayout.FloatField("Distance", (float)distance);
            if (distanceHelper >= 0)
                distance = distanceHelper;

        }

        public override void fillData(JsonData json)
        {
            open = (bool)json["open"];
            direction = (eDirection)(int)json["direction"];
            distance = (double)json["distance"];
        }

        //TODO: mover para dentro do tipo de movimento atual
        int[] directionsAvaliable = new int[2] { -1, 1 };

        public override IEnumerator perform(SimpleAI _simpleAI)
        {
            int choosedDirection;

            if (direction == eDirection.left)
            {
                choosedDirection = -1;
            }
            else if (direction == eDirection.right)
            {
                choosedDirection = 1;
            }
            else
            {

                System.Random rd = new System.Random();
                int randomIndex = rd.Next(0, 2);
                choosedDirection = directionsAvaliable[randomIndex];
            }

            double tmpDistance = distance;
            bool haveJustFliped = false;

            while (tmpDistance > 0)
            {
                if (!_simpleAI.target && (_simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.both || _simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.distanceToSpawnArea)) {

                    float distPercent = ((Vector2.Distance(_simpleAI.transform.position, _simpleAI.spawnPoint) * 100) / _simpleAI.maxDistanceFromSpawn) / 100;
                    if (distPercent >= 0.8f)
                    {
                        if (!haveJustFliped)
                        {
                            haveJustFliped = true;
                            choosedDirection *= -1;
                        }
                    } else
                    {
                        haveJustFliped = false;
                    }
                }

                _simpleAI._controller.Move(choosedDirection);
                tmpDistance -= Time.deltaTime;
                yield return false;
            }
           

        }


    }

}