namespace UsinaOS.Exceptions.Cliente
{
    public class ValidaCepException : Exception
    {
        public ValidaCepException() : base("O Cep informado é invalido") { }
        public ValidaCepException(string message) : base(message) { }

        public ValidaCepException(string message, Exception innerException) : base(message,innerException) { }
    }
}
