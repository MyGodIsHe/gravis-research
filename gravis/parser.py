import operator
import re
from typing import IO

from . import nodes

__all__ = (
    'ParseException',
    'Parser',
)

RE_NAME = r'(?P<name>\w+)'
RE_NODE = r'(?P<node>\w+)\((?P<args>[^)]*)\)'
RE_DEFINITION = re.compile(
    r'^'
    r'(?P<name>\w+)'
    r'='
    r'*(?P<node>\w+)\((?P<args>[^)]*)\)'
    r'$'
)
RE_LINK = re.compile(
    r'^'
    r'((?P<left_name>\w+.?\w*)|(?P<left_node>\w+)\((?P<left_args>[^)]*)\))'
    r'>>'
    r'((?P<right_name>\w+.?\w*)|(?P<right_node>\w+)\((?P<right_args>[^)]*)\))'
    r'$'
)
RE_SUBSPACE = re.compile(
    r'^'
    r'(?P<name>\w+):'
    r'$'
)

OPERATORS_MAP = {
    '<': operator.lt,
    '>': operator.gt,
    '<=': operator.le,
    '>=': operator.ge,
    '=': operator.eq,
    '!=': operator.ne,
    '+': operator.add,
    '-': operator.sub,
    '*': operator.mul,
    '/': operator.truediv,
}
NODES_MAP = {
    'If': nodes.If,
    'Input': nodes.Input,
    'Output': nodes.Output,
    'Const': nodes.Constant,
    'Operator': nodes.Operator,
    'Subspace': nodes.Subspace,
}


class ParseException(Exception):
    pass


class BadLine(ParseException):

    def __init__(self, source, line):
        self.source = source
        self.line = line

    def __str__(self):
        return 'Error in line {}: {}'.format(self.line, self.source)


class UnknownNode(ParseException):

    def __init__(self, source, line, node):
        self.source = source
        self.line = line
        self.node = node

    def __str__(self):
        return 'Error in line {}: {}\n{} is unknown node'.format(
            self.line,
            self.source,
            self.node,
        )


class UnknownDefinition(ParseException):

    def __init__(self, source, line, node):
        self.source = source
        self.line = line
        self.node = node

    def __str__(self):
        return 'Error in line {}: {}\n{} is unknown definition'.format(
            self.line,
            self.source,
            self.node,
        )


class BadSpacing(ParseException):

    def __init__(self, source, line):
        self.source = source
        self.line = line

    def __str__(self):
        return 'Error in line {}: {}\nBad spacing'.format(
            self.line,
            self.source,
        )


class BadBranch(ParseException):

    def __init__(self, source, line, node, branch):
        self.source = source
        self.line = line
        self.node = node
        self.branch = branch

    def __str__(self):
        return 'Error in line {}: {}\n{}.{} is bad branch'.format(
            self.line,
            self.source,
            self.node,
            self.branch,
        )


class Parser:

    def __init__(self):
        self.current_source = None
        self.current_line = None
        self.current_depth_level = 0
        self.definitions = {}
        self.inputs = []
        self.subspace_stack = []

    def set_current_depth_level(self, line, spaces_per_level=4):
        spaces = 0
        for char in line:
            if char == '\t':
                spaces += spaces_per_level
            elif char == ' ':
                spaces += 1
            else:
                break
        if spaces % spaces_per_level != 0:
            raise BadSpacing(self.current_source, self.current_line)
        self.current_depth_level = int(spaces / spaces_per_level)

        while self.subspace_stack:
            subspace, lvl = self.subspace_stack[-1]
            if self.current_depth_level < lvl:
                self.subspace_stack.pop()
                subspace.__exit__()
            else:
                break

    def get_definition(self, name):
        if name not in self.definitions:
            raise UnknownDefinition(
                self.current_source,
                self.current_line,
                name,
            )
        return self.definitions[name]

    def parse_arg(self, arg: str):
        if arg in OPERATORS_MAP:
            return OPERATORS_MAP[arg]
        try:
            return int(arg)
        except ValueError:
            pass
        try:
            return float(arg)
        except ValueError:
            pass
        return self.get_definition(arg)

    def create_node(self, name, args):
        if name not in NODES_MAP:
            raise UnknownNode(self.current_source, self.current_line, name)
        node_class = NODES_MAP[name]
        if args:
            args = [
                self.parse_arg(arg)
                for arg in args.split(',')
            ]
        else:
            args = []
        return node_class(*args)

    def parse_definition(self, definition):
        node = self.create_node(
            definition.group('node'),
            definition.group('args'),
        )
        if isinstance(node, nodes.Input):
            self.inputs.append(node)
        self.definitions[definition.group('name')] = node

    def parse_link(self, link):
        if link.group('left_name'):
            parts = link.group('left_name').split('.')
            left_node = self.get_definition(parts[0])
            if isinstance(left_node, nodes.If):
                if parts[1] not in ['true', 'false']:
                    raise BadBranch(
                        self.current_source, self.current_line,
                        parts[0], parts[1],
                    )
                left_node = getattr(left_node, parts[1])
        else:
            left_node = self.create_node(
                link.group('left_node'),
                link.group('left_args'),
            )
            if isinstance(left_node, nodes.Input):
                self.inputs.append(left_node)
        if link.group('right_name'):
            right_node = self.get_definition(link.group('right_name'))
        else:
            right_node = self.create_node(
                link.group('right_node'),
                link.group('right_args'),
            )
            if isinstance(right_node, nodes.Input):
                self.inputs.append(right_node)
        left_node >> right_node

    def parse_subspace(self, subspace):
        subspace = self.get_definition(subspace.group('name'))
        subspace.__enter__()
        self.subspace_stack.append((subspace, self.current_depth_level + 1))

    def parse(self, data: IO):
        for line_number, line in enumerate(data.readlines(), 1):
            self.current_source = line.strip()
            self.current_line = line_number
            if self.current_source:
                self.set_current_depth_level(line)
            line = line.replace(' ', '').replace('\t', '').replace('\n', '')
            if not line:
                continue

            definition = RE_DEFINITION.match(line)
            if definition:
                self.parse_definition(definition)
                continue

            link = RE_LINK.match(line)
            if link:
                self.parse_link(link)
                continue

            subspace = RE_SUBSPACE.match(line)
            if subspace:
                self.parse_subspace(subspace)
                continue

            raise BadLine(self.current_source, self.current_line)

        while self.subspace_stack:
            subspace, lvl = self.subspace_stack.pop()
            subspace.__exit__()

        return self.inputs
