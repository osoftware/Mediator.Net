﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Mediator.Net.Binding;
using Mediator.Net.Test.CommandHandlers;
using Mediator.Net.Test.Messages;
using Mediator.Net.Test.Middlewares;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.Test.TestPerformance
{
    [Collection("Avoid parallel execution")]
    public class DifferentMediatorSend1000CommandsOneMiddlewareEachPipe : TestBase
    {
        private long milliSeconds = 0;
        private MediatorBuilder _builder;
        public void GivenAMediator()
        {
            ClearBinding();
            _builder = new MediatorBuilder();
            _builder.RegisterHandlers(() =>
            {
                var binding = new List<MessageBinding> { new MessageBinding(typeof(NoWorkCommand), typeof(NoWorkCommandHandler)) };
                return binding;
            })
            .ConfigureGlobalReceivePipe(x =>
                {
                    x.UseNoWorkMiddleware();
                })
                .ConfigureCommandReceivePipe(y =>
                {
                    y.UseNoWorkMiddleware();
                });

        }

        public async Task When1000CommandIsSentByDifferentMediator()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                var mediator = _builder.Build();
                await mediator.SendAsync(new NoWorkCommand());
            }

            sw.Stop();
            milliSeconds = sw.ElapsedMilliseconds;
            Console.WriteLine($"it took {milliSeconds} milliseconds");
        }

        public void ThenItShouldNotTakeMoreThan50MilliSeconds()
        {
            milliSeconds.ShouldBeLessThan(50);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
