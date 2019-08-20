#!/usr/bin/env python3
import operator

from gravis import *


input = Input()
if_le = If(operator.le)
sub1 = Operator(operator.sub)
sub2 = Operator(operator.sub)
plus = Operator(operator.add)
const1 = Constant(1)

input >> if_le
Constant(1) >> if_le
if_le >> (True, const1)
const1 >> Output()
if_le >> (False, sub1)
Constant(1) >> sub1
sub1 >> plus
if_le >> (False, sub2)
Constant(2) >> sub2
sub2 >> plus
plus >> Output()


def render(digraph):
    try:
        import graphviz
    except ImportError:
        print(digraph)
    else:
        dot = graphviz.Source(digraph)
        dot.render(view=True)


def main(input_value):
    with DebugContext() as debug:
        input.activate(input_value, None)
        render(debug.create_digraph())


if __name__ == '__main__':
    main(5)
