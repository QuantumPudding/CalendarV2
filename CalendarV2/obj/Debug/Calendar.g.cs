﻿

#pragma checksum "d:\users\tom\documents\visual studio 2012\Projects\CalendarV2\CalendarV2\Calendar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9B8101F520FFF9A86D6B898BF5D3A8F4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CalendarV2
{
    partial class Calendar : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 48 "..\..\Calendar.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.OnNextYearTapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 58 "..\..\Calendar.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.OnPreviousYearTapped;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 139 "..\..\Calendar.xaml"
                ((global::Windows.UI.Xaml.Controls.ScrollViewer)(target)).ViewChanged += this.OnCalendarScroll;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 176 "..\..\Calendar.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.OnNotesTapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

