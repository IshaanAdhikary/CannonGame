using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public enum DisplayCategory { All, Ref, Bools, Floats }
    public DisplayCategory categoryToDisplay;

    public override void OnInspectorGUI()
    {
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        EditorGUILayout.Space();

        switch (categoryToDisplay)
        {
            case DisplayCategory.All:
                DisplayRef();
                DisplayBools();
                DisplayFloats();
                break;

            case DisplayCategory.Ref:
                DisplayRef();
                break;

            case DisplayCategory.Bools:
                DisplayBools();
                break;

            case DisplayCategory.Floats:
                DisplayFloats();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DisplayRef()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("darkenedScreen"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deadScreen"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pausePanel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arrowSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("powerBarObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pauseAnimator"));
    }
    void DisplayBools()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hasLaunched"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isDead"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isCharging"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isCooldown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isPaused"));
    }
    void DisplayFloats()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("runSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLaunch"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("camOffset"));
    }
}
