using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class Saver
{
    StreamWriter file;
    int varIndex;
    Dictionary<Node, string> definitions = new();
    Dictionary<Node, List<Node>> subspaces = new();

    public Saver(StreamWriter file)
    {
        this.file = file;
    }
    public async Task Write(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.type == NodeType.Subspace || node.subspace == null)
                continue;
            if (!subspaces.ContainsKey(node.subspace.node))
            {
                subspaces[node.subspace.node] = new();
            }
            subspaces[node.subspace.node].Add(node);
        }
        foreach (var pair in subspaces)
        {
            varIndex++;
            await file.WriteAsync($"subspace{varIndex} = subspace [\n");
            definitions[pair.Key] = $"subspace{varIndex}";

            foreach (var node in pair.Value)
            {
                await writeDef(node);
            }

            foreach (var node in pair.Value)
            {
                await writeLink(node);
            }

            await file.WriteAsync($"]\n");
        }
        foreach (var node in nodes)
        {
            if (node.type == NodeType.Subspace || node.subspace != null)
                continue;
            await writeDef(node);
        }

        foreach (var node in nodes)
        {
            if (node.subspace != null)
                continue;
            await writeLink(node);
        }
    }

    async Task writeLink(Node node)
    {
        if (node.type == NodeType.If)
        {
            var src = definitions[node] + ".true";
            foreach (var otherNode in node.trueOutputs)
            {
                await file.WriteAsync($"{src} >> {definitions[otherNode]}\n");
            }
            src = definitions[node] + ".false";
            foreach (var otherNode in node.falseOutputs)
            {
                await file.WriteAsync($"{src} >> {definitions[otherNode]}\n");
            }
        }
        else
        {
            foreach (var otherNode in node.trueOutputs)
            {
                await file.WriteAsync($"{definitions[node]} >> {definitions[otherNode]}\n");
            }
        }
    }

    async Task writeDef(Node node)
    {
        varIndex++;
        switch (node.type)
        {
            case NodeType.Input:
                await file.WriteAsync($"input{varIndex} = input\n");
                definitions[node] = $"input{varIndex}";
                break;
            case NodeType.Output:
                await file.WriteAsync($"output{varIndex} = output\n");
                definitions[node] = $"output{varIndex}";
                break;
            case NodeType.If:
                await file.WriteAsync($"if{varIndex} = if[{node.text}]\n");
                definitions[node] = $"if{varIndex}";
                break;
            case NodeType.Constant:
                await file.WriteAsync($"const{varIndex} = const[{node.text}]\n");
                definitions[node] = $"const{varIndex}";
                break;
            case NodeType.Operator:
                await file.WriteAsync($"opr{varIndex} = opr[{node.text}]\n");
                definitions[node] = $"opr{varIndex}";
                break;
            case NodeType.SelfSubspace:
                await file.WriteAsync($"self{varIndex} = subspace[self]\n");
                definitions[node] = $"self{varIndex}";
                break;
        }
    }
}

class SaveException : Exception
{
    string message;

    public SaveException(string message)
    {
        this.message = message;
    }

    public override string ToString()
    {
        return message;
    }
}
