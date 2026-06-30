using BehaviourModule.Nodes;

namespace BehaviourModule.Interfaces
{
    /// <summary>
    /// Defines the classes that can be visualized inside <see cref="BehaviourModule.TreeVisualizer"/>
    /// </summary>
    public interface IVisualizable
    {
        /// <summary>
        /// Fetches the root of this object
        /// </summary>
        Node GetRoot();

        /// <summary>
        /// Rebuilds the root of this object
        /// </summary>
        void RebuildRoot();
    }
}