using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Node
{
    public GameObject gameObject;
    public List<Node> inputs = new();
    public List<Node> outputs = new();
    public Vector3 position;
    public string text;
    public NodeType type;

    public static List<List<Node>> FindIsolatedGraphs(List<Node> nodes)
    {
        var result = new List<List<Node>>();
        var pass = new HashSet<Node>();
        foreach (var node in nodes)
        {
            if (pass.Contains(node))
                continue;
            var subgraph = new List<Node>();
            FindSubgraph(node, subgraph);
            result.Add(subgraph);
            pass.UnionWith(subgraph);
        }
        return result;
    }

    private static void FindSubgraph(Node node, List<Node> result)
    {
        if (result.Contains(node))
            return;
        result.Add(node);
        foreach (var input_node in node.inputs)
        {
            FindSubgraph(input_node, result);
        }
        foreach (var output_node in node.outputs)
        {
            FindSubgraph(output_node, result);
        }
    }

    public static Node GetStartNode(List<Node> nodes)
    {
        var e = nodes.GetEnumerator();
        e.MoveNext();
        var result = e.Current;
        while (e.MoveNext())
        {
            var node = e.Current;
            if (node.inputs.Count < result.inputs.Count && node.outputs.Count != 0)
            {
                result = node;
            }
        }
        return result;
    }

    public static async Task AlignNodesByForceDirected(List<Node> nodes)
    {
        const float MAX_ITERS = 100;

        for (var i = 0; i < MAX_ITERS; i++)
        {
            if (!AlignNodesByForceDirectedStep(nodes))
            {
                break;
            }
            await Task.Yield();
        }
    }

    public static bool AlignNodesByForceDirectedStep(List<Node> nodes)
    {
        const float MIN_DIST = 1.0f;
        const float DELTA_TIME = 0.05f;
        const float EPSILON = 0.01f;
        var changes = new Dictionary<Node, Vector3>();

        foreach (var node in nodes)
            changes[node] = Vector3.zero;
        foreach (var node in nodes)
        {
            foreach (var others in new List<List<Node>> { node.inputs, node.outputs })
            {
                foreach (var other in others)
                {
                    var force = dispersionForce() + node.position - other.position;
                    var dist = force.magnitude;
                    changes[node] += (MIN_DIST - dist) * DELTA_TIME * force.normalized;
                }
            }
            foreach (var other in nodes)
            {
                if (node == other) continue;
                var force = dispersionForce() + other.position - node.position;
                var dist = force.magnitude;
                var value = DELTA_TIME / (dist * dist);
                changes[other] += force.normalized * value;
            }
        }
        var wasChanges = false;
        foreach (var pair in changes)
        {
            if (pair.Value.magnitude > EPSILON)
            {
                pair.Key.position += pair.Value;
                wasChanges = true;
            }
        }
        return wasChanges;
    }

    private static Vector3 dispersionForce()
    {
        return new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
        );
    }

    public static void AlignNodes(List<Node> nodes)
    {
        // set levels
        var startNode = GetStartNode(nodes);
        var levels = new Dictionary<Node, int>
        {
            [startNode] = 0
        };
        var future = new HashSet<Node>
        {
            startNode
        };
        var pass = new HashSet<Node>();
        while (future.Count != 0)
        {
            var newFuture = new HashSet<Node>();
            foreach (var node in future)
            {
                var level = levels[node];
                foreach (var input_node in node.inputs)
                {
                    if (!pass.Contains(input_node))
                    {
                        levels[input_node] = level - 1;
                        newFuture.Add(input_node);
                    }
                }
                foreach (var output_node in node.outputs)
                {
                    if (!pass.Contains(output_node))
                    {
                        levels[output_node] = level + 1;
                        newFuture.Add(output_node);
                    }
                }
                pass.Add(node);
            }
            future = newFuture;
        }

        // group by level
        var group_by_level = new SortedDictionary<int, List<Node>>();
        foreach (var pair in levels)
        {
            if (!group_by_level.ContainsKey(pair.Value))
            {
                group_by_level[pair.Value] = new List<Node>();
            }
            group_by_level[pair.Value].Add(pair.Key);
        }

        // set positions
        foreach (var pair in group_by_level)
        {
            var offset = new Vector3(-pair.Value.Count / 2.0f, 0, 0);
            int i = 0;
            foreach (var node in pair.Value)
            {
                node.position = new Vector3(i, pair.Key) + offset;
                i++;
            }
        }
    }
}

public enum NodeType
{
    Input,
    Output,
    If,
    Constant,
    Operator,
    Subspace,
    SelfSubspace
}
