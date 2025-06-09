using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Interfaces;
public interface IUserRepository
{
    //ユーザ情報を追加する
    
    //追加
    Task AddUser(User user);
    //検索
    Task<User?> GetUserByEmail(string email);
    Task<User> GetUserById(int id);
    //一覧検索
    Task<IEnumerable<User>> GetAll();
    //更新
    Task UpdateUser(User user);
    //削除
    Task DelUser(int id);  
}
