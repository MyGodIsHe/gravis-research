from contextlib import ContextDecorator
from typing import Optional

from . import events
from .base import Node

__all__ = (
    'Input',
    'Output',
    'Constant',
    'If',
    'Operator',
    'Subspace',
    'branch_true',
    'branch_false',
)


class Input(Node):

    def _activate(self, value):
        self.saved_value = value
        for node in self.out_nodes:
            node.activate(value, self)

    def activate(self, value, src_node):
        self._activate(value)

    def activate_me(self, dst_node):
        self._activate(self.saved_value)


class Output(Node):

    def activate(self, value, src_node):
        self.saved_value = value

    def activate_me(self, dst_node):
        raise Exception


class Constant(Node):

    def __init__(self, value):
        super().__init__()
        self.value = value

    def _activate(self):
        for node in self.out_nodes:
            node.activate(self.value, self)

    def activate(self, value, src_node):
        self._activate()

    def activate_me(self, dst_node):
        self._activate()


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


def branch_true(node):
    return True, node


def branch_false(node):
    return False, node


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


class Subspace(ContextDecorator, Node):
    STACK = []

    def __init__(self, parent=None):
        super().__init__()
        self.parent = parent
        self.inits = {}
        self.connects = []
        self.input: Optional[Input] = None
        self.output: Optional[Output] = None
        self.is_generated = False

    def _generate(self):
        instance_map = {}  # original to new

        for self_node, other_args in self.parent.connects:
            # TODO: need redesign branching
            if isinstance(other_args, tuple):
                other_node = other_args[1]
            else:
                other_node = other_args

            # map original instance to new instance
            for node in [self_node, other_node]:
                if node not in instance_map:
                    args, kwargs = self.parent.inits[node]
                    new_node = node.__class__(*args, **kwargs)
                    new_node.subspace = self
                    instance_map[node] = new_node

            self_node = instance_map[self_node]
            if isinstance(other_args, tuple):
                other_node = other_args[0], instance_map[other_node]
            else:
                other_node = instance_map[other_node]

            # create new connect
            self_node >> other_node

    def activate(self, value, src_node):
        """
        clone self and activate input
        """
        if not self.is_generated:
            with self:
                self._generate()
        self.input.activate(value, src_node)
        self.saved_value = self.output.saved_value
        for node in self.out_nodes:
            node.activate(self.saved_value, self)

    def activate_me(self, dst_node):
        for node in self.in_nodes:
            if not node.is_activated:
                node.activate_me(self)
                break

    def __enter__(self):
        self.STACK.append(self)
        return self

    def __exit__(self, *exc):
        self.is_generated = True
        self.STACK.pop()

    @classmethod
    def current(cls) -> 'Subspace':
        return cls.STACK[-1] if cls.STACK else None


@events.event_init
def subspace_init(self, *args, **kwargs):
    subspace = Subspace.current()
    if subspace and self not in subspace.inits:
        subspace.inits[self] = (args, kwargs)
        if isinstance(self, Input):
            subspace.input = self
        elif isinstance(self, Output):
            subspace.output = self


@events.event_connect
def subspace_connect(self, other):
    subspace = Subspace.current()
    if subspace:
        if isinstance(self, If) and not isinstance(other, tuple):
            return
        subspace.connects.append((self, other))
