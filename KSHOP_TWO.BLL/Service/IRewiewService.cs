using KSHOP_TWO.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface IRewiewService
    {
        Task<bool> AddReview(string UserId, AddReviewRequest requset);
    }
}
