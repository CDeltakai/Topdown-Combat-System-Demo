using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

/// <summary>
/// Looks through the build settings in the project and dynamically generates an enum which contains the names of all the
/// scenes in the build. Very useful when dealing with a large number of scenes - allows loading of scenes through enums instead
/// of manually typed strings which is both error-prone and inflexible.
/// </summary>
public class SceneNameGenerator
{
    private const string EnumName = "SceneNames";
    private static readonly string OutputPath = "Assets/Scripts/Enums/" + EnumName + ".cs";

    [MenuItem("Tools/Generate Scene Names Enum")]
    public static void GenerateSceneNames()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum " + EnumName);
        sb.AppendLine("{");

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                //string enumValue = sceneName.Replace(" ", "_"); // Replace spaces with underscores
                sb.AppendLine($"    {sceneName},");
            }
        }

        sb.AppendLine("}");

        File.WriteAllText(OutputPath, sb.ToString());
        AssetDatabase.Refresh();
    }    


}
