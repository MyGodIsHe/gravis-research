SUBSPACE_STACK = []
DEBUG_STACK = []


def current_subspace():
    return SUBSPACE_STACK[-1] if SUBSPACE_STACK else None


def current_debug():
    return DEBUG_STACK[-1] if DEBUG_STACK else None
