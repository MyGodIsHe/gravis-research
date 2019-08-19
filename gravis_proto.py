#!/usr/bin/env python3
import functools
import operator
import sys
from typing import Any

line_terator = 0


def get_name(self):
    if isinstance(self, Node):
        for key, value in globals().items():
            if value == self:
                return key
        uuniq = hex(id(self))[2:]
        if isinstance(self, Constant):
            result = '{}({})'.format(self, self.value)
        else:
            result = repr(self)
        return '{}\n{}'.format(result, uuniq)
    return repr(self)


def method_logger(func):

    @functools.wraps(func)
    def wrapper(self, *args):
        global line_terator
        line_terator += 1
        named_args = tuple(get_name(item) for item in args[:-1])
        style = 'solid' if func.__name__ == 'activate' else 'dotted'
        label_args = '({})'.format(', '.join(named_args)) if named_args else ''
        print(
            '"{}" -> "{}"[label="[{}]{}";style={}]'.format(
                get_name(args[-1]),
                get_name(self),
                line_terator,
                label_args,
                style,
            )
        )
        return func(self, *args)
    return wrapper


class NodeMeta(type):

    def __new__(mcls, name, bases, namespace, **kwargs):
        for method in ['activate', 'activate_me']:
            if method in namespace:
                namespace[method] = method_logger(namespace[method])
        return super().__new__(mcls, name, bases, namespace, **kwargs)


class Node(metaclass=NodeMeta):
    out_nodes: list
    saved_value: Any

    def __repr__(self):
        return self.__class__.__name__

    def __rshift__(self, other):
        self.out_nodes.append(other)
        if hasattr(other, 'connect'):
            other.connect(self)


class Input(Node):

    def __init__(self):
        self.out_nodes = []

    def activate(self, value, src_node):
        self.saved_value = value
        for node in self.out_nodes:
            node.activate(value, self)

    def activate_me(self, dst_node):
        self.activate(self.saved_value, self)


class Output(Node):

    def activate(self, value, src_node):
        self.saved_value = value
        print(value)

    def __rshift__(self, other):
        other.connect(self)

    def activate_me(self, dst_node):
        raise Exception


class Constant(Node):

    def __init__(self, value):
        self.out_nodes = []
        self.value = value

    def activate(self, value, src_node):
        self._activate(value, src_node)

    def _activate(self, value, src_node):
        for node in self.out_nodes:
            node.activate(self.value, self)

    def activate_me(self, dst_node):
        self._activate(self.value, self)


class If(Node):

    def __init__(self, operator):
        self.first_in_node = None
        self.second_in_node = None
        self.in_values = {}
        self.operator = operator
        self.positive_out_nodes = []
        self.negative_out_nodes = []

    def activate(self, value, src_node):
        self.in_values[src_node] = value
        if len(self.in_values) != 2:
            if src_node == self.first_in_node:
                self.second_in_node.activate_me(self)
            else:
                self.first_in_node.activate_me(self)
            return

        first_value = self.in_values[self.first_in_node]
        result = self.operator(
            first_value,
            self.in_values[self.second_in_node],
        )
        self.in_values = {}
        self.saved_value = first_value
        if result:
            for node in self.positive_out_nodes:
                if not hasattr(node, 'saved_value'):
                    node.activate(first_value, self)
        else:
            for node in self.negative_out_nodes:
                if not hasattr(node, 'saved_value'):
                    node.activate(first_value, self)

    def activate_me(self, dst_node):
        if hasattr(self, 'saved_value'):
            dst_node.activate(self.saved_value, self)
        else:
            self.first_in_node.activate_me(self)

    def __rshift__(self, other):
        direction, other = other
        if direction:
            self.positive_out_nodes.append(other)
        else:
            self.negative_out_nodes.append(other)
        if hasattr(other, 'connect'):
            other.connect(self)

    def connect(self, other):
        if self.first_in_node is None:
            self.first_in_node = other
        elif self.second_in_node is None:
            self.second_in_node = other
        else:
            raise Exception()


class Operator(Node):

    def __init__(self, operator):
        self.in_nodes = []
        self.in_values = {}
        self.operator = operator
        self.out_nodes = []

    def activate(self, value, src_node):
        self.in_values[src_node] = value
        missing = set(self.in_nodes) - set(self.in_values)
        if missing:
            missing.pop().activate_me(self)
            return

        result = self.operator(*self.in_values.values())
        self.saved_value = result
        self.in_values = {}
        for node in self.out_nodes:
            node.activate(result, self)

    def activate_me(self, dst_node):
        if hasattr(self, 'saved_value'):
            dst_node.activate(self.saved_value, self)
        else:
            self.in_nodes[0].activate_me(self)

    def connect(self, other):
        self.in_nodes.append(other)


class Callback:

    def __init__(self):
        self.in_node = None
        self.back_node = None
        self.out_nodes = []

    def activate(self, value, src_node):
        self.in_values = {}


class Subspace:

    def __init__(self, *in_nodes):
        self.in_nodes = in_nodes


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

fib = Subspace(input)

fib.in_nodes[0].activate(int(sys.argv[1]), None)
