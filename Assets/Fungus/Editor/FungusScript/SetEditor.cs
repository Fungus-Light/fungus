using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fungus.Script
{

	[CustomEditor (typeof(Set))]
	public class SetEditor : FungusCommandEditor 
	{
		public override void DrawCommandInspectorGUI()
		{
			Set t = target as Set;

			FungusScript fungusScript = t.GetFungusScript();
			if (fungusScript == null)
			{
				return;
			}

			FungusVariable variable = SequenceEditor.VariableField(new GUIContent("Variable", "Variable to set"),
			                                                  	   fungusScript,
			                                                  	   t.variable);

			if (variable != t.variable)
			{
				Undo.RecordObject(t, "Set Variable Key");
				t.variable = variable;
			}

			if (t.variable == null)
			{
				return;
			}
			
			List<GUIContent> operatorsList = new List<GUIContent>();
			operatorsList.Add(new GUIContent("="));
			if (variable.GetType() == typeof(BooleanVariable))
			{
				operatorsList.Add(new GUIContent("!"));
			}
			else if (variable.GetType() == typeof(IntegerVariable) ||
			         variable.GetType() == typeof(FloatVariable))
			{
				operatorsList.Add(new GUIContent("+"));
				operatorsList.Add(new GUIContent("-"));
				operatorsList.Add(new GUIContent("*"));
				operatorsList.Add(new GUIContent("/"));
			}
			
			int selectedIndex = 0;
			switch (t.setOperator)
			{
				default:
				case Set.SetOperator.Assign:
					selectedIndex = 0;
					break;
				case Set.SetOperator.Negate:
					selectedIndex = 1;
					break;
				case Set.SetOperator.Add:
					selectedIndex = 1;
					break;
				case Set.SetOperator.Subtract:
					selectedIndex = 2;
					break;
				case Set.SetOperator.Multiply:
					selectedIndex = 3;
					break;
				case Set.SetOperator.Divide:
					selectedIndex = 4;
					break;
			}

			selectedIndex = EditorGUILayout.Popup(new GUIContent("Operator", "Arithmetic operator to use"), selectedIndex, operatorsList.ToArray());
			
			Set.SetOperator setOperator = Set.SetOperator.Assign;
			if (variable.GetType() == typeof(BooleanVariable) || 
			    variable.GetType() == typeof(StringVariable))
			{
				switch (selectedIndex)
				{
				default:
				case 0:
					setOperator = Set.SetOperator.Assign;
					break;
				case 1:
					setOperator = Set.SetOperator.Negate;
					break;
				}
			} 
			else if (variable.GetType() == typeof(IntegerVariable) || 
			         variable.GetType() == typeof(FloatVariable))
			{
				switch (selectedIndex)
				{
				default:
				case 0:
					setOperator = Set.SetOperator.Assign;
					break;
				case 1:
					setOperator = Set.SetOperator.Add;
					break;
				case 2:
					setOperator = Set.SetOperator.Subtract;
					break;
				case 3:
					setOperator = Set.SetOperator.Multiply;
					break;
				case 4:
					setOperator = Set.SetOperator.Divide;
					break;
				}
			}

			if (setOperator != t.setOperator)
			{
				Undo.RecordObject(t, "Set Operator");
				t.setOperator = setOperator;
			}
			
			bool booleanValue = t.booleanData.value;
			int integerValue = t.integerData.value;
			float floatValue = t.floatData.value;
			string stringValue = t.stringData.value;

			if (variable.GetType() == typeof(BooleanVariable))
			{
				booleanValue = EditorGUILayout.Toggle(new GUIContent("Boolean Value", "The boolean value to set the variable with"), booleanValue);
			}
			else if (variable.GetType() == typeof(IntegerVariable))
			{
				integerValue = EditorGUILayout.IntField(new GUIContent("Integer Value", "The integer value to set the variable with"), integerValue);
			}
			else if (variable.GetType() == typeof(FloatVariable))
			{
				floatValue = EditorGUILayout.FloatField(new GUIContent("Float Value", "The float value to set the variable with"), floatValue);
			}
			else if (variable.GetType() == typeof(StringVariable))
			{
				stringValue = EditorGUILayout.TextField(new GUIContent("String Value", "The string value to set the variable with"), stringValue);
			}

			if (booleanValue != t.booleanData.value)
			{
				Undo.RecordObject(t, "Set boolean value");
				t.booleanData.value = booleanValue;
			}
			else if (integerValue != t.integerData.value)
			{
				Undo.RecordObject(t, "Set integer value");
				t.integerData.value = integerValue;
			}
			else if (floatValue != t.floatData.value)
			{
				Undo.RecordObject(t, "Set float value");
				t.floatData.value = floatValue;
			}
			else if (stringValue != t.stringData.value)
			{
				Undo.RecordObject(t, "Set string value");
				t.stringData.value = stringValue;
			}
		}
	}

}
