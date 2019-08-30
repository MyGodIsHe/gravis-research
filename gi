#!/usr/bin/env python
import argparse
import sys

import gravis


parser = argparse.ArgumentParser(description='Gravis Interpreter.')
parser.add_argument('-p', '--profiling', action='store_true')
parser.add_argument('file', metavar='file',
                    help='program read from script file')
parser.add_argument('argument', metavar='arg',
                    help='argument passed to input node')
args = parser.parse_args()


def parse_arg(arg):
    try:
        return int(arg)
    except ValueError:
        pass
    try:
        return float(arg)
    except ValueError:
        pass
    return arg


def profiling_render(digraph):
    try:
        import graphviz
    except ImportError:
        with open('profiling.gv', 'w') as f:
            f.write(digraph)
        print('DEBUG: profiling.gv was created')
    else:
        dot = graphviz.Source(digraph)
        dot.render('profiling.gv', view=True, format='svg')
        print('DEBUG: profiling.gv and profiling.gv.svg were created')


with open(args.file) as f:
    with gravis.Subspace() as main:
        gravis.parse(f.read())
    if not main.input:
        print('Error: need input node')
        sys.exit(1)
    if not main.output:
        print('Error: need output node')
        sys.exit(1)
    if args.profiling:
        with gravis.DebugContext() as debug:
            main.activate(parse_arg(args.argument), None)
        digraph = debug.create_digraph()
        profiling_render(digraph)
    else:
        main.activate(parse_arg(args.argument), None)
    print(main.saved_value)
