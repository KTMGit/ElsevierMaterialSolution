using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Common.Interfaces {
    public interface IStackNavigation {
        // Summary:
        //     Returns the ordered list of items that are currently in navigation container
        //     from the oldest one to the youngest one.
        //
        // Returns:
        //     ordered list of navigable objects in collection
        IList<INavigable> GetOrderedItems();
        //
        // Summary:
        //     Jumps to the selected element on the navigation stack. All the elements after
        //     this element are removed from the stack.  and this element becomes inactive
        //
        // Parameters:
        //   navigableID:
        //     the id of the element to jump to
        void JumpTo(string navigableID);
        //
        // Summary:
        //     Returns the top of the stack without removing it from the stack.
        //
        // Returns:
        //     INavigable from the top of the stack
        INavigable Peek();
        //
        // Summary:
        //     Pops the INavigable from the top of the navigation stack.
        //
        // Returns:
        //     The INavigable that is removed from the stack
        INavigable Pop();
        //
        // Summary:
        //     Replaces the last element in the navigation stack with the provided one.
        //
        // Parameters:
        //   navigable:
        //     the new leaf in the navigation stach
        void PopPush(INavigable navigable);
        //
        // Summary:
        //     Puts element to the top of the stack.
        //
        // Parameters:
        //   navigable:
        //     the INavigable to put on top of the stack
        void Push(INavigable navigable);
    }
}
