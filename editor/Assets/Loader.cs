using System.Collections.Generic;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class Loader : MonoBehaviour
{
    public string filePath;
    public GameObject cubeNode;
    public GameObject sphereNode;

    Volume _volume;
    public LoadListener loader;
    private Material lineMaterial;

    private void Start()
    {
        _volume = new Volume();
    }

    public void Load()
    {
        //add with LegendEvils
        DragonEventHandler.sInstance.loaded = true;
        //--------------------

        lineMaterial = new Material(Shader.Find("Sprites/Default"));

        var stream = CharStreams.fromPath(filePath);

        //add with LegendEvils
        DragonEventHandler.sInstance.selectedGStream = stream.ToString();
        //--------------------

        var lexer = new GravisLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new GravisParser(tokens)
        {
            BuildParseTree = true
        };
        loader = new LoadListener();
        var tree = parser.file_input();
        ParseTreeWalker.Default.Walk(loader, tree);
        
        RelinkSubspaces(loader.subspaces, loader.nodes);

        var parts = Node.FindIsolatedGraphs(loader.nodes);
        for (var i = 0; i < parts.Count; i++)
        {
            var nodes = parts[i];
            Node.AlignNodesByForceDirected(nodes);
            DrawNodes(nodes, i, _volume);
        }

        var orbit = Camera.main.GetComponent<DragMouseOrbit>();
        orbit.target = _volume.GetCenter();
        orbit.distance = _volume.GetRadius() * 2;
    }

    public void LineTo(GameObject start, GameObject stop) {
        var lineRenderer = start.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = start.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.widthMultiplier = 0.15f;
            lineRenderer.positionCount = 0;
        }
        lineRenderer.positionCount += 2;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, start.transform.position);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, stop.transform.position);
    }

    private void DrawNodes(List<Node> nodes, int offset, Volume volume)
    {
        var definitions = new Dictionary<Node, GameObject>();
        var links = new HashSet<(Node from, Node to)> ();
        var minVolume = Vector3.zero;
        var maxVolume = Vector3.zero;
        foreach (var node in nodes)
        {
            var position = (node.position + new Vector3(0, 0, offset)) * 2;
            position.y = -position.y;
            var gameObject = Instantiate(cubeNode, position, Quaternion.identity);
            volume.Add(gameObject);

            //add with LegendEvils
            gameObject.GetComponent<CubeScript>()._node = node;
            //--------------------

            var textMesh = gameObject.GetComponentInChildren<TextMesh>();
            textMesh.text = node.text;
            definitions[node] = gameObject;
            foreach (var input_node in node.inputs)
            {
                links.Add((from: input_node, to: node));
                //Debug.Log("input_node = " + input_node.text + " node = " + node.text);
            }
            foreach (var output_node in node.outputs)
            {
                links.Add((from: node, to: output_node));
                //Debug.Log("node = " + node.text + " output_node = " + output_node.text);
            }
        }
        foreach (var (from, to) in links)
        {
            LineTo(definitions[from], definitions[to]);
        }
    }

    private static void RelinkSubspaces(List<Subspace> subspaces, List<Node> nodes)
    {
        foreach (var subspace in subspaces)
        {
            foreach (var input_node in subspace.node.inputs)
            {
                input_node.outputs.Clear();
                input_node.outputs.Add(subspace.input);
                subspace.input.inputs.Add(input_node);
                subspace.input.inputs.Remove(subspace.node);
            }
            foreach (var output_node in subspace.node.outputs)
            {
                output_node.inputs.Clear();
                output_node.inputs.Add(subspace.output);
                subspace.output.outputs.Add(output_node);
                subspace.output.outputs.Remove(subspace.node);
            }
            nodes.Remove(subspace.node);
        }
    }
}

class Volume
{
    public Vector3 Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    public Vector3 Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

    public void Add(GameObject gameObject)
    {
        var position = gameObject.transform.position;

        if (Min.x > position.x)
            Min.x = position.x;
        if (Min.y > position.y)
            Min.y = position.y;
        if (Min.z > position.z)
            Min.z = position.z;

        if (Max.x < position.x)
            Max.x = position.x;
        if (Max.y < position.y)
            Max.y = position.y;
        if (Max.z < position.z)
            Max.z = position.z;
    }

    public Vector3 GetCenter()
    {
        return (Max + Min) / 2;
    }

    public float GetRadius()
    {
        return ((Max - Min) / 2).magnitude;
    }
}