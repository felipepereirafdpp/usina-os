namespace UsinaOS.Exceptions.Cliente
{
    public class ValidaBuscaClienteException: Exception
    {
        public ValidaBuscaClienteException() : base("Cliente nao encontrado") { }
        public ValidaBuscaClienteException(string message) : base(message) { }

    }
}
