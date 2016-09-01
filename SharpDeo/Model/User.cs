namespace SharpDeo.Model {
    public class User : TheoricUser {
        public int Id { get; set; }
        public string Salt { get; set; }
        public object GradeOther { get; set; }
        public ServerDateTime Created { get; set; }
        public ServerDateTime Modified { get; set; }
    }
}