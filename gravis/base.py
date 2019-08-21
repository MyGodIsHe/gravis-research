import abc
import functools
from collections import defaultdict
from contextlib import ContextDecorator
from enum import Enum
from io import StringIO
from typing import NamedTuple, List, Tuple, Set

__all__ = (
    'Node',
    'DebugContext',
)


def method_logger(func):

    @functools.wraps(func)
    def wrapper(self, *args):
        debug = DebugContext.current()
        if debug:
            values = args[:-1]
            src_node = args[-1]
            debug.iterations.append(Iteration(
                src=NodeName.create(src_node),
                dst=NodeName.create(self),
                direction=Direction(func.__name__ == 'activate'),
                args=values,
            ))
        return func(self, *args)
    return wrapper


class NodeMeta(type):

    def __new__(mcs, name, bases, namespace):
        for method in ['activate', 'activate_me']:
            if method in namespace:
                namespace[method] = method_logger(namespace[method])
        return super().__new__(mcs, name, bases, namespace)


class Node(metaclass=NodeMeta):
    VALUE_NONE = object()

    def __init__(self):
        self.in_nodes = []
        self.out_nodes = []
        self.saved_value = self.VALUE_NONE

    def __repr__(self):
        return self.__class__.__name__

    def __rshift__(self, other: 'Node'):
        self.add_out(other)
        other.add_in(self)

    def add_in(self, other):
        self.in_nodes.append(other)

    def add_out(self, other):
        self.out_nodes.append(other)

    @property
    def is_activated(self):
        return self.saved_value != self.VALUE_NONE

    @abc.abstractmethod
    def activate(self, value, src_node):
        pass

    @abc.abstractmethod
    def activate_me(self, dst_node):
        pass


def uuid(node):
    return hex(id(node))[2:]


class Direction(Enum):
    backward = False
    forward = True


class NodeName(NamedTuple):
    label: str
    id: str
    node: Node

    OPERATOR_MAP = {
        'le': '<=',
        'add': '+',
        'sub': '-',
    }

    @staticmethod
    def resolve_operator(value):
        return NodeName.OPERATOR_MAP.get(value, value)

    @staticmethod
    def create(node: Node) -> 'NodeName':
        from . import nodes

        if isinstance(node, nodes.Constant):
            label = str(node.value)
        elif isinstance(node, nodes.If):
            label = NodeName.resolve_operator(node.operator.__name__)
        elif isinstance(node, nodes.Operator):
            label = NodeName.resolve_operator(node.operator.__name__)
        else:
            label = repr(node)

        return NodeName(label=label, id=uuid(node), node=node)


class Iteration(NamedTuple):
    src: NodeName
    dst: NodeName
    args: Tuple[NodeName]
    direction: Direction

    @property
    def style(self):
        return 'solid' if self.direction == Direction.forward else 'dotted'

    @property
    def label_args(self):
        if self.args:
            return ', '.join(str(arg) for arg in self.args)
        return ''

    def render(self, step):
        label_args = self.label_args
        if label_args:
            label = r'[{}]\n{}'.format(step, label_args)
        else:
            label = '[{}]'.format(step)
        return (
            '\t"{src}" -> "{dst}" '
            '[label="{label}";style={style}];'
            '\n'.format(
                src=self.src.id,
                dst=self.dst.id,
                label=label,
                style=self.style,
            )
        )


class DebugContext(ContextDecorator):
    STACK = []

    def __init__(self):
        self.iterations: List[Iteration] = []

    def create_digraph(self):
        stream = StringIO()
        stream.write('digraph {\n')

        for step, iteration in enumerate(self.iterations[1:]):
            stream.write(iteration.render(step + 1))

        start_node = self.iterations[0].dst.node
        all_links, all_nodes = collect_links(start_node)
        level_map = defaultdict(set)
        collect_levels(start_node, level_map)
        activated_links = {
            (
                (iteration.src.node, iteration.dst.node)
                if id(iteration.src.node) > id(iteration.dst.node)
                else (iteration.dst.node, iteration.src.node)
            )
            for iteration in self.iterations[1:]
        }
        pass_links = all_links - activated_links

        pass_links = {
            (NodeName.create(src), NodeName.create(dst))
            for src, dst in pass_links
        }
        for src, dst in pass_links:
            stream.write((
                '\t"{src}" -> "{dst}" [arrowhead=none];'
                '\n'.format(
                    src=src.id,
                    dst=dst.id,
                )
            ))

        nodes = set()
        for iteration in self.iterations[1:]:
            nodes.add(iteration.src)
            nodes.add(iteration.dst)
        for src, dst in pass_links:
            nodes.add(src)
            nodes.add(dst)

        for node in nodes:
            stream.write('\t"{}" [label="{}"];\n'.format(node.id, node.label))

        for nodes in level_map.values():
            if len(nodes) > 1:
                stream.write(
                    '\t{{rank=same {}}}\n'.format(
                        ' '.join(
                            '"{}"'.format(uuid(node))
                            for node in nodes
                        )
                    )
                )

        stream.write('}\n')
        return stream.getvalue()

    def __enter__(self):
        self.STACK.append(self)
        return self

    def __exit__(self, *exc):
        self.STACK.pop()

    @classmethod
    def current(cls):
        return cls.STACK[-1] if cls.STACK else None


def collect_links(start_node: Node):
    pass_nodes = set()
    links = set()
    coming_nodes = {start_node}

    while coming_nodes:
        current = coming_nodes.pop()
        pass_nodes.add(current)
        linked_nodes = set(current.in_nodes + current.out_nodes) - pass_nodes
        coming_nodes |= linked_nodes
        for other in linked_nodes:
            if id(current) > id(other):
                links.add((current, other))
            else:
                links.add((other, current))

    return links, pass_nodes


def collect_levels(node: Node, level_map, level=0):
    if node in level_map[level]:
        return
    level_map[level].add(node)
    for obj in node.out_nodes:
        collect_levels(obj, level_map, level + 1)
    for obj in node.in_nodes:
        collect_levels(obj, level_map, level - 1)
