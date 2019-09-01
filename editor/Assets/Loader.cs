using System.Collections.Generic;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class Loader : MonoBehaviour
{
    public GameObject cubeNode;
    public GameObject sphereNode;

    private Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        lineMaterial = new Material(Shader.Find("Sprites/Default"));

        var stream = CharStreams.fromPath("../examples/fibonacci.g");
        var lexer = new GravisLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new GravisParser(tokens)
        {
            BuildParseTree = true
        };
        var loader = new LoadListener();
        var tree = parser.file_input();
        ParseTreeWalker.Default.Walk(loader, tree);
        var parts = Node.FindIsolatedGraphs(loader.nodes);
        for (var i = 0; i < parts.Count; i++)
        {
            var nodes = parts[i];
            Node.AlignNodes(nodes);
            DrawNodes(nodes, i);
        }
    }
    // https://www.youtube.com/watch?v=pWxucHof_5A Projectile

    // Update is called once per frame
    void Update()
    {
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

    private void DrawNodes(List<Node> nodes, int offset)
    {
        var definitions = new Dictionary<Node, GameObject>();
        var links = new HashSet<(Node from, Node to)> ();
        foreach (var node in nodes)
        {
            definitions[node] = Instantiate(cubeNode, (node.position + new Vector3(0, 0, offset)) * 2, Quaternion.identity);
            foreach (var input_node in node.inputs)
                links.Add((from: input_node, to: node));
            foreach (var output_node in node.outputs)
                links.Add((from: node, to: output_node));
        }
        foreach (var (from, to) in links)
            LineTo(definitions[from], definitions[to]);
    }
}
