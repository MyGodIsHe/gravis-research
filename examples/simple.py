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
if_le >> branch_true(const1)
const1 >> Output()
if_le >> branch_false(sub1)
Constant(1) >> sub1
sub1 >> plus
if_le >> branch_false(sub2)
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
        dot.render(view=True, format='svg')


def main(input_value):
    with DebugContext() as debug:
        result = input.activate(input_value, None)
        print('Result:', result)
        digraph = debug.create_digraph()
        render(digraph)


if __name__ == '__main__':
    main(5)
