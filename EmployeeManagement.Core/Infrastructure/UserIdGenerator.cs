using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.Infrastructure;

public class InMemoryUserIdGenerator : IUserIdGenerator
{
    // Khởi tạo _currentId từ 0. Vì Interlocked.Increment sẽ tăng trước, lần đầu tiên trả về 1.
    private int _currentId = 0;

    public int GetNextUserId()
    {
        return Interlocked.Increment(ref _currentId);
    }
}

