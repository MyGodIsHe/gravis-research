from functools import wraps

__all__ = (
    'event_activate',
    'event_activate_me',
    'event_init',
    'event_connect',
)


_EVENT_SUBSCRIBERS = {
    'activate': set(),
    'activate_me': set(),
    '__init__': set(),
    '__rshift__': set(),
}
EVENT_METHODS = tuple(_EVENT_SUBSCRIBERS)


def event_wrapper(func):

    @wraps(func)
    def wrapper(*args, **kwargs):
        for event in _EVENT_SUBSCRIBERS[func.__name__]:
            event(*args, **kwargs)
        return func(*args, **kwargs)

    return wrapper


def event_activate(func):
    _EVENT_SUBSCRIBERS['activate'].add(func)
    return func


def event_activate_me(func):
    _EVENT_SUBSCRIBERS['activate_me'].add(func)
    return func


def event_init(func):
    _EVENT_SUBSCRIBERS['__init__'].add(func)
    return func


def event_connect(func):
    _EVENT_SUBSCRIBERS['__rshift__'].add(func)
    return func
