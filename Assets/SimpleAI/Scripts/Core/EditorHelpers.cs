using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rapadura
{

#if UNITY_EDITOR

    public class WindowHelpers
    {

        public static Color editorColor = Color.white;

        public static void drawLabel(string t)
        {

            drawLabel(t, 160);
        }

        public static void drawLabel(string t, int size)
        {

            GUIStyle myStyle = new GUIStyle();
            myStyle.normal.textColor = editorColor;
            myStyle.fontSize = 13;


            EditorGUILayout.LabelField(t, myStyle, GUILayout.Width(size));

        }

        public static GUIStyle pressedButton()
        {
            GUIStyle BTNStyle = new GUIStyle("Button");
            BTNStyle.normal = BTNStyle.onActive;

            return BTNStyle;
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

    }


    public class EditorTools
    {

        static List<string> layers;
        static string[] layerNames;


        public static GUIStyle BorderLassButton()
        {
            GUIStyle BorderLassButton;
            BorderLassButton = new GUIStyle();
            BorderLassButton.border.top = 0;
            BorderLassButton.alignment = TextAnchor.MiddleRight;
            BorderLassButton.normal.textColor = Color.red;

            return BorderLassButton;
        }
        
        public static LayerMask LayerMaskField(string label, LayerMask selected)
        {

            if (layers == null)
            {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else
            {
                layers.Clear();
            }

            int emptyLayers = 0;
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {

                    for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                    layers.Add(layerName);
                }
                else
                {
                    emptyLayers++;
                }
            }

            if (layerNames.Length != layers.Count)
            {
                layerNames = new string[layers.Count];
            }
            for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];

            selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);

            return selected;
        }

        public static void DrawDivision()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static void DrawProgress(Rect rect, float progress)
        {
            Texture2D progressBackground = Resources.Load("Textures/healthBarBG") as Texture2D;
            Texture2D progressForground = Resources.Load(((progress >= 0.3f) ? "Textures/healthBarFG" : "Textures/healthBarFG2")) as Texture2D;

            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, rect.height), progressBackground);
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * progress, rect.height), progressForground);
        }

        public static GUIStyle getColorStyle(Color c)
        {
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.normal.textColor = c;
            myFoldoutStyle.onNormal.textColor = c;
            myFoldoutStyle.hover.textColor = c;
            myFoldoutStyle.onHover.textColor = c;
            myFoldoutStyle.focused.textColor = c;
            myFoldoutStyle.onFocused.textColor = c;
            myFoldoutStyle.active.textColor = c;
            myFoldoutStyle.onActive.textColor = c;

            myFoldoutStyle.fontSize = 13;

            return myFoldoutStyle;

        }

    }

#endif

}