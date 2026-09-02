namespace UsinaOS.Exceptions.Cliente
{
    public class ValidaCnpjException : Exception
    {
        public ValidaCnpjException() : base("CNPJ invalido") { }
        public ValidaCnpjException(string message) : base(message) { }
        public ValidaCnpjException(string message, Exception innerException) : base(message, innerException) { }

    }
}
