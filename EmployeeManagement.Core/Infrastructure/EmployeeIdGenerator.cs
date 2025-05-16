using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.Infrastructure;

public class InMemoryEmployeeIdGenerator : IEmployeeIdGenerator
{
    // Khởi tạo _currentId từ 0. Vì Interlocked.Increment sẽ tăng trước, lần đầu tiên trả về 1.
    private int _currentId = 0;

    public int GetNextEmployeeId()
    {
        return Interlocked.Increment(ref _currentId);
    }
}

