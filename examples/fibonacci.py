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
    with Subspace() as self:
        input = Input()
        if_le = If(operator.le)
        sub_left = Operator(operator.sub)
        sub_right = Operator(operator.sub)
        plus = Operator(operator.add)
        const1 = Constant(1)
        output = Output()
        self_left = Subspace(self)
        self_right = Subspace(self)

        input >> if_le
        Constant(2) >> if_le
        if_le.true >> const1
        const1 >> output
        if_le.false >> sub_left
        Constant(1) >> sub_left
        sub_left >> self_left
        self_left >> plus
        if_le.false >> sub_right
        Constant(2) >> sub_right
        sub_right >> self_right
        self_right >> plus
        plus >> output

    with DebugContext() as debug:
        input.activate(input_value, None)
        print('Result:', output.saved_value)
        digraph = debug.create_digraph()
        render(digraph)


if __name__ == '__main__':
    main(4)
