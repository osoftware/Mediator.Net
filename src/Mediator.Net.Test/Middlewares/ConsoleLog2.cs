﻿using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Mediator.Net.Test.Middlewares
{
    static class ConsoleLog2
    {
        public static void UseConsoleLogger2<TContext, TMessage>(this IPipeConfigurator<TContext, TMessage> configurator)
            where TContext : IContext<TMessage>
            where TMessage : IMessage
        {
            configurator.AddPipeSpecification(new ConsoleLogSpecification2<TContext, TMessage>());
        }
    }

    class ConsoleLogSpecification2<TContext, TMessage> : IPipeSpecification<TContext, TMessage> where TMessage : IMessage where TContext : IContext<TMessage>
    {
        public bool ShouldExecute(TContext context)
        {
            return true;

        }

        public Task ExecuteBeforeConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("Before 2");
            return Task.FromResult(0);

        }

        public Task ExecuteAfterConnect(TContext context)
        {
            if (ShouldExecute(context))
                Console.WriteLine("After 2");
            return Task.FromResult(0);
        }
    }
}
