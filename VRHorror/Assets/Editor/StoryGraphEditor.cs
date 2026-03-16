using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class StoryGraphEditor : EditorWindow
{
    private Vector2 canvasScroll;
    private List<StoryNode> nodes = new List<StoryNode>();
    private Dictionary<StoryNode, Rect> nodeRects = new Dictionary<StoryNode, Rect>();

    [MenuItem("Tools/Story Graph Editor")]
    public static void ShowWindow()
    {
        GetWindow<StoryGraphEditor>("Story Graph Editor");
    }

    private void OnEnable()
    {
        RefreshNodes();
        ArrangeNodes();
    }

    private void OnFocus()
    {
        RefreshNodes();
        ArrangeNodes();
    }

    void RefreshNodes()
    {
        nodes.Clear();
        nodeRects.Clear();

        string[] guids = AssetDatabase.FindAssets("t:StoryNode");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            StoryNode node = AssetDatabase.LoadAssetAtPath<StoryNode>(path);
            if (node != null)
                nodes.Add(node);
        }
    }

    void ArrangeNodes()
    {
        float xStart = 50;
        float yStart = 50;
        float yStep = 250;
        float xStep = 350;
        float maxColumnHeight = 1000;

        float x = xStart;
        float y = yStart;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].editorPosition == Vector2.zero)
            {
                nodes[i].editorPosition = new Vector2(x, y);
                y += yStep;

                if (y > maxColumnHeight)
                {
                    y = yStart;
                    x += xStep;
                }

                if (!Application.isPlaying)
                    EditorUtility.SetDirty(nodes[i]);
            }
        }
    }

    private void OnGUI()
    {
        canvasScroll = EditorGUILayout.BeginScrollView(canvasScroll, true, true);

        BeginWindows();
        nodeRects.Clear();

        for (int i = 0; i < nodes.Count; i++)
        {
            StoryNode node = nodes[i];

            float nodeHeight = CalculateNodeHeight(node);

            Rect nodeRect = new Rect(node.editorPosition.x, node.editorPosition.y, 300, nodeHeight);
            nodeRect = GUI.Window(i, nodeRect, DrawNodeWindow, node.name);
            nodeRects[node] = nodeRect;

            node.editorPosition = nodeRect.position;
            node.editorPosition.y = Mathf.Max(node.editorPosition.y, 25);
            node.editorPosition.x = Mathf.Max(node.editorPosition.x, 0);

            if (!Application.isPlaying)
                EditorUtility.SetDirty(node);
        }

        EndWindows();

        DrawConnections();

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Nodes"))
            RefreshNodes();
        if (GUILayout.Button("Create Node"))
            CreateNode();
        EditorGUILayout.EndHorizontal();
    }

    float CalculateNodeHeight(StoryNode node)
    {
        float height = 25; 

        height += 20 + node.enterActions.Count * 30;
        height += 25; 
     
        height += 20 + node.exitActions.Count * 30;
        height += 25; 

        height += 20 + node.transitions.Count * 30;
        height += 25; 

        height += 5; 

        return Mathf.Max(height, 180);
    }

    void DrawNodeWindow(int id)
    {
        StoryNode node = nodes[id];

        GUI.DragWindow(new Rect(0, 0, 300, 25));

        EditorGUILayout.LabelField("Enter Actions", EditorStyles.boldLabel);
        for (int i = 0; i < node.enterActions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            node.enterActions[i] = (StoryAction)EditorGUILayout.EnumPopup(node.enterActions[i]);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                node.enterActions.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Enter Action"))
            node.enterActions.Add(StoryAction.None);

        EditorGUILayout.LabelField("Exit Actions", EditorStyles.boldLabel);
        for (int i = 0; i < node.exitActions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            node.exitActions[i] = (StoryAction)EditorGUILayout.EnumPopup(node.exitActions[i]);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                node.exitActions.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Exit Action"))
            node.exitActions.Add(StoryAction.None);

        EditorGUILayout.LabelField("Transitions", EditorStyles.boldLabel);
        for (int i = 0; i < node.transitions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            node.transitions[i].triggerEvent = (StoryEvent)EditorGUILayout.EnumPopup(node.transitions[i].triggerEvent);
            node.transitions[i].nextNode = (StoryNode)EditorGUILayout.ObjectField(node.transitions[i].nextNode, typeof(StoryNode), false);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                node.transitions.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add Transition"))
            node.transitions.Add(new StoryTransition());
    }

    void DrawConnections()
    {
        Handles.color = Color.white;

        foreach (var node in nodes)
        {
            if (!nodeRects.ContainsKey(node)) continue;
            Rect fromRect = nodeRects[node];

            foreach (var trans in node.transitions)
            {
                if (trans.nextNode == null || !nodeRects.ContainsKey(trans.nextNode)) continue;
                Rect toRect = nodeRects[trans.nextNode];

                Vector3 start = new Vector3(fromRect.xMax, fromRect.center.y);
                Vector3 end = new Vector3(toRect.xMin, toRect.center.y);

                Handles.DrawBezier(
                    start,
                    end,
                    start + Vector3.right * 50,
                    end + Vector3.left * 50,
                    Color.white,
                    null,
                    2f
                );

                Handles.DrawSolidDisc(end, Vector3.forward, 5f);
            }
        }
    }

    void CreateNode()
    {
        StoryNode node = ScriptableObject.CreateInstance<StoryNode>();
        node.name = "NewStoryNode";

        string folderPath = "Assets/StoryNodes";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets", "StoryNodes");

        string path = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewStoryNode.asset");
        AssetDatabase.CreateAsset(node, path);
        AssetDatabase.SaveAssets();

        RefreshNodes();
        ArrangeNodes();
    }
}
