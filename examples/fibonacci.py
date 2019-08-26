#!/usr/bin/env python3
import operator

from gravis import *




def render(digraph):
    try:
        import graphviz
    except ImportError:
        print(digraph)
    else:
        dot = graphviz.Source(digraph)
        dot.render(view=True, format='svg')


def main(input_value):
    with Subspace() as fibonacci:
        if_le = If(operator.le)
        sub1 = Operator(operator.sub)
        sub2 = Operator(operator.sub)
        plus = Operator(operator.add)
        const1 = Constant(1)
        output = Output()

        Input() >> if_le
        Constant(2) >> if_le
        if_le >> branch_true(const1)
        const1 >> output
        if_le >> branch_false(sub1)
        Constant(1) >> sub1
        sub1 >> fibonacci
        fibonacci >> plus
        if_le >> branch_false(sub2)
        Constant(2) >> sub2
        sub2 >> fibonacci
        fibonacci >> plus
        plus >> output

    with DebugContext() as debug:
        fibonacci.activate(input_value, None)
        digraph = debug.create_digraph()
        render(digraph)


if __name__ == '__main__':
    main(2)
