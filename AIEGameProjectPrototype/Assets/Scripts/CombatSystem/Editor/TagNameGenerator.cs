using UnityEditor;
using System.IO;
using System.Text;


/// <summary>
/// Looks through all the tags in the project and dynamically generates an enum which contains the names of all the
/// tags in the project. Allows comparing of tags using enums rather than manually typed strings, which is both 
/// error-prone and inflexible.
/// </summary>
public class TagNameGenerator
{
    private const string EnumName = "TagNames";
    private static readonly string OutputPath = "Assets/Scripts/Enums/" + EnumName + ".cs";

    [MenuItem("Tools/Generate Tag Names Enum")]
    public static void GenerateTagNamesEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum " + EnumName);
        sb.AppendLine("{");

        foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            sb.AppendLine($"    {tag},");
        }

        sb.AppendLine("}");

        File.WriteAllText(OutputPath, sb.ToString());
        AssetDatabase.Refresh();
    }
}