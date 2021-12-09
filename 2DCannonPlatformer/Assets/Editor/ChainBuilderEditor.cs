using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChainBuilderScript))]
public class ChainBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChainBuilderScript myScript = (ChainBuilderScript)target;
        if (GUILayout.Button("Build Chain"))
        {
            myScript.BuildChain();
        }
    }
}
