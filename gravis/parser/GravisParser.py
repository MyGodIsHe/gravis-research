# Generated from Gravis.g4 by ANTLR 4.7.2
# encoding: utf-8
from antlr4 import *
from io import StringIO
from typing.io import TextIO
import sys


def serializedATN():
    with StringIO() as buf:
        buf.write("\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\36")
        buf.write("n\4\2\t\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b")
        buf.write("\t\b\4\t\t\t\4\n\t\n\4\13\t\13\4\f\t\f\4\r\t\r\4\16\t")
        buf.write("\16\4\17\t\17\4\20\t\20\4\21\t\21\3\2\7\2$\n\2\f\2\16")
        buf.write("\2\'\13\2\3\2\3\2\3\3\3\3\5\3-\n\3\3\4\3\4\3\4\3\4\3\5")
        buf.write("\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\6\3\6\3\6\5\6>\n\6\3\7")
        buf.write("\3\7\3\b\3\b\3\t\3\t\3\t\3\t\3\t\3\n\3\n\3\n\3\n\3\n\3")
        buf.write("\13\3\13\3\13\3\13\3\13\3\f\3\f\3\f\6\fV\n\f\r\f\16\f")
        buf.write("W\3\f\3\f\3\r\3\r\3\r\3\r\3\r\3\16\3\16\3\16\5\16d\n\16")
        buf.write("\3\17\3\17\5\17h\n\17\3\20\3\20\3\21\3\21\3\21\2\2\22")
        buf.write("\2\4\6\b\n\f\16\20\22\24\26\30\32\34\36 \2\4\3\2\21\26")
        buf.write("\3\2\f\20\2h\2%\3\2\2\2\4,\3\2\2\2\6.\3\2\2\2\b\62\3\2")
        buf.write("\2\2\n=\3\2\2\2\f?\3\2\2\2\16A\3\2\2\2\20C\3\2\2\2\22")
        buf.write("H\3\2\2\2\24M\3\2\2\2\26R\3\2\2\2\30[\3\2\2\2\32`\3\2")
        buf.write("\2\2\34g\3\2\2\2\36i\3\2\2\2 k\3\2\2\2\"$\5\4\3\2#\"\3")
        buf.write("\2\2\2$\'\3\2\2\2%#\3\2\2\2%&\3\2\2\2&(\3\2\2\2\'%\3\2")
        buf.write("\2\2()\7\2\2\3)\3\3\2\2\2*-\5\b\5\2+-\5\6\4\2,*\3\2\2")
        buf.write("\2,+\3\2\2\2-\5\3\2\2\2./\5\32\16\2/\60\7\13\2\2\60\61")
        buf.write("\5\32\16\2\61\7\3\2\2\2\62\63\7\36\2\2\63\64\7\n\2\2\64")
        buf.write("\65\5\n\6\2\65\t\3\2\2\2\66>\5\f\7\2\67>\5\16\b\28>\5")
        buf.write("\20\t\29>\5\22\n\2:>\5\24\13\2;>\5\26\f\2<>\5\30\r\2=")
        buf.write("\66\3\2\2\2=\67\3\2\2\2=8\3\2\2\2=9\3\2\2\2=:\3\2\2\2")
        buf.write("=;\3\2\2\2=<\3\2\2\2>\13\3\2\2\2?@\7\27\2\2@\r\3\2\2\2")
        buf.write("AB\7\30\2\2B\17\3\2\2\2CD\7\3\2\2DE\7\b\2\2EF\7\4\2\2")
        buf.write("FG\7\t\2\2G\21\3\2\2\2HI\7\31\2\2IJ\7\b\2\2JK\5\36\20")
        buf.write("\2KL\7\t\2\2L\23\3\2\2\2MN\7\32\2\2NO\7\b\2\2OP\5 \21")
        buf.write("\2PQ\7\t\2\2Q\25\3\2\2\2RS\7\33\2\2SU\7\b\2\2TV\5\4\3")
        buf.write("\2UT\3\2\2\2VW\3\2\2\2WU\3\2\2\2WX\3\2\2\2XY\3\2\2\2Y")
        buf.write("Z\7\t\2\2Z\27\3\2\2\2[\\\7\33\2\2\\]\7\b\2\2]^\7\34\2")
        buf.write("\2^_\7\t\2\2_\31\3\2\2\2`c\5\34\17\2ab\7\7\2\2bd\7\36")
        buf.write("\2\2ca\3\2\2\2cd\3\2\2\2d\33\3\2\2\2eh\7\36\2\2fh\5\n")
        buf.write("\6\2ge\3\2\2\2gf\3\2\2\2h\35\3\2\2\2ij\t\2\2\2j\37\3\2")
        buf.write("\2\2kl\t\3\2\2l!\3\2\2\2\b%,=Wcg")
        return buf.getvalue()


class GravisParser ( Parser ):

    grammarFileName = "Gravis.g4"

    atn = ATNDeserializer().deserialize(serializedATN())

    decisionsToDFA = [ DFA(ds, i) for i, ds in enumerate(atn.decisionToState) ]

    sharedContextCache = PredictionContextCache()

    literalNames = [ "<INVALID>", "'const'", "<INVALID>", "<INVALID>", "<INVALID>", 
                     "'.'", "'['", "']'", "'='", "'>>'", "'+'", "'-'", "'*'", 
                     "'/'", "'%'", "'<'", "'>'", "'=='", "'>='", "'<='", 
                     "'!='", "'input'", "'output'", "'if'", "'opr'", "'subspace'", 
                     "'self'" ]

    symbolicNames = [ "<INVALID>", "<INVALID>", "NUMBER", "INTEGER", "FLOAT", 
                      "DOT", "OPEN_PAREN", "CLOSE_PAREN", "ASSIGN", "RIGHT_SHIFT", 
                      "ADD", "MINUS", "MUL", "DIV", "MOD", "LESS_THAN", 
                      "GREATER_THAN", "EQUALS", "GT_EQ", "LT_EQ", "NOT_EQ", 
                      "INPUT", "OUTPUT", "IF", "OPR", "SUBSPACE", "SELF", 
                      "WHITESPACE", "NAME" ]

    RULE_file_input = 0
    RULE_stmt = 1
    RULE_link_stmt = 2
    RULE_def_stmt = 3
    RULE_node_def = 4
    RULE_input_def = 5
    RULE_output_def = 6
    RULE_const_def = 7
    RULE_if_def = 8
    RULE_opr_def = 9
    RULE_subspace_def = 10
    RULE_self_subspace_def = 11
    RULE_dotted_name = 12
    RULE_node_inst = 13
    RULE_comp_op = 14
    RULE_arith_op = 15

    ruleNames =  [ "file_input", "stmt", "link_stmt", "def_stmt", "node_def", 
                   "input_def", "output_def", "const_def", "if_def", "opr_def", 
                   "subspace_def", "self_subspace_def", "dotted_name", "node_inst", 
                   "comp_op", "arith_op" ]

    EOF = Token.EOF
    T__0=1
    NUMBER=2
    INTEGER=3
    FLOAT=4
    DOT=5
    OPEN_PAREN=6
    CLOSE_PAREN=7
    ASSIGN=8
    RIGHT_SHIFT=9
    ADD=10
    MINUS=11
    MUL=12
    DIV=13
    MOD=14
    LESS_THAN=15
    GREATER_THAN=16
    EQUALS=17
    GT_EQ=18
    LT_EQ=19
    NOT_EQ=20
    INPUT=21
    OUTPUT=22
    IF=23
    OPR=24
    SUBSPACE=25
    SELF=26
    WHITESPACE=27
    NAME=28

    def __init__(self, input:TokenStream, output:TextIO = sys.stdout):
        super().__init__(input, output)
        self.checkVersion("4.7.2")
        self._interp = ParserATNSimulator(self, self.atn, self.decisionsToDFA, self.sharedContextCache)
        self._predicates = None




    class File_inputContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def EOF(self):
            return self.getToken(GravisParser.EOF, 0)

        def stmt(self, i:int=None):
            if i is None:
                return self.getTypedRuleContexts(GravisParser.StmtContext)
            else:
                return self.getTypedRuleContext(GravisParser.StmtContext,i)


        def getRuleIndex(self):
            return GravisParser.RULE_file_input

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterFile_input" ):
                listener.enterFile_input(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitFile_input" ):
                listener.exitFile_input(self)




    def file_input(self):

        localctx = GravisParser.File_inputContext(self, self._ctx, self.state)
        self.enterRule(localctx, 0, self.RULE_file_input)
        self._la = 0 # Token type
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 35
            self._errHandler.sync(self)
            _la = self._input.LA(1)
            while (((_la) & ~0x3f) == 0 and ((1 << _la) & ((1 << GravisParser.T__0) | (1 << GravisParser.INPUT) | (1 << GravisParser.OUTPUT) | (1 << GravisParser.IF) | (1 << GravisParser.OPR) | (1 << GravisParser.SUBSPACE) | (1 << GravisParser.NAME))) != 0):
                self.state = 32
                self.stmt()
                self.state = 37
                self._errHandler.sync(self)
                _la = self._input.LA(1)

            self.state = 38
            self.match(GravisParser.EOF)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class StmtContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def def_stmt(self):
            return self.getTypedRuleContext(GravisParser.Def_stmtContext,0)


        def link_stmt(self):
            return self.getTypedRuleContext(GravisParser.Link_stmtContext,0)


        def getRuleIndex(self):
            return GravisParser.RULE_stmt

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterStmt" ):
                listener.enterStmt(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitStmt" ):
                listener.exitStmt(self)




    def stmt(self):

        localctx = GravisParser.StmtContext(self, self._ctx, self.state)
        self.enterRule(localctx, 2, self.RULE_stmt)
        try:
            self.state = 42
            self._errHandler.sync(self)
            la_ = self._interp.adaptivePredict(self._input,1,self._ctx)
            if la_ == 1:
                self.enterOuterAlt(localctx, 1)
                self.state = 40
                self.def_stmt()
                pass

            elif la_ == 2:
                self.enterOuterAlt(localctx, 2)
                self.state = 41
                self.link_stmt()
                pass


        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Link_stmtContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def dotted_name(self, i:int=None):
            if i is None:
                return self.getTypedRuleContexts(GravisParser.Dotted_nameContext)
            else:
                return self.getTypedRuleContext(GravisParser.Dotted_nameContext,i)


        def RIGHT_SHIFT(self):
            return self.getToken(GravisParser.RIGHT_SHIFT, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_link_stmt

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterLink_stmt" ):
                listener.enterLink_stmt(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitLink_stmt" ):
                listener.exitLink_stmt(self)




    def link_stmt(self):

        localctx = GravisParser.Link_stmtContext(self, self._ctx, self.state)
        self.enterRule(localctx, 4, self.RULE_link_stmt)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 44
            self.dotted_name()
            self.state = 45
            self.match(GravisParser.RIGHT_SHIFT)
            self.state = 46
            self.dotted_name()
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Def_stmtContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def NAME(self):
            return self.getToken(GravisParser.NAME, 0)

        def ASSIGN(self):
            return self.getToken(GravisParser.ASSIGN, 0)

        def node_def(self):
            return self.getTypedRuleContext(GravisParser.Node_defContext,0)


        def getRuleIndex(self):
            return GravisParser.RULE_def_stmt

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterDef_stmt" ):
                listener.enterDef_stmt(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitDef_stmt" ):
                listener.exitDef_stmt(self)




    def def_stmt(self):

        localctx = GravisParser.Def_stmtContext(self, self._ctx, self.state)
        self.enterRule(localctx, 6, self.RULE_def_stmt)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 48
            self.match(GravisParser.NAME)
            self.state = 49
            self.match(GravisParser.ASSIGN)
            self.state = 50
            self.node_def()
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Node_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def input_def(self):
            return self.getTypedRuleContext(GravisParser.Input_defContext,0)


        def output_def(self):
            return self.getTypedRuleContext(GravisParser.Output_defContext,0)


        def const_def(self):
            return self.getTypedRuleContext(GravisParser.Const_defContext,0)


        def if_def(self):
            return self.getTypedRuleContext(GravisParser.If_defContext,0)


        def opr_def(self):
            return self.getTypedRuleContext(GravisParser.Opr_defContext,0)


        def subspace_def(self):
            return self.getTypedRuleContext(GravisParser.Subspace_defContext,0)


        def self_subspace_def(self):
            return self.getTypedRuleContext(GravisParser.Self_subspace_defContext,0)


        def getRuleIndex(self):
            return GravisParser.RULE_node_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterNode_def" ):
                listener.enterNode_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitNode_def" ):
                listener.exitNode_def(self)




    def node_def(self):

        localctx = GravisParser.Node_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 8, self.RULE_node_def)
        try:
            self.state = 59
            self._errHandler.sync(self)
            la_ = self._interp.adaptivePredict(self._input,2,self._ctx)
            if la_ == 1:
                self.enterOuterAlt(localctx, 1)
                self.state = 52
                self.input_def()
                pass

            elif la_ == 2:
                self.enterOuterAlt(localctx, 2)
                self.state = 53
                self.output_def()
                pass

            elif la_ == 3:
                self.enterOuterAlt(localctx, 3)
                self.state = 54
                self.const_def()
                pass

            elif la_ == 4:
                self.enterOuterAlt(localctx, 4)
                self.state = 55
                self.if_def()
                pass

            elif la_ == 5:
                self.enterOuterAlt(localctx, 5)
                self.state = 56
                self.opr_def()
                pass

            elif la_ == 6:
                self.enterOuterAlt(localctx, 6)
                self.state = 57
                self.subspace_def()
                pass

            elif la_ == 7:
                self.enterOuterAlt(localctx, 7)
                self.state = 58
                self.self_subspace_def()
                pass


        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Input_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def INPUT(self):
            return self.getToken(GravisParser.INPUT, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_input_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterInput_def" ):
                listener.enterInput_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitInput_def" ):
                listener.exitInput_def(self)




    def input_def(self):

        localctx = GravisParser.Input_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 10, self.RULE_input_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 61
            self.match(GravisParser.INPUT)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Output_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def OUTPUT(self):
            return self.getToken(GravisParser.OUTPUT, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_output_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterOutput_def" ):
                listener.enterOutput_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitOutput_def" ):
                listener.exitOutput_def(self)




    def output_def(self):

        localctx = GravisParser.Output_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 12, self.RULE_output_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 63
            self.match(GravisParser.OUTPUT)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Const_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def OPEN_PAREN(self):
            return self.getToken(GravisParser.OPEN_PAREN, 0)

        def NUMBER(self):
            return self.getToken(GravisParser.NUMBER, 0)

        def CLOSE_PAREN(self):
            return self.getToken(GravisParser.CLOSE_PAREN, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_const_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterConst_def" ):
                listener.enterConst_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitConst_def" ):
                listener.exitConst_def(self)




    def const_def(self):

        localctx = GravisParser.Const_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 14, self.RULE_const_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 65
            self.match(GravisParser.T__0)
            self.state = 66
            self.match(GravisParser.OPEN_PAREN)
            self.state = 67
            self.match(GravisParser.NUMBER)
            self.state = 68
            self.match(GravisParser.CLOSE_PAREN)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class If_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def IF(self):
            return self.getToken(GravisParser.IF, 0)

        def OPEN_PAREN(self):
            return self.getToken(GravisParser.OPEN_PAREN, 0)

        def comp_op(self):
            return self.getTypedRuleContext(GravisParser.Comp_opContext,0)


        def CLOSE_PAREN(self):
            return self.getToken(GravisParser.CLOSE_PAREN, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_if_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterIf_def" ):
                listener.enterIf_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitIf_def" ):
                listener.exitIf_def(self)




    def if_def(self):

        localctx = GravisParser.If_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 16, self.RULE_if_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 70
            self.match(GravisParser.IF)
            self.state = 71
            self.match(GravisParser.OPEN_PAREN)
            self.state = 72
            self.comp_op()
            self.state = 73
            self.match(GravisParser.CLOSE_PAREN)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Opr_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def OPR(self):
            return self.getToken(GravisParser.OPR, 0)

        def OPEN_PAREN(self):
            return self.getToken(GravisParser.OPEN_PAREN, 0)

        def arith_op(self):
            return self.getTypedRuleContext(GravisParser.Arith_opContext,0)


        def CLOSE_PAREN(self):
            return self.getToken(GravisParser.CLOSE_PAREN, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_opr_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterOpr_def" ):
                listener.enterOpr_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitOpr_def" ):
                listener.exitOpr_def(self)




    def opr_def(self):

        localctx = GravisParser.Opr_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 18, self.RULE_opr_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 75
            self.match(GravisParser.OPR)
            self.state = 76
            self.match(GravisParser.OPEN_PAREN)
            self.state = 77
            self.arith_op()
            self.state = 78
            self.match(GravisParser.CLOSE_PAREN)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Subspace_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def SUBSPACE(self):
            return self.getToken(GravisParser.SUBSPACE, 0)

        def OPEN_PAREN(self):
            return self.getToken(GravisParser.OPEN_PAREN, 0)

        def CLOSE_PAREN(self):
            return self.getToken(GravisParser.CLOSE_PAREN, 0)

        def stmt(self, i:int=None):
            if i is None:
                return self.getTypedRuleContexts(GravisParser.StmtContext)
            else:
                return self.getTypedRuleContext(GravisParser.StmtContext,i)


        def getRuleIndex(self):
            return GravisParser.RULE_subspace_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterSubspace_def" ):
                listener.enterSubspace_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitSubspace_def" ):
                listener.exitSubspace_def(self)




    def subspace_def(self):

        localctx = GravisParser.Subspace_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 20, self.RULE_subspace_def)
        self._la = 0 # Token type
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 80
            self.match(GravisParser.SUBSPACE)
            self.state = 81
            self.match(GravisParser.OPEN_PAREN)
            self.state = 83 
            self._errHandler.sync(self)
            _la = self._input.LA(1)
            while True:
                self.state = 82
                self.stmt()
                self.state = 85 
                self._errHandler.sync(self)
                _la = self._input.LA(1)
                if not ((((_la) & ~0x3f) == 0 and ((1 << _la) & ((1 << GravisParser.T__0) | (1 << GravisParser.INPUT) | (1 << GravisParser.OUTPUT) | (1 << GravisParser.IF) | (1 << GravisParser.OPR) | (1 << GravisParser.SUBSPACE) | (1 << GravisParser.NAME))) != 0)):
                    break

            self.state = 87
            self.match(GravisParser.CLOSE_PAREN)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Self_subspace_defContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def SUBSPACE(self):
            return self.getToken(GravisParser.SUBSPACE, 0)

        def OPEN_PAREN(self):
            return self.getToken(GravisParser.OPEN_PAREN, 0)

        def SELF(self):
            return self.getToken(GravisParser.SELF, 0)

        def CLOSE_PAREN(self):
            return self.getToken(GravisParser.CLOSE_PAREN, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_self_subspace_def

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterSelf_subspace_def" ):
                listener.enterSelf_subspace_def(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitSelf_subspace_def" ):
                listener.exitSelf_subspace_def(self)




    def self_subspace_def(self):

        localctx = GravisParser.Self_subspace_defContext(self, self._ctx, self.state)
        self.enterRule(localctx, 22, self.RULE_self_subspace_def)
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 89
            self.match(GravisParser.SUBSPACE)
            self.state = 90
            self.match(GravisParser.OPEN_PAREN)
            self.state = 91
            self.match(GravisParser.SELF)
            self.state = 92
            self.match(GravisParser.CLOSE_PAREN)
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Dotted_nameContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def node_inst(self):
            return self.getTypedRuleContext(GravisParser.Node_instContext,0)


        def DOT(self):
            return self.getToken(GravisParser.DOT, 0)

        def NAME(self):
            return self.getToken(GravisParser.NAME, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_dotted_name

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterDotted_name" ):
                listener.enterDotted_name(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitDotted_name" ):
                listener.exitDotted_name(self)




    def dotted_name(self):

        localctx = GravisParser.Dotted_nameContext(self, self._ctx, self.state)
        self.enterRule(localctx, 24, self.RULE_dotted_name)
        self._la = 0 # Token type
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 94
            self.node_inst()
            self.state = 97
            self._errHandler.sync(self)
            _la = self._input.LA(1)
            if _la==GravisParser.DOT:
                self.state = 95
                self.match(GravisParser.DOT)
                self.state = 96
                self.match(GravisParser.NAME)


        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Node_instContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def NAME(self):
            return self.getToken(GravisParser.NAME, 0)

        def node_def(self):
            return self.getTypedRuleContext(GravisParser.Node_defContext,0)


        def getRuleIndex(self):
            return GravisParser.RULE_node_inst

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterNode_inst" ):
                listener.enterNode_inst(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitNode_inst" ):
                listener.exitNode_inst(self)




    def node_inst(self):

        localctx = GravisParser.Node_instContext(self, self._ctx, self.state)
        self.enterRule(localctx, 26, self.RULE_node_inst)
        try:
            self.state = 101
            self._errHandler.sync(self)
            token = self._input.LA(1)
            if token in [GravisParser.NAME]:
                self.enterOuterAlt(localctx, 1)
                self.state = 99
                self.match(GravisParser.NAME)
                pass
            elif token in [GravisParser.T__0, GravisParser.INPUT, GravisParser.OUTPUT, GravisParser.IF, GravisParser.OPR, GravisParser.SUBSPACE]:
                self.enterOuterAlt(localctx, 2)
                self.state = 100
                self.node_def()
                pass
            else:
                raise NoViableAltException(self)

        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Comp_opContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def LESS_THAN(self):
            return self.getToken(GravisParser.LESS_THAN, 0)

        def GREATER_THAN(self):
            return self.getToken(GravisParser.GREATER_THAN, 0)

        def EQUALS(self):
            return self.getToken(GravisParser.EQUALS, 0)

        def GT_EQ(self):
            return self.getToken(GravisParser.GT_EQ, 0)

        def LT_EQ(self):
            return self.getToken(GravisParser.LT_EQ, 0)

        def NOT_EQ(self):
            return self.getToken(GravisParser.NOT_EQ, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_comp_op

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterComp_op" ):
                listener.enterComp_op(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitComp_op" ):
                listener.exitComp_op(self)




    def comp_op(self):

        localctx = GravisParser.Comp_opContext(self, self._ctx, self.state)
        self.enterRule(localctx, 28, self.RULE_comp_op)
        self._la = 0 # Token type
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 103
            _la = self._input.LA(1)
            if not((((_la) & ~0x3f) == 0 and ((1 << _la) & ((1 << GravisParser.LESS_THAN) | (1 << GravisParser.GREATER_THAN) | (1 << GravisParser.EQUALS) | (1 << GravisParser.GT_EQ) | (1 << GravisParser.LT_EQ) | (1 << GravisParser.NOT_EQ))) != 0)):
                self._errHandler.recoverInline(self)
            else:
                self._errHandler.reportMatch(self)
                self.consume()
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx


    class Arith_opContext(ParserRuleContext):

        def __init__(self, parser, parent:ParserRuleContext=None, invokingState:int=-1):
            super().__init__(parent, invokingState)
            self.parser = parser

        def ADD(self):
            return self.getToken(GravisParser.ADD, 0)

        def MINUS(self):
            return self.getToken(GravisParser.MINUS, 0)

        def MUL(self):
            return self.getToken(GravisParser.MUL, 0)

        def DIV(self):
            return self.getToken(GravisParser.DIV, 0)

        def MOD(self):
            return self.getToken(GravisParser.MOD, 0)

        def getRuleIndex(self):
            return GravisParser.RULE_arith_op

        def enterRule(self, listener:ParseTreeListener):
            if hasattr( listener, "enterArith_op" ):
                listener.enterArith_op(self)

        def exitRule(self, listener:ParseTreeListener):
            if hasattr( listener, "exitArith_op" ):
                listener.exitArith_op(self)




    def arith_op(self):

        localctx = GravisParser.Arith_opContext(self, self._ctx, self.state)
        self.enterRule(localctx, 30, self.RULE_arith_op)
        self._la = 0 # Token type
        try:
            self.enterOuterAlt(localctx, 1)
            self.state = 105
            _la = self._input.LA(1)
            if not((((_la) & ~0x3f) == 0 and ((1 << _la) & ((1 << GravisParser.ADD) | (1 << GravisParser.MINUS) | (1 << GravisParser.MUL) | (1 << GravisParser.DIV) | (1 << GravisParser.MOD))) != 0)):
                self._errHandler.recoverInline(self)
            else:
                self._errHandler.reportMatch(self)
                self.consume()
        except RecognitionException as re:
            localctx.exception = re
            self._errHandler.reportError(self, re)
            self._errHandler.recover(self, re)
        finally:
            self.exitRule()
        return localctx





