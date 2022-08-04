using Auth.DTO.Entity;
using Auth.DTO.SeacrhModel.Matter;
using Auth.Enum.Matter;
using Auth.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Service
{
    public class MatterService : IMatterService
    {
        private readonly AuthDbContext _db;
        public MatterService(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddMatter(int userId, string userName, AddMatterModel model)
        {
            await _db.Matters.AddAsync(new Matter
            {
                CreateTime = DateTime.Now,
                CreateUserId = userId,
                CreateUserName = userName,
                Level = (MatterLevelEnum)model.Level,
                ModifieyTime = DateTime.Now,
                ModifieyUserId = userId,
                ModifieyUserName = userName,
                Title = model.Title,
                Type = MatterTypeEnum.NeedMatter,
            });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMatter(int userId, string userName, int id)
        {
            var matter = await _db.Matters.FirstOrDefaultAsync(x => x.Id == id);
            if (matter is null)
                return false;
            matter.Type = MatterTypeEnum.Dustbin;
            matter.ModifieyUserId = userId;
            matter.ModifieyUserName = userName;
            matter.ModifieyTime = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditMatter(int userId, string userName, EditMatterModel model)
        {
            var matter = await _db.Matters.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (matter is null)
                return false;
            matter.Title = model.Title;
            matter.Level = (MatterLevelEnum)model.Level;
            matter.ModifieyUserId = userId;
            matter.ModifieyUserName = userName;
            matter.ModifieyTime = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<(int, List<Matter>)> GetMatterPageList(int userId, MatterSearchModel model)
        {
            var query = _db.Matters.AsNoTracking().Where(x => x.Type == (MatterTypeEnum)model.MatterType && x.CreateUserId == userId);
            var total = await query.CountAsync();
            var result = await query.OrderBy(x => x.Id).Skip(model.Skip).Take(model.Take).ToListAsync();
            return (total, result);
        }

        /// <summary>
        /// 事项已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> MatterRead(int userId, string userName, int id)
        {
            var matter = await _db.Matters.FirstOrDefaultAsync(x => x.Id == id);
            if (matter is null)
                return false;
            matter.Type = MatterTypeEnum.DoneMatter;
            matter.ModifieyUserId = userId;
            matter.ModifieyUserName = userName;
            matter.ModifieyTime = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 垃圾箱处理
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DustbinHandle(int userId, string userName, int id,int type)
        {
            var matter = await _db.Matters.FirstOrDefaultAsync(x => x.Id == id);
            if (matter is null)
                return false;
            matter.Type = (MatterTypeEnum)type;
            matter.ModifieyUserId = userId;
            matter.ModifieyUserName = userName;
            matter.ModifieyTime = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
