using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;

public class Fib : MonoBehaviour
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
        var interpreter = new InterpreterListener(this);
        var tree = parser.file_input();
        ParseTreeWalker.Default.Walk(interpreter, tree);
    }
    // https://www.youtube.com/watch?v=pWxucHof_5A Projectile

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject Define(GameObject node, Vector3 position)
    {
        var obj = Instantiate(node, position, Quaternion.identity);
        return obj;
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
}
