using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Domain.User;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Rating;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vigo.Service.Admin.Service
{
    public class RatingService : IRatingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public RatingService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }
        public async Task Approve(List<int> ids, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("rating_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Rating> data = new List<Rating>();
            foreach (int id in ids) {
                data.Add(await _unitOfWorkVigo.Ratings.GetById(id));
            }
            foreach (Rating rating in data) {
                if (rating.Status == false) {
                    rating.Status = true;
                }
                if (!rating.UpdateComment.IsNullOrEmpty()) {
                    rating.Comment = rating.UpdateComment;
                    rating.UpdateComment = string.Empty;
                }

                List<Expression<Func<Rating, bool>>> conditions = new List<Expression<Func<Rating, bool>>>()
                {
                    e => e.RoomId == rating.RoomId
                };
                var rates = await _unitOfWorkVigo.Ratings.GetAll(conditions);
                var room = await _unitOfWorkVigo.Rooms.GetById(rating.RoomId);
                decimal starTemp = rating.Star;
                foreach (var rate in rates)
                {
                    starTemp += rate.Star;
                }
                room.Star = Math.Round(starTemp/(rates.Count() + 1), 1);
            }
            await _unitOfWorkVigo.Complete();
        }

        public async Task Delete(List<int> ids, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("rating_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Rating> data = new List<Rating>();
            DateTime Datenow = DateTime.Now;
            foreach (int id in ids)
            {
                data.Add(await _unitOfWorkVigo.Ratings.GetById(id));
            }
            foreach (Rating rating in data)
            {
                rating.DeletedDate = Datenow;
            }
            await _unitOfWorkVigo.Complete();
        }

        public async Task<PagedResultCustom<RatingDTO>> GetPaging(int page, int perPage, RatingType type, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("rating_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<Rating, bool>>> conditions = new List<Expression<Func<Rating, bool>>>()
            {
                e => e.DeletedDate == null
            };
            if ((int)type == 0)
            {
                conditions.Add(e => e.Status == true);
                var data = await _unitOfWorkVigo.Ratings.GetPaging(conditions,
                                                                   null,
                                                                   null,
                                                                   null,
                                                                   page,
                                                                   perPage);
                List<RatingDTO> result = new List<RatingDTO>();
                foreach (var rating in data.Items)
                {
                    result.Add(new RatingDTO()
                    {
                        Id = rating.Id,
                        Comment = rating.Comment,
                        LastUpdatedDate = rating.LastUpdatedDate,
                        Rate = rating.Star
                    });
                }
                return new PagedResultCustom<RatingDTO>(result, data.TotalRecords, data.PageIndex, data.PageSize);
            }
            if ((int)type == 1)
            {
                conditions.Add(e => e.Status == false || !e.UpdateComment.Equals(string.Empty));
                var data = await _unitOfWorkVigo.Ratings.GetPaging(conditions,
                                                                   null,
                                                                   null,
                                                                   null,
                                                                   page,
                                                                   perPage);
                List<RatingDTO> result = new List<RatingDTO>();
                foreach (var rating in data.Items)
                {
                    result.Add(new RatingDTO()
                    {
                        Id = rating.Id,
                        Comment = rating.UpdateComment.IsNullOrEmpty() ? rating.Comment : rating.UpdateComment,
                        LastUpdatedDate = rating.LastUpdatedDate,
                        Rate = rating.Star
                    });
                }
                return new PagedResultCustom<RatingDTO>(result, data.TotalRecords, data.PageIndex, data.PageSize);
            }
            return new PagedResultCustom<RatingDTO>(new List<RatingDTO>(), 0, 0, 0);
        }

        public async Task UnApprove(List<int> ids, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("rating_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Rating> data = new List<Rating>();
            foreach (int id in ids)
            {
                var rate = await _unitOfWorkVigo.Ratings.GetById(id);
                if (!rate.UpdateComment.IsNullOrEmpty())
                {
                    rate.UpdateComment = string.Empty;
                    continue;
                }
                data.Add(rate);
            }
            _unitOfWorkVigo.Ratings.DeleteRange(data);
            await _unitOfWorkVigo.Complete();
        }
    }
}
