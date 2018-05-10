using System;


{

    public sealed class CommandManager
    {
        #region Static Features 

        #region Public Events 

   =

 
        public static void AddPreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.AddHandler(PreviewExecutedEvent, handler);
        }

      
        public static void RemovePreviewExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.RemoveHandler(PreviewExecutedEvent, handler);
        }

        public static readonly RoutedEvent ExecutedEvent =
                EventManager.RegisterRoutedEvent("Executed",
                                                 RoutingStrategy.Bubble,
                                                 typeof(ExecutedRoutedEventHandler),
                                                 typeof(CommandManager));


        public static void AddExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.AddHandler(ExecutedEvent, handler);
        }


        public static void RemoveExecutedHandler(UIElement element, ExecutedRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.RemoveHandler(ExecutedEvent, handler);
        }

        public static readonly RoutedEvent PreviewCanExecuteEvent =
                EventManager.RegisterRoutedEvent("PreviewCanExecute",
                                                 RoutingStrategy.Tunnel,
                                                 typeof(CanExecuteRoutedEventHandler),
                                                 typeof(CommandManager));

       
        public static void AddPreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.AddHandler(PreviewCanExecuteEvent, handler);
        }

        public static void RemovePreviewCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.RemoveHandler(PreviewCanExecuteEvent, handler);
        }

   
        public static readonly RoutedEvent CanExecuteEvent =
                EventManager.RegisterRoutedEvent("CanExecute",
                                                 RoutingStrategy.Bubble,
                                                 typeof(CanExecuteRoutedEventHandler),
                                                 typeof(CommandManager));
        
        public static void AddCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.AddHandler(CanExecuteEvent, handler);
        }

        public static void RemoveCanExecuteHandler(UIElement element, CanExecuteRoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            element.RemoveHandler(CanExecuteEvent, handler);
        }

   