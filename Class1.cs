InputBindingCollection localInputBindings = null;
            if (isUIElement)
            { 
                localInputBindings = ((UIElement) targetElement).InputBindingsInternal;
            } 
            else if (isContentElement) 
            {
                localInputBindings = ((ContentElement) targetElement).InputBindingsInternal; 
            }
            else if (isUIElement3D)
            {
                localInputBindings = ((UIElement3D) targetElement).InputBindingsInternal; 
            }
            if (localInputBindings != null) 
            { 
                InputBinding inputBinding = localInputBindings.FindMatch(targetElement, inputEventArgs);
                if (inputBinding != null) 
                {
                    command = inputBinding.Command;
                    target = inputBinding.CommandTarget;
                    parameter = inputBinding.CommandParameter; 
                }
            } 
  
            if (command == null) 
            {
                lock (_classInputBindings.SyncRoot)
                {
                    Type classType = targetElement.GetType(); 
                    while (classType != null)
                    { 
                        InputBindingCollection classInputBindings = _classInputBindings[classType] as InputBindingCollection; 
                        if (classInputBindings != null)
                        { 
                            InputBinding inputBinding = classInputBindings.FindMatch(targetElement, inputEventArgs);
                            if (inputBinding != null)
                            {
                                command = inputBinding.Command; 
                                target = inputBinding.CommandTarget;
                                parameter = inputBinding.CommandParameter; 
                                break; 
                            }
                        } 
                        classType = classType.BaseType;
                    }
                }
            } 
 
 
            if (command == null) 
            {
       
                CommandBindingCollection localCommandBindings = null;
                if (isUIElement)
                {
                    localCommandBindings = ((UIElement) targetElement).CommandBindingsInternal; 
                }
                else if (isContentElement) 
                { 
                    localCommandBindings = ((ContentElement) targetElement).CommandBindingsInternal;
                } 
                else if (isUIElement3D)
                {
                    localCommandBindings = ((UIElement3D) targetElement).CommandBindingsInternal;
                } 
                if (localCommandBindings != null)
                { 
                    command = localCommandBindings.FindMatch(targetElement, inputEventArgs); 
                }
            } 
 
            if (command == null)
            { 
                lock (_classCommandBindings.SyncRoot)
                { 
                    Type classType = targetElement.GetType(); 
                    while (classType != null)
                    { 
                        CommandBindingCollection classCommandBindings = _classCommandBindings[classType] as CommandBindingCollection;
                        if (classCommandBindings != null)
                        {
                            command = classCommandBindings.FindMatch(targetElement, inputEventArgs); 
                            if (command != null)
                            { 
                                break; 
                            }
                        } 
                        classType = classType.BaseType;
                    }
                }
            } 
 
         
            if (command != null && command != ApplicationCommands.NotACommand) 
            {
           
                if (target == null)
                { 
                    target = targetElement; 
                }
  
                bool continueRouting = false;

RoutedCommand routedCommand = command as RoutedCommand;
                if (routedCommand != null) 
                {
                    if (routedCommand.CriticalCanExecute(parameter,
                                                    target,
                                                    inputEventArgs.UserInitiated ,
                                                    out continueRouting)) 
                    {
                        ExecuteCommand(routedCommand, parameter, target, inputEventArgs);
                    }
                } 
                else
                { 
                    if (command.CanExecute(parameter)) 
                    {
                        command.Execute(parameter); 
                    }
                }
