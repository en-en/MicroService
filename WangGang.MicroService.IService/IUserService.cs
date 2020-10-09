using System;
using System.Collections.Generic;
using WangGang.MicroService.Model;

namespace WangGang.MicroService.IService
{
    public interface IUserService
    {
        User FindSingle(int id);

        IEnumerable<User> FindUsers();
    }
}
