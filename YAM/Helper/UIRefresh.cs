using System;
using System.Windows;
using System.Windows.Threading;

namespace YAM
{
    public static class UIRefresh
    {
        private static Action EmptyDelegate = delegate() { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
