import abc
from typing import Optional, List

from . import events

__all__ = (
    'Node',
)


class NodeMeta(type):

    def __new__(mcs, name, bases, namespace):
        for method in events.EVENT_METHODS:
            if method in namespace:
                namespace[method] = events.event_wrapper(namespace[method])
        return super().__new__(mcs, name, bases, namespace)


class Node(metaclass=NodeMeta):
    VALUE_NONE = object()

    def __init__(self, subspace=None):
        self.in_nodes: List['Node'] = []
        self.out_nodes: List['Node'] = []
        self.saved_value = self.VALUE_NONE
        self.subspace: Optional['Node'] = subspace

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
