namespace EmployeeManagement.Tests.UnitTest;

    // Định nghĩa model theo yêu cầu của API
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public bool IsActive { get; set; }

        // Constructor mặc định
        public User(string username, string password, string fullname, bool isActive)
        {
            Username = username;
            Password = password;
            Fullname = fullname;
            IsActive = isActive;
        }
    }
    
    // Interface cho repository
    public interface IUserRepository
    {
        List<User> GetUsers();
    }

    // Giả lập repository để lấy dữ liệu người dùng
    public class UserRepository : IUserRepository
    {
        public List<User> GetUsers()
        {
            // Giả lập dữ liệu người dùng
            return new List<User>
            {
                new User("user1", "password1", "User One", true),
                new User("user2", "password2", "User Two", false)
            };
        }
    }

