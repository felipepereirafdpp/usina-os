namespace UsinaOS.Exceptions.Cliente
{
    public class ValidaEmailException : Exception
    {
        public ValidaEmailException() :base("Email informado é invalido") { }

        public ValidaEmailException(string message) : base(message) { }
        public ValidaEmailException(string message, Exception innerException) : base(message, innerException) { }
    }
}
