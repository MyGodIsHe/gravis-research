import operator
import re
from typing import IO

from . import nodes

__all__ = (
    'parse',
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
}


class ParseException(Exception):
    pass


class BadLine(ParseException):

    def __init__(self, line):
        self.line = line

    def __repr__(self):
        return 'Error in line {}'.format(self.line)


class BadArgument(ParseException):

    def __init__(self, line, arg):
        self.line = line
        self.arg = arg

    def __repr__(self):
        return 'Error in line {}, arg {}'.format(self.line, self.arg)


class BadBranch(ParseException):

    def __init__(self, line, node, branch):
        self.line = line
        self.node = node
        self.branch = branch

    def __repr__(self):
        return 'Error in line {}, {}.{} is bad branch'.format(
            self.line,
            self.node,
            self.branch,
        )


def parse_arg(arg: str, line: int):
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
    raise BadArgument(line, arg)


def create_node(name, args, line: int):
    node_class = NODES_MAP[name]
    if args:
        args = [
            parse_arg(arg, line)
            for arg in args.split(',')
        ]
    else:
        args = []
    return node_class(*args)


def parse(data: IO):
    definitions = {}
    inputs = []

    for line_number, line in enumerate(data.readlines(), 1):
        line = line.replace(' ', '').replace('\t', '').replace('\n', '')
        if not line:
            continue
        definition = RE_DEFINITION.match(line)
        if definition:
            node = create_node(
                definition.group('node'),
                definition.group('args'),
                line_number,
            )
            if isinstance(node, nodes.Input):
                inputs.append(node)
            definitions[definition.group('name')] = node
            continue
        link = RE_LINK.match(line)
        if link:
            if link.group('left_name'):
                parts = link.group('left_name').split('.')
                left_node = definitions[parts[0]]
                if isinstance(definitions[parts[0]], nodes.If):
                    if parts[1] not in ['true', 'false']:
                        raise BadBranch(line_number, parts[0], parts[1])
                    left_node = getattr(left_node, parts[1])
            else:
                left_node = create_node(
                    link.group('left_node'),
                    link.group('left_args'),
                    line_number,
                )
                if isinstance(left_node, nodes.Input):
                    inputs.append(left_node)
            if link.group('right_name'):
                right_node = definitions[link.group('right_name')]
            else:
                right_node = create_node(
                    link.group('right_node'),
                    link.group('right_args'),
                    line_number,
                )
                if isinstance(right_node, nodes.Input):
                    inputs.append(right_node)
            left_node >> right_node
            continue
        raise BadLine(line_number)

    return inputs
