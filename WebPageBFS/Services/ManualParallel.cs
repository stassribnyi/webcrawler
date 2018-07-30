using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Services
{
    /// <summary>
    /// Class encapsulating implementation of manual parallel service.
    /// </summary>
    /// <seealso cref="WebPageBFS.Interfaces.IManualParallel" />
    public class ManualParallel : IManualParallel
    {
        /// <summary>
        /// The lowest break index lock
        /// </summary>
        private readonly object _lowestBreakIndexLock = new object();

        /// <summary>
        /// The lowest break index
        /// </summary>
        private long? _lowestBreakIndex = null;

        /// <summary>
        /// The state lock
        /// </summary>
        private readonly object _stateLock = new object();

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
        /// Gets the index of the lowest break.
        /// </summary>
        public long? LowestBreakIndex
        {
            get
            {
                lock (_lowestBreakIndexLock)
                {
                    return _lowestBreakIndex;
                }
            }
            private set
            {
                lock (_lowestBreakIndexLock)
                {
                    _lowestBreakIndex = value;
                }
            }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public ManualParallelState State
        {
            get
            {
                lock (_stateLock)
                {
                    return _state;
                }
            }
            private set
            {
                lock (_stateLock)
                {
                    _state = value;
                }
            }
        }

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
        /// Starts this instance.
        /// </summary>
        public async Task Start()
        {
            await Task.Run(() =>
            {
                _state = ManualParallelState.Processing;
                var itemsToProcess = _actions.Skip((int)(LowestBreakIndex ?? 0));

                ParallelLoopResult result = Parallel.ForEach(itemsToProcess, _options, ActionWrapper);

                LowestBreakIndex = result.LowestBreakIteration;

                _state = LowestBreakIndex == null
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
