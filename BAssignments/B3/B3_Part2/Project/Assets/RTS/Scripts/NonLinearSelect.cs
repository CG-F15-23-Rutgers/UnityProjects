using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TreeSharpPlus
{
    /// <summary>
    /// The base selector class. This will attempt to execute all branches of logic, until one succeeds. 
    /// This composite will fail only if all branches fail as well.
    /// </summary>
    public class NonLinearSelect : NodeGroup
    {
        public NonLinearSelect(params Node[] children)
            : base(children)
        {
        }

        public override IEnumerable<RunStatus> Execute()
        {          
           while(true)
            {
                // Move to the next node
                int val = Random.Range(0, Children.Count);
                Node node = Children[val];
                this.Selection = Children[val];
                
                node.Start();

                // If the current node is still running, report that. Don't 'break' the enumerator
                RunStatus result;
                while ((result = this.TickNode(node)) == RunStatus.Running)
                    yield return RunStatus.Running;

                // Call Stop to allow the node to clean anything up.
                node.Stop();

                // Clear the selection
                this.Selection.ClearLastStatus();
                this.Selection = null;

                // If it succeeded, we return success without trying any subsequent nodes
                if (result == RunStatus.Success)
                {
                    yield return RunStatus.Success;
                    yield break;
                }

                // Otherwise, we're still running
                yield return RunStatus.Running;
            }

            // We ran out of children, and none succeeded. Return failed.
            yield return RunStatus.Failure;
            yield break;
        }
    }
}