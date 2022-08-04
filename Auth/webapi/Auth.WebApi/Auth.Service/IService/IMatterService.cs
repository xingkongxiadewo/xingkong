using Auth.DTO.Entity;
using Auth.DTO.SeacrhModel.Matter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.IService
{
    public interface IMatterService
    {
        /// <summary>
        /// 获取事项列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<(int, List<Matter>)> GetMatterPageList(int userId, MatterSearchModel searchModel);
        /// <summary>
        /// 新增事项
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddMatter(int userId, string userName, AddMatterModel model);

        /// <summary>
        /// 修改事项
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> EditMatter(int userId,string userName, EditMatterModel model);

        /// <summary>
        /// 垃圾箱处理
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DustbinHandle(int userId, string userName, int id,int type);
    }
}
