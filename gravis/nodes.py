from .base import Node

__all__ = (
    'Input',
    'Output',
    'Constant',
    'If',
    'Operator',
)


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
