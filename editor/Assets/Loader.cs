using System.Collections.Generic;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class Loader : MonoBehaviour
{
    public string filePath;

    public async void Load()
    {
        var stream = CharStreams.fromPath(filePath);
        var lexer = new GravisLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new GravisParser(tokens)
        {
            BuildParseTree = true
        };
        var loader = new LoadListener();
        var tree = parser.file_input();
        ParseTreeWalker.Default.Walk(loader, tree);

        RelinkSubspaces(loader.subspaces, loader.nodes);

        await GraphManager.Get().Init(loader.nodes);
    }

    private static void RelinkSubspaces(List<Subspace> subspaces, List<Node> nodes)
    {
        foreach (var subspace in subspaces)
        {
            foreach (var input_node in subspace.node.inputs)
            {
                subspace.input.inputs.AddRange(input_node.outputs);
                input_node.inputs.Add(subspace.node);
            }
            nodes.Remove(subspace.node);
        }
    }
}
