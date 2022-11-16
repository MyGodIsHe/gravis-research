using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Constants;
using UI.Selection;

public class Loader : MonoBehaviour
{
    private static readonly string TemporaryFile = "Temp" + SerializationConstants.FileExtension;
    
    public string filePath;

    public async Task LoadFromPath(string path)
    {
        filePath = path;
        await Load();
    }

    public async Task LoadFromString(string source)
    {
        var file = Application.persistentDataPath + TemporaryFile;
        
        var writer = File.CreateText(file);
        await using (writer)
        {
            await writer.WriteAsync(source);
        }
    }

    public async Task LoadEmpty()
    {
        var gm = GraphManager.Get();

        var type = NodeType.Input;
        var text = "I";
        
        var node = new Node
        {
            type = type,
            text = text
        };

        var list = new List<Node>();
        list.Add(node);
        
        await gm.Init(list, true, false);
    }

    public async Task Load()
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
