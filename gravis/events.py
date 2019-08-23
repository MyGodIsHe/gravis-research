from functools import wraps

__all__ = (
    'event_activate',
    'event_activate_me',
)


_EVENT_SUBSCRIBERS = {
    'activate': set(),
    'activate_me': set(),
}


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
