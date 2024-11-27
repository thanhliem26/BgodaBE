using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Service.Admin.IService;

namespace vigo.Service.Admin.Service
{
    public class CheckOutService : ICheckOutService
    {
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public CheckOutService(IUnitOfWorkVigo unitOfWorkVigo)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
        }
        public async Task CheckOutEveryday(DateTime time)
        {
            List<Expression<Func<Booking, bool>>> conditions = new List<Expression<Func<Booking, bool>>>()
            {
                e => e.DeletedDate == null,
                e => e.IsCheckOut == false
            };
            var books = await _unitOfWorkVigo.Bookings.GetAll(conditions);
            foreach (var book in books) {
                if (book.CheckOutDate < time)
                {
                    var room = await _unitOfWorkVigo.Rooms.GetById(book.RoomId);
                    room.Avaiable += 1;
                    book.IsCheckOut = true;
                }
            }
            await _unitOfWorkVigo.Complete();
        }
    }
}
