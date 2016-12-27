﻿using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using NUnit.Framework;
using TestStack.BDDfy;

namespace Mediator.Net.Test
{
    class UselessMiddlewareShouldNotBeExecuted : TestBase
    {
        private IMediator _mediator;
        public void GivenAMediator()
        {
            var builder = new MediatorBuilder();
            var receivePipe = builder.RegisterHandlersFor(this.GetType().Assembly)
                .BuildReceivePipe<IReceiveContext<IMessage>, IMessage>(x =>
            {
                x.UseConsoleLogger2();
                x.UseConsoleLogger1();
                x.UseUselessMiddleware();
            })
            .Build();
            
            _mediator = new Mediator(receivePipe, null);
        }

        public async Task WhenACommandIsSent()
        {
            await _mediator.SendAsync(new TestBaseCommand(Guid.NewGuid()));
        }

        public void ThenItShouldReachTheRightHandler()
        {
            
        }

        [Test]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
