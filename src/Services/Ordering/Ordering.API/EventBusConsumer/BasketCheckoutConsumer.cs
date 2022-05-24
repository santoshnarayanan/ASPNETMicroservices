using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
