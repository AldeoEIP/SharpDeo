namespace SharpDeo.Model {
    public class User {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object GradeOther { get; set; }
        public ServerDateTime Created { get; set; }
        public ServerDateTime Modified { get; set; }
    }
}