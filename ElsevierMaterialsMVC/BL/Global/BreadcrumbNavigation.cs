using ElsevierMaterials.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class BreadcrumbNavigation : IStackNavigation {

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadcrumbNavigation"/> class.
        /// </summary>
        public BreadcrumbNavigation() {
            NavigationStack = new Stack<INavigable>();
        }

        /// <summary>
        /// Gets or sets the navigation stack.
        /// </summary>
        /// <value>
        /// The navigation stack.
        /// </value>
        protected Stack<INavigable> NavigationStack { get; set; }

        public string LastNavigable { get; set; }

        /// <summary>
        /// Gets the <i>INavigable</i> object from the top of the stack without doing actual removal of the object from the top of the stack.
        /// </summary>
        /// <returns><i>INavigable</i> that is currently on the top of the stack</returns>
        public INavigable Peek() {
            return NavigationStack.Peek();
        }

        /// <summary>
        /// Jumps to the selected navigable by removing one by one object from the top of the stack until it comes to the desired one.
        /// </summary>
        /// <param name="navigableID">The navigable ID to jump to.</param>
        public void JumpTo(string navigableID) {
            while (true) {
                INavigable top = this.Peek();
                if (top == null)
                    break;
                if (string.Equals(top.NavigableID, navigableID)) {
                    top.IsActive = false;
                    return;
                }
                this.Pop();
            }
        }

        /// <summary>
        /// Removes one object from the top of the stack and returns its instance to the calling code.
        /// </summary>
        /// <returns>instance of the <i>INavigable</i> interface from the top of the stack</returns>
        public INavigable Pop() {
            return NavigationStack.Pop();
        }

        /// <summary>
        /// Pops the object from the top of the stack and then pushes the provided one to the top of the stack.
        /// </summary>
        /// <param name="navigable">The navigable to push.</param>
        public void PopPush(INavigable navigable) {
            NavigationStack.Pop();
            Push(navigable);
        }

        /// <summary>
        /// Pushes the specified navigable to the top of the stack.
        /// </summary>
        /// <param name="navigable">The navigable to push to the top ofthe stack</param>
        public void Push(INavigable navigable) {
            if (!NavigationStack.ToList().Any(nav => nav.NavigableID == navigable.NavigableID)) {
                NavigationStack.Push(navigable);
            }
        }

        /// <summary>
        /// Gets the list of the items in the stack, starting from the bottom of the stack.
        /// </summary>
        /// <returns>the list of the items in the stack</returns>
        public IList<INavigable> GetOrderedItems() {
            return NavigationStack.ToList();
        }

        /// <summary>
        /// Removes all objects from the stack.
        /// </summary>
        public void Clear() {
            NavigationStack.Clear();
        }

        /// <summary>
        /// Determines whether navigation contains navigable with the specified navigable id.
        /// </summary>
        /// <param name="navigableId">The navigable id.</param>
        /// <returns>
        ///   <c>true</c> if naviagtionb contains navigable with the specified navigable id; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string navigableId) {
            return NavigationStack.ToList().Where(nav => nav.NavigableID == navigableId).Any();
        }

        public string GetNavigableId<T>() {
            foreach (BaseNavigablePage item in NavigationStack.ToList()) {
                if (item is T) {
                    return item.NavigableID;
                }
            }
            return null;
        }
    }
}