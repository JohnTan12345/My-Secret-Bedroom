using UnityEditor;

public class MenuTemplateEditor
{
    [MenuItem("Assets/Create/Core Game Mechanics/Game Interactable")]
    public static void CreateGameplayBehaviour()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            "Assets/Editor/Templates/NewGameInteractable.cs.txt",
            "NewGameInteractable.cs"
        );
    }
}
