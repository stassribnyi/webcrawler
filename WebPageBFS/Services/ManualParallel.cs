using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Services
{
    public class ManualParallel : IManualParallel
    {
        /// <summary>
        /// The lowest break index
        /// </summary>
        private long? _lowestBreakIndex = null;

        /// <summary>
        /// The state
        /// </summary>
        private ManualParallelState _state = ManualParallelState.None;

        /// <summary>
        /// The actions
        /// </summary>
        private readonly ICollection<Action> _actions;

        /// <summary>
        /// The options
        /// </summary>
        private readonly ParallelOptions _options;

        /// <summary>
        /// Gets the state.
        /// </summary>
        public ManualParallelState State { get { return _state; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualParallel"/> class.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <param name="options">The options.</param>
        public ManualParallel(ICollection<Action> actions, ParallelOptions options = null)
        {
            _actions = actions;
            _options = options ?? new ParallelOptions();
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            _state = ManualParallelState.Paused;
        }

        /// <summary>
        /// Starts or resumes this instance.
        /// </summary>
        public async void Start()
        {
            await Task.Run(() =>
            {
                _state = ManualParallelState.Processing;
                var itemsToProcess = _actions.Skip((int)(_lowestBreakIndex ?? 0));

                ParallelLoopResult result = Parallel.ForEach(itemsToProcess, _options, ActionWrapper);

                _lowestBreakIndex = result.LowestBreakIteration;

                _state = _lowestBreakIndex == null
                    ? ManualParallelState.None
                    : _state;
            });
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _state = ManualParallelState.Terminated;
        }

        /// <summary>
        /// Actions the wrapper.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="state">The state.</param>
        private void ActionWrapper(Action action, ParallelLoopState state)
        {
            switch (_state)
            {
                case ManualParallelState.Paused:
                    state.Break();
                    break;
                case ManualParallelState.Processing:
                    action();
                    break;
                case ManualParallelState.Terminated:
                    state.Stop();
                    break;
                default:
                    break;
            }
        }
    }
}
