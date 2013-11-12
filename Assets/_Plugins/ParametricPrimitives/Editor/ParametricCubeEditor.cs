using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ParametricCube))]
public class ParametricCubeEditor : Editor
{	
	public override void OnInspectorGUI() 
	{
		EditorGUIUtility.LookLikeControls();
		
		ParametricCube primitive = (ParametricCube) target as ParametricCube;

		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Width");
		primitive._width = (float)GUILayout.HorizontalSlider(primitive._width, 0.0f, 100.0f);
		primitive._width = (float)EditorGUILayout.FloatField(primitive._width, GUILayout.Width(70));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Height");
		primitive._height = (float)GUILayout.HorizontalSlider(primitive._height, 0.0f, 100.0f);
		primitive._height = (float)EditorGUILayout.FloatField(primitive._height, GUILayout.Width(70));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Depth");
		primitive._depth = (float)GUILayout.HorizontalSlider(primitive._depth, 0.0f, 100.0f);
		primitive._depth = (float)EditorGUILayout.FloatField(primitive._depth, GUILayout.Width(70));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		primitive._subdivisionsWidth = (int)EditorGUILayout.IntSlider("Width Subdivision", primitive._subdivisionsWidth, 1, 40);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		primitive._subdivisionsHeight = (int)EditorGUILayout.IntSlider("Height Subdivision", primitive._subdivisionsHeight, 1, 40);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		primitive._subdivisionsDepth = (int)EditorGUILayout.IntSlider("Depth Subdivision", primitive._subdivisionsDepth, 1, 40);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		string[] names = {"X axis", "Y axis", "Z axis"};
		int[] sizes = {0,1,2};
		primitive._align = (ParametricPrimitive.eAlign)EditorGUILayout.IntPopup("Alignment", (int)primitive._align, names, sizes);
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		primitive._invertNormal = (bool)EditorGUILayout.Toggle("Invert Normals", primitive._invertNormal);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		Rect resetButton = EditorGUILayout.BeginHorizontal();
		resetButton.x = resetButton.width / 2 - 100;
		resetButton.width = 200;
		resetButton.height = 18;
		
		if (GUI.Button(resetButton, "Reset")) 
		{
			primitive.Reset();
			primitive.ShowMesh();
			GUIUtility.ExitGUI();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		if (GUI.changed) 
		{
			EditorUtility.SetDirty(primitive);
		}
	}
}
