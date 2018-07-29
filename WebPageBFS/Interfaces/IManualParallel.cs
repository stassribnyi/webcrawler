using System.Threading.Tasks;

using WebPageBFS.Models;

namespace WebPageBFS.Interfaces
{
    /// <summary>
    /// Interface describing manual parallel service
    /// </summary>
    public interface IManualParallel
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        ManualParallelState State { get; }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        void Pause();

        /// <summary>
        /// Starts or resumes this instance.
        /// </summary>
        Task Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
