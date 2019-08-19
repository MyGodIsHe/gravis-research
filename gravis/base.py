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

    @staticmethod
    def get_pair(node: 'Node'):
        from .nodes import Constant

        if isinstance(node, Node):
            for key, value in globals().items():
                if value == node:
                    return key, value
            if isinstance(node, Constant):
                result = '{}({})'.format(node, node.value)
            else:
                result = repr(node)
        else:
            result = repr(node)

        return result, node

    @staticmethod
    def create(node: 'Node') -> 'NodeName':
        label, obj = NodeName.get_pair(node)
        uuid = hex(id(obj))[2:]
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
            return '({})'.format(
                ', '.join(arg.label for arg in self.args)
            )
        return ''


class DebugContext(ContextDecorator):
    STACK = []

    def __init__(self):
        self.iterations: List[Iteration] = []

    def create_digraph(self):
        stream = StringIO()
        stream.write('digraph {\n')

        for step, iteration in enumerate(self.iterations):
            stream.write(
                '\t"{src}" -> "{dst}"'
                '[label="[{step}]{args}";style={style}];\n'.format(
                    src=iteration.src.id,
                    dst=iteration.dst.id,
                    step=step + 1,
                    args=iteration.label_args,
                    style=iteration.style,
                )
            )

        nodes = set()
        for iteration in self.iterations:
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
            debug.iterations.append(Iteration(
                src=NodeName.create(args[-1]),
                dst=NodeName.create(self),
                direction=Direction(func.__name__ == 'activate'),
                args=tuple(NodeName.create(item) for item in args[:-1]),
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
