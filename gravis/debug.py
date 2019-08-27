from collections import defaultdict
from contextlib import ContextDecorator
from enum import Enum
from io import StringIO
from typing import NamedTuple, List, Tuple

from .base import Node
from .nodes import *
from . import events

__all__ = (
    'DebugContext',
)


@events.event_activate
def log_forward(self, *args):
    debug = DebugContext.current()
    if debug:
        values = args[:-1]
        src_node = args[-1]
        debug.iterations.append(Iteration(
            src=NodeName.create(src_node),
            dst=NodeName.create(self),
            direction=Direction.forward,
            args=values,
        ))


@events.event_activate_me
def log_backward(self, *args):
    debug = DebugContext.current()
    if debug:
        values = args[:-1]
        src_node = args[-1]
        debug.iterations.append(Iteration(
            src=NodeName.create(src_node),
            dst=NodeName.create(self),
            direction=Direction.backward,
            args=values,
        ))


def get_uuid(node, prefix=None):
    if not prefix:
        prefix = repr(node).lower()
    return '{}_{}'.format(prefix, hex(id(node))[2:])


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

    SHAPE_MAP = {
        'Input': 'square',
        'Output': 'square',
        'Constant': 'circle',
        'If': 'diamond',
        'Operator': 'diamond',
    }

    @property
    def shape(self):
        return self.SHAPE_MAP.get(repr(self.node), 'box')

    @staticmethod
    def resolve_operator(value):
        return NodeName.OPERATOR_MAP.get(value, value)

    @staticmethod
    def create(node: Node) -> 'NodeName':
        if isinstance(node, Input):
            label = 'I'
        elif isinstance(node, Output):
            label = 'O'
        elif isinstance(node, Constant):
            label = str(node.value)
        elif isinstance(node, If):
            label = NodeName.resolve_operator(node.operator.__name__)
        elif isinstance(node, Operator):
            label = NodeName.resolve_operator(node.operator.__name__)
        else:
            label = repr(node)

        return NodeName(
            label=label,
            id=get_uuid(node),
            node=node,
        )


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
            '\t{src} -> {dst} '
            '[label="{label}";style={style}];'
            '\n'.format(
                src=self.src.id,
                dst=self.dst.id,
                label=label,
                style=self.style,
            )
        )


def around_generator(items: List[Iteration]):
    items = iter(items)
    past = None
    current = next(items, None)
    future = next(items, None)
    while current:
        yield past, current, future
        past = current
        current = future
        future = next(items, None)


def subspace_bridge(past: Iteration, current: Iteration, future: Iteration):
    if current.direction == Direction.forward:
        if isinstance(current.dst.node, Subspace):
            return Iteration(
                src=current.src,
                dst=future.dst,
                args=current.args,
                direction=current.direction,
            )
        if isinstance(current.src.node, Subspace):
            return Iteration(
                src=past.dst,
                dst=current.dst,
                args=current.args,
                direction=current.direction,
            )


def filter_iterations(iterations: List[Iteration]):
    filtered = []
    for past, current, future in around_generator(iterations):
        if current.src.node is None:
            continue
        if current.direction == Direction.forward:
            if isinstance(current.dst.node, Subspace):
                continue
            if isinstance(current.src.node, Subspace):
                continue
            if (
                    isinstance(current.dst.node, Output) and
                    future and isinstance(future.src.node, Subspace)
            ):
                filtered.append(current)
                filtered.append(Iteration(
                    src=current.dst,
                    dst=future.dst,
                    args=current.args,
                    direction=Direction.forward,
                ))
                continue
        else:
            if isinstance(current.dst.node, Subspace):
                filtered.append(Iteration(
                    src=current.src,
                    dst=future.dst,
                    args=current.args,
                    direction=Direction.backward,
                ))
                continue
            if isinstance(current.src.node, Subspace):
                continue
        filtered.append(current)

    return filtered


def collect_links(start_node: Node):
    pass_nodes = set()
    links = set()
    coming_nodes = {start_node}

    while coming_nodes:
        current = coming_nodes.pop()
        pass_nodes.add(current)
        linked_nodes = set(current.in_nodes + current.out_nodes) - pass_nodes
        coming_nodes |= linked_nodes
        if isinstance(current, Subspace):
            continue
        for other in linked_nodes:
            if isinstance(other, Subspace):
                continue
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


def get_subspace_map(node_names):
    subspace_map = defaultdict(set)
    for node_name in node_names:
        if node_name.node.subspace:
            subspace_map[node_name.node.subspace].add(node_name.node)

    return subspace_map


class DebugContext(ContextDecorator):
    STACK = []

    def __init__(self):
        self.iterations: List[Iteration] = []

    def create_digraph(self):
        stream = StringIO()
        stream.write('digraph {\n')

        if self.iterations:
            iterations = filter_iterations(self.iterations)

            for step, iteration in enumerate(iterations):
                stream.write(iteration.render(step + 1))

            start_node = iterations[0].src.node
            all_links, all_nodes = collect_links(start_node)
            level_map = defaultdict(set)
            #collect_levels(start_node, level_map)
            activated_links = {
                (
                    (iteration.src.node, iteration.dst.node)
                    if id(iteration.src.node) > id(iteration.dst.node)
                    else (iteration.dst.node, iteration.src.node)
                )
                for iteration in iterations
            }
            pass_links = all_links - activated_links
            pass_links = {
                (NodeName.create(src), NodeName.create(dst))
                for src, dst in pass_links
            }

            all_node_names = set()
            for iteration in iterations:
                all_node_names.add(iteration.src)
                all_node_names.add(iteration.dst)
            for src, dst in pass_links:
                all_node_names.add(src)
                all_node_names.add(dst)

            for src, dst in pass_links:
                stream.write((
                    '\t{src} -> {dst} [arrowhead=none];'
                    '\n'.format(
                        src=src.id,
                        dst=dst.id,
                    )
                ))

            for node_name in all_node_names:
                stream.write(
                    '\t{} [label="{}";shape={}];\n'.format(
                        node_name.id,
                        node_name.label,
                        node_name.shape,
                    )
                )

            for subspace, own_nodes in get_subspace_map(all_node_names).items():
                stream.write(
                    '\tsubgraph {} {{\n'.format(get_uuid(subspace, 'cluster'))
                )
                for node in own_nodes:
                    stream.write('\t\t{};\n'.format(get_uuid(node)))
                stream.write('\t}\n')

            for level_nodes in level_map.values():
                if len(level_nodes) > 1:
                    stream.write(
                        '\t{{rank=same {}}}\n'.format(
                            ' '.join(
                                '{}'.format(get_uuid(node))
                                for node in level_nodes
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
    def current(cls) -> 'DebugContext':
        return cls.STACK[-1] if cls.STACK else None
