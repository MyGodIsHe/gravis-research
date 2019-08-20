import functools
from contextlib import ContextDecorator
from enum import Enum
from io import StringIO
from typing import Any, NamedTuple, List, Tuple

__all__ = (
    'Node',
    'DebugContext',
)


class Direction(Enum):
    back = False
    forward = True


class NodeName(NamedTuple):
    label: str
    id: str

    OPERATOR_MAP = {
        'le': '<=',
        'add': '+',
        'sub': '-',
    }

    @staticmethod
    def resolve_operator(value):
        return NodeName.OPERATOR_MAP.get(value, value)

    @staticmethod
    def create(node: 'Node') -> 'NodeName':
        from . import nodes

        if isinstance(node, Node):
            if isinstance(node, nodes.Constant):
                label = str(node.value)
            elif isinstance(node, nodes.If):
                label = NodeName.resolve_operator(node.operator.__name__)
            elif isinstance(node, nodes.Operator):
                label = NodeName.resolve_operator(node.operator.__name__)
            else:
                label = repr(node)
        else:
            label = repr(node)

        uuid = hex(id(node))[2:]
        return NodeName(label=label, id=uuid)


class Iteration(NamedTuple):
    src: NodeName
    dst: NodeName
    args: Tuple[NodeName]
    direction: Direction

    @property
    def style(self):
        return 'solid' if self.direction.value else 'dotted'

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

        nodes = set()
        for iteration in self.iterations:
            if iteration.src.label == 'None':
                continue
            nodes.add(iteration.src)
            nodes.add(iteration.dst)

        for node in nodes:
            stream.write('\t"{}" [label="{}"];\n'.format(node.id, node.label))

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
    out_nodes: list
    saved_value: Any

    def __repr__(self):
        return self.__class__.__name__

    def __rshift__(self, other):
        self.out_nodes.append(other)
        if hasattr(other, 'connect'):
            other.connect(self)
