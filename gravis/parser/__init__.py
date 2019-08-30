import operator
import sys
from typing import List

from antlr4 import *
from .GravisListener import GravisListener
from .GravisLexer import GravisLexer
from .GravisParser import GravisParser
from .. import nodes


__all__ = (
    'parse',
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


class InterpreterListener(GravisListener):

    def __init__(self):
        self.definitions = {}

    def create_node(self, node_def: GravisParser.Node_defContext):
        if node_def.input_def():
            return nodes.Input()
        if node_def.output_def():
            return nodes.Output()
        if node_def.const_def():
            arg = node_def.const_def().NUMBER().getText()
            if arg.isdecimal():
                arg = int(arg)
            elif arg.isnumeric():
                arg = float(arg)
            return nodes.Constant(arg)
        if node_def.if_def():
            arg = OPERATORS_MAP[node_def.if_def().comp_op().getText()]
            return nodes.If(arg)
        if node_def.opr_def():
            arg = OPERATORS_MAP[node_def.opr_def().arith_op().getText()]
            return nodes.Operator(arg)
        if node_def.subspace_def():
            subspace = nodes.Subspace()
            subspace.__enter__()
            return subspace
        if node_def.self_subspace_def():
            self_subspace = nodes.Subspace.current()
            return nodes.Subspace(self_subspace)

    def enterDef_stmt(self, ctx: GravisParser.Def_stmtContext):
        self.definitions[ctx.NAME().getText()] = self.create_node(
            ctx.node_def(),
        )

    def get_dotted_name(self, dotted_name: GravisParser.Dotted_nameContext):
        node_inst = dotted_name.node_inst()
        if node_inst.NAME():
            node_name = node_inst.NAME().getText()
            if node_name not in self.definitions:
                node_inst.parser.notifyErrorListeners(
                    '{} is unknown definition'.format(node_name),
                    e=RecognitionException(
                        recognizer=node_inst.parser,
                        ctx=node_inst,
                    ),
                )
                sys.exit(1)
            node = self.definitions[node_name]
        else:
            node = self.create_node(node_inst.node_def())
        if dotted_name.NAME():
            node = getattr(node, dotted_name.NAME().getText())
        return node

    def enterLink_stmt(self, ctx: GravisParser.Link_stmtContext):
        pair: List[GravisParser.Dotted_nameContext] = ctx.dotted_name()
        left, right = pair
        self.get_dotted_name(left) >> self.get_dotted_name(right)

    def exitSubspace_def(self, ctx: GravisParser.Subspace_defContext):
        subspace = nodes.Subspace.current()
        subspace.__exit__()


def parse(data: str):
    input_stream = InputStream(data)
    lexer = GravisLexer(input_stream)
    stream = CommonTokenStream(lexer)
    parser = GravisParser(stream)
    interpreter = InterpreterListener()
    walker = ParseTreeWalker()
    tree = parser.file_input()
    walker.walk(interpreter, tree)
