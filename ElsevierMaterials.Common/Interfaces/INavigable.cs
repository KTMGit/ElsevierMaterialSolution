using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Common.Interfaces {
    public interface INavigable {
        // Summary:
        //     Determines whether INavigable is active or not (can be clicked or not).
        bool IsActive { get; set; }
        //
        // Summary:
        //     The system ID of the navigable object.
        string NavigableID { get; set; }
        //
        // Summary:
        //     The string representation that will be displayed when navigation is constructed.
        string NavigableName { get; set; }
        //
        // Summary:
        //     Determines whether INavigable should replace the last item that participate
        //     in the navigation (first pop of the last item is done and then push of this
        //     item is performed).
        bool ReplaceLast { get; set; }
        bool IsVisible { get; set; }
    }
}
