using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Inputs
{
    public class InputService : ITickable
    {
        private readonly IDictionary<KeyCode, Action> _actions = new Dictionary<KeyCode, Action>();

        public void Register(KeyCode key, Action action)
        {
            _actions.Add(key, action);
        }

        public void Unregister(KeyCode key)
        {
            _actions.Remove(key);
        }

        public void Tick()
        {
            foreach (var (key, action) in _actions)
            {
                if (Input.GetKeyDown(key))
                {
                    action.Invoke();
                }
            }
        }
    }
}