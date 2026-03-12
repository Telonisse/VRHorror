using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoryGraphWindow : EditorWindow
{
    List<Rect> nodes = new List<Rect>();
    List<StoryNode> storyNodes = new List<StoryNode>();

    [MenuItem("Tools/Story Graph")]
    public static void OpenWindow()
    {
        GetWindow<StoryGraphWindow>("Story Graph");
    }

    void OnEnable()
    {
        storyNodes.Clear();

        string[] guids = AssetDatabase.FindAssets("t:StoryNode");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StoryNode node = AssetDatabase.LoadAssetAtPath<StoryNode>(path);

            storyNodes.Add(node);
            nodes.Add(new Rect(Random.Range(50, 500), Random.Range(50, 400), 200, 100));
        }
    }

    private void OnGUI()
    {
        BeginWindows();


        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i] = GUI.Window(i, nodes[i], DrawNodeWindow, storyNodes[i].nodeName);
        }

        if (GUILayout.Button("Create Node"))
        {
            CreateNode();
        }
        EndWindows();

        foreach (var node in storyNodes)
        {
            if (node.NextNodes == null) continue;
            int fromIndex = storyNodes.IndexOf(node);

            foreach (var next in node.NextNodes)
            {
                int toIndex = storyNodes.IndexOf(next);

                if (toIndex == -1) continue;

                DrawConnection(nodes[fromIndex], nodes[toIndex]);
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        GUILayout.Label("Trigger:");
        GUILayout.Label(storyNodes[id].triggerEvent.ToString());

        GUI.DragWindow();
    }

    void DrawConnection(Rect from, Rect to)
    {
        Vector3 start = new Vector3(from.xMax, from.center.y);
        Vector3 end = new Vector3(to.xMin, to.center.y);

        Handles.DrawBezier(
            start,
            end,
            start + Vector3.right * 50,
            end + Vector3.left * 50,
            Color.white,
            null,
            2f
            );
    }

    void CreateNode()
    {
        StoryNode node = ScriptableObject.CreateInstance<StoryNode>();

        string path = "Assets/NewStoryNode.asset";

        AssetDatabase.CreateAsset(node, AssetDatabase.GenerateUniqueAssetPath(path));

        AssetDatabase.SaveAssets();
    }
}
