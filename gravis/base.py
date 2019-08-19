import functools
from contextlib import ContextDecorator
from enum import Enum
from io import StringIO
from typing import Any, NamedTuple, List

__all__ = (
    'Node',
    'DebugContext',
)


def get_name(self):
    from .nodes import Constant

    if isinstance(self, Node):
        for key, value in globals().items():
            if value == self:
                return key
        uuniq = hex(id(self))[2:]
        if isinstance(self, Constant):
            result = '{}({})'.format(self, self.value)
        else:
            result = repr(self)
        return r'{}\n{}'.format(result, uuniq)
    return repr(self)


def method_logger(func):

    @functools.wraps(func)
    def wrapper(self, *args):
        DebugContext.current().iterations.append(Iteration(
            src=get_name(args[-1]),
            dst=get_name(self),
            direction=Direction(func.__name__ == 'activate'),
            args=tuple(get_name(item) for item in args[:-1]),
        ))
        return func(self, *args)
    return wrapper


class Direction(Enum):
    back = False
    forward = True


class Iteration(NamedTuple):
    src: str
    dst: str
    args: tuple
    direction: Direction

    @property
    def style(self):
        return 'solid' if self.direction.value else 'dotted'

    @property
    def label_args(self):
        return '({})'.format(', '.join(self.args)) if self.args else ''


class DebugContext(ContextDecorator):
    STACK = []

    def __init__(self):
        self.iterations: List[Iteration] = []

    def create_digraph(self):
        stream = StringIO()
        stream.write('digraph {\n')
        for step, iteration in enumerate(self.iterations):
            stream.write(
                '\t"{src}" -> "{dst}"[label="[{step}]{args}";style={style}]\n'.format(
                    src=iteration.src,
                    dst=iteration.dst,
                    step=step + 1,
                    args=iteration.label_args,
                    style=iteration.style,
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
        return cls.STACK[-1]


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
