grammar Gravis;

/*
 * parser rules
 */

file_input: stmt* EOF;

stmt: def_stmt | link_stmt;
link_stmt: dotted_name  '>>' dotted_name;
def_stmt: NAME  '=' node_def;
node_def
 : input_def
 | output_def
 | const_def
 | if_def
 | opr_def
 | subspace_def
 | self_subspace_def
 ;
input_def: 'input';
output_def: 'output';
const_def: 'const' '[' NUMBER ']';
if_def: 'if' '[' comp_op ']';
opr_def: 'opr' '[' arith_op ']';
subspace_def: 'subspace' '[' stmt+ ']';
self_subspace_def: 'subspace' '[' 'self' ']';
dotted_name: node_inst ('.' NAME)?;
node_inst: NAME | node_def;
comp_op: '<'|'>'|'=='|'>='|'<='|'!=';
arith_op: '+'|'-'|'*'|'/'|'%';

/*
 * lexer rules
 */

NUMBER
 : INTEGER
 | FLOAT
 ;

INTEGER
 : NON_ZERO_DIGIT DIGIT*
 | '0'+
 ;

fragment NON_ZERO_DIGIT
 : [1-9]
 ;

fragment DIGIT
 : [0-9]
 ;

FLOAT
 : INT_PART FRACTION
 ;

fragment INT_PART
 : DIGIT+
 ;

fragment FRACTION
 : '.' DIGIT+
 ;

DOT : '.';
OPEN_PAREN : '[';
CLOSE_PAREN : ']';
ASSIGN : '=';
RIGHT_SHIFT : '>>';
ADD : '+';
MINUS : '-';
MUL : '*';
DIV : '/';
MOD : '%';
LESS_THAN : '<';
GREATER_THAN : '>';
EQUALS : '==';
GT_EQ : '>=';
LT_EQ : '<=';
NOT_EQ : '!=';
INPUT : 'input';
OUTPUT : 'output';
IF : 'if';
OPR : 'opr';
SUBSPACE : 'subspace';
SELF : 'self';

WHITESPACE
 : ( SPACES | COMMENT ) -> skip
 ;

fragment SPACES
 : [ \t\n\r]+
 ;

fragment COMMENT
 : '#' ~[\r\n\f]*
 ;

NAME
 : ID_START ID_CONTINUE*
 ;

fragment ID_START
 : '_'
 | [A-Z]
 | [a-z]
 ;

fragment ID_CONTINUE
 : ID_START
 | [0-9]
 ;
