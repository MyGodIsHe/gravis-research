from .base import Node

__all__ = (
    'Input',
    'Output',
    'Constant',
    'If',
    'Operator',
)


class Input(Node):

    def _activate(self, value, src_node):
        self.saved_value = value
        for node in self.out_nodes:
            node.activate(value, self)

    def activate(self, value, src_node):
        self._activate(value, src_node)

    def activate_me(self, dst_node):
        self._activate(self.saved_value, self)


class Output(Node):

    def activate(self, value, src_node):
        self.saved_value = value
        print(value)

    def activate_me(self, dst_node):
        raise Exception


class Constant(Node):

    def __init__(self, value):
        super().__init__()
        self.value = value

    def _activate(self, value, src_node):
        for node in self.out_nodes:
            node.activate(self.value, self)

    def activate(self, value, src_node):
        self._activate(value, src_node)

    def activate_me(self, dst_node):
        self._activate(self.value, self)


class If(Node):

    def __init__(self, operator):
        super().__init__()
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
                if not node.is_activated:
                    node.activate(first_value, self)
        else:
            for node in self.negative_out_nodes:
                if not node.is_activated:
                    node.activate(first_value, self)

    def activate_me(self, dst_node):
        if self.is_activated:
            dst_node.activate(self.saved_value, self)
        else:
            self.first_in_node.activate_me(self)

    def __rshift__(self, other):
        direction, other = other
        if direction:
            self.positive_out_nodes.append(other)
        else:
            self.negative_out_nodes.append(other)
        super().__rshift__(other)

    def add_in(self, other):
        super().add_in(other)
        if self.first_in_node is None:
            self.first_in_node = other
        elif self.second_in_node is None:
            self.second_in_node = other
        else:
            raise Exception()


class Operator(Node):

    def __init__(self, operator):
        super().__init__()
        self.in_values = {}
        self.operator = operator

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
        if self.is_activated:
            dst_node.activate(self.saved_value, self)
        else:
            self.in_nodes[0].activate_me(self)
