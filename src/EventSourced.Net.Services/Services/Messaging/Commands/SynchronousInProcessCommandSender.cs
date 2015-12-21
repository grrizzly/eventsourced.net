﻿using System.Threading.Tasks;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Commands
{
  public class SynchronousInProcessCommandSender : ISendCommand
  {
    private readonly Container _container;

    public SynchronousInProcessCommandSender(Container container) {
      _container = container;
    }

    public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand {
      var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());
      dynamic handler = _container.GetInstance(handlerType);
      await handler.HandleAsync((dynamic)command).ConfigureAwait(false);
    }
  }
}
