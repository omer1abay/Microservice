using MassTransit;
using Microservice.Services.Order.Infrastructure;
using Microservice.Shared.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext dbContext;

        public CourseNameChangedEventConsumer(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await dbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

            orderItems.ForEach(item =>
            {
                item.UpdateOrderItem(context.Message.UpdateName, item.PictureUrl, item.Price);
            });

            //db'ye yansıma
            dbContext.SaveChanges();

        }
    }
}
